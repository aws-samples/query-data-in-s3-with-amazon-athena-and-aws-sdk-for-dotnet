using Amazon.Athena;
using Amazon.Athena.Model;
using AthenaNetCore.BusinessLogic.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AthenaNetCore.BusinessLogic.Extentions
{
    internal static class AmazonAthenaClientExtention
    {
        private const int SLEEP_AMOUNT_IN_MS = 1000;
        private static readonly string S3_RESULT = Environment.GetEnvironmentVariable("S3_RESULT") ?? "s3://athena-results-netcore-s3bucket-fk6joy3h5yof/athena/results/";

        /// <summary>
        /// Execute an SQL query using Amazon Athena, wait for the result of the query 
        /// and map the result to a C# Entity object. If the Amazon Athena did not complete 
        /// processing the giving query and the timeout is reached, this method 
        /// will throw exception and return the QueryExecutionId that 
        /// can be used later to get the result
        /// </summary>
        /// <typeparam name="T">Type of the entity result</typeparam>
        /// <param name="athenaClient">Instance of Amazon Athena Client</param>
        /// <param name="queryString">SQL query</param>
        /// <param name="timeoutInMinutes"> default 2 minutes: timeout in minutes for the application to abort waiting.</param>
        /// <returns>The enumerator, which supports a simple iteration over a collection of a specified type</returns>
        public static async Task<IEnumerable<T>> QueryAsync<T>(this IAmazonAthena athenaClient, string queryString, int timeoutInMinutes = 2) where T : new()
        {
            if (athenaClient == null || string.IsNullOrEmpty(queryString))
            {
                return default;
            }

            var qry = await athenaClient.StartQueryExecutionAsync(new StartQueryExecutionRequest
            {
                QueryString = queryString,
                ResultConfiguration = new ResultConfiguration
                {
                    OutputLocation = S3_RESULT
                }
            });

            await WaitForQueryToComplete(athenaClient, qry.QueryExecutionId, DateTime.Now.AddMinutes(timeoutInMinutes));

            return await ProcessQueryResultsAsync<T>(athenaClient, qry.QueryExecutionId);
        }

        /// <summary>
        /// Execute an SQL query using Amazon Athena and return QueryExecutionId 
        /// without waiting for the result, the QueryExecutionId can be used later to get the result. 
        /// </summary>
        /// <param name="athenaClient">Instance of Amazon Athena Client</param>
        /// <param name="queryString">SQL query</param>
        /// <returns>Eexecution Id to track the query progress</returns>
        public static async Task<string> QueryAndGoAsync(this IAmazonAthena athenaClient, string queryString)
        {
            var qry = await athenaClient.StartQueryExecutionAsync(new StartQueryExecutionRequest
            {
                QueryString = queryString,
                ResultConfiguration = new ResultConfiguration
                {
                    OutputLocation = S3_RESULT
                }
            });

            return qry.QueryExecutionId;
        }

        /// <summary>
        /// Retive the query result and return it as a collection of a specified type
        /// </summary>
        /// <typeparam name="T">Type of the entity result</typeparam>
        /// <param name="athenaClient">Instance of Amazon Athena Client</param>
        /// <param name="queryExecutionId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> ProcessQueryResultsAsync<T>(this IAmazonAthena athenaClient, string queryExecutionId) where T : new()
        {
            if (athenaClient == null || string.IsNullOrEmpty(queryExecutionId))
            {
                return default;
            }

            var results = new List<T>();
            try
            {
                // Max Results can be set but if its not set,
                // it will choose the maximum page size
                // As of the writing of this code, the maximum value is 1000
                var getQueryResultsRequest = new GetQueryResultsRequest { QueryExecutionId = queryExecutionId };
                var getQueryResultsResults = await athenaClient.GetQueryResultsAsync(getQueryResultsRequest);

                var columnInfoList = getQueryResultsResults.ResultSet.ResultSetMetadata.ColumnInfo;
                var rows = getQueryResultsResults.ResultSet.Rows;
                var columnPositionMap = MapColumnsPositions(rows[0].Data, columnInfoList);
                rows.RemoveAt(0);
                //Debug.WriteLine(string.Join(" | ", columnsPosition.Data.Select(s => s.VarCharValue)));
                //Debug.WriteLine(string.Join(" | ", columnInfoList.Select(s => s.Type)));

                foreach (var row in rows)
                {
                    results.Add(ProcessRow<T>(row.Data, columnPositionMap));
                }

            }
            catch (AmazonAthenaException e)
            {
                Debug.WriteLine(e);
            }

            return results;
        }

        /// <summary>
        /// Check if the query still running and return FALSE n case of completion. 
        /// Otherwise It will return TRUE or throw an exception in case of Failed or Cancelled
        /// </summary>
        /// <param name="athenaClient"></param>
        /// <param name="queryExecutionId"></param>
        /// <returns></returns>
        public static async Task<bool> IsTheQueryStillRunning(this IAmazonAthena athenaClient, string queryExecutionId)
        {
            var getQueryExecutionRequest = new GetQueryExecutionRequest { QueryExecutionId = queryExecutionId };
            bool isQueryStillRunning = true;
            var getQueryExecutionResponse = await athenaClient.GetQueryExecutionAsync(getQueryExecutionRequest);
            var queryState = getQueryExecutionResponse.QueryExecution.Status.State;
            if (queryState == QueryExecutionState.FAILED)
            {
                throw new Exception("Query Failed to run with Error Message: " + getQueryExecutionResponse.QueryExecution.Status.StateChangeReason);
            }
            else if (queryState == QueryExecutionState.CANCELLED)
            {
                throw new Exception("Query was cancelled.");
            }
            else if (queryState == QueryExecutionState.SUCCEEDED)
            {
                isQueryStillRunning = false;
            }

            Debug.WriteLine("Current Status is: " + queryState);
            return isQueryStillRunning;
        }

        /// <summary>
        /// Wait for Amazon Athena to complete execution of the query. If It is not completed until the timeout, then It should throw exception.
        /// </summary>
        /// <param name="athenaClient">Amazon Athena Client instance</param>
        /// <param name="queryExecutionId">Eexecution Id to track the query progress</param>
        /// <param name="timeout">max DateTime to wait before abort</param>
        /// <returns></returns>
        private static async Task WaitForQueryToComplete(IAmazonAthena athenaClient, string queryExecutionId, DateTime timeout)
        {
            bool isQueryStillRunning = true;
            while (isQueryStillRunning && DateTime.Now <= timeout)
            {
                isQueryStillRunning = await athenaClient.IsTheQueryStillRunning(queryExecutionId);
                if (isQueryStillRunning)
                {
                    // Sleep an amount of time before retrying again.
                    await Task.Delay(SLEEP_AMOUNT_IN_MS);
                }
            }

            if (isQueryStillRunning && DateTime.Now > timeout)
            {
                throw new AmazonAthenaException("Timeout: Amazon Athena still processing your query, use the RequestId to get the result later.")
                {
                    RequestId = queryExecutionId
                };
            }
        }

        /// <summary>
        /// Map the columns with tier positions on the first row and the columns Info
        /// </summary>
        /// <param name="columnsPositions">first row columns position</param>
        /// <param name="columnInfoList">columns info</param>
        /// <returns></returns>
        private static IReadOnlyDictionary<string, ColumnPositionInfo> MapColumnsPositions(IReadOnlyList<Datum> columnsPositions, IReadOnlyList<ColumnInfo> columnInfoList)
        {
            var result = new Dictionary<string, ColumnPositionInfo>();

            if (columnsPositions == null || columnInfoList == null)
            {
                return result;
            }

            for (int i = 0; i < columnsPositions.Count; i++)
            {
                var column = columnsPositions[i].VarCharValue.ToLower();
                
                result.Add(column, new ColumnPositionInfo
                {
                    IndexPosition = i,
                    ColumnInfo = columnInfoList.FirstOrDefault(f => f.Name.ToLower() == column)
                });
            }

            return result;
        }

        /// <summary>
        /// Compute the row data to entity object
        /// </summary>
        /// <typeparam name="T">Specified entity object type</typeparam>
        /// <param name="columnsData">Collection of columns data in row</param>
        /// <param name="columnsPositionMap">Map of columns position and info for each data</param>
        /// <returns></returns>
        private static T ProcessRow<T>(IReadOnlyList<Datum> columnsData, IReadOnlyDictionary<string, ColumnPositionInfo> columnsPositionMap) where T : new()
        {
            //Debug.WriteLine(string.Join(" | ", columnsData.Select(s => s.VarCharValue)));

            var entityItem = new T();

            foreach (var prop in entityItem.GetType().GetProperties())
            {
                var propColumnName = prop.Name.ToLower();
                var att = System.Attribute.GetCustomAttribute(prop, typeof(AthenaColumnAttribute));
                if (att is AthenaColumnAttribute attribute)
                {
                    propColumnName = attribute.ColumnName;
                }

                if (columnsPositionMap.ContainsKey(propColumnName))
                {
                    var mapped = columnsPositionMap[propColumnName];
                    var athenaColumnInfo = mapped.ColumnInfo;
                    var i = mapped.IndexPosition;
                    //For more detail about Amazon Athena Data Type, check: https://docs.aws.amazon.com/athena/latest/ug/data-types.html
                    if (athenaColumnInfo.Type == "integer" || athenaColumnInfo.Type == "tinyint" || athenaColumnInfo.Type == "smallint")
                    {
                        prop.SetValue(entityItem, Convert.ToInt32(columnsData[i]?.VarCharValue));
                    }
                    else if (athenaColumnInfo.Type == "bigint")
                    {
                        prop.SetValue(entityItem, Convert.ToInt64(columnsData[i]?.VarCharValue));
                    }
                    else if (athenaColumnInfo.Type == "double" || athenaColumnInfo.Type == "float")
                    {
                        prop.SetValue(entityItem, Convert.ToDouble(columnsData[i]?.VarCharValue));
                    }
                    else if (athenaColumnInfo.Type == "decimal")
                    {
                        prop.SetValue(entityItem, Convert.ToDecimal(columnsData[i]?.VarCharValue));
                    }
                    else if (athenaColumnInfo.Type == "date" || athenaColumnInfo.Type == "timestamp")
                    {
                        prop.SetValue(entityItem, Convert.ToDateTime(columnsData[i]?.VarCharValue));
                    }
                    else
                    {
                        prop.SetValue(entityItem, columnsData[i]?.VarCharValue);
                    }
                }
            }

            return entityItem;
        }
    }
}
