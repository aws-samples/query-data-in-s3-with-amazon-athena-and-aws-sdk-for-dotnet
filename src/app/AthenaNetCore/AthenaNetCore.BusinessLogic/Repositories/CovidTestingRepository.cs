/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * SPDX-License-Identifier: MIT-0
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify,
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using AthenaNetCore.BusinessLogic.Entities;
using AthenaNetCore.BusinessLogic.Extentions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AthenaNetCore.BusinessLogic.Repositories
{
    public class CovidTestingRepository : BaseRepository, ICovidTestingRepository
    {
        const string quote = "\"";
        private readonly string baseQuery = $@"
                SELECT 
                       DATE_PARSE((SUBSTR(sta.date,1,4) || '-' || SUBSTR(sta.date,5,2) || '-' || SUBSTR(sta.date,7,2)),'%Y-%m-%d') as date,
                       positive,
                       negative,
                       pending,
                       hospitalized,
                       death,
                       total,
                       deathincrease,
                       hospitalizedincrease,
                       negativeincrease,
                       positiveincrease,
                       sta.state AS state_abbreviation,
                       abb.state 
                FROM {quote}covid-19{quote}.{quote}covid_testing_states_daily{quote} sta
                JOIN {quote}covid-19{quote}.{quote}us_state_abbreviations{quote} abb ON sta.state = abb.abbreviation";

        /// <summary>
        /// Provide last 500 records of COVID-19 status and progress in USA
        /// </summary>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        public async Task<IEnumerable<CovidTestingStatesDaily>> ProgressAsync()
        {
            var queryString = $@"{baseQuery}
                                ORDER BY sta.date DESC, sta.state ASC 
                                LIMIT 500";

            return await AmazonAthenaClient.QueryAsync<CovidTestingStatesDaily>(queryString);
        }

        /// <summary>
        /// Provide COVID-19 status and progress by a given date
        /// </summary>
        /// <param name="date">date to filter</param>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        public Task<string> ProgressAsync(DateTime date)
        {
            // '{date:yyyyMMdd}' is a simplified code of date.ToString("yyyyMMdd")
            var queryString = $@"{baseQuery} 
                    WHERE sta.date ='{date:yyyyMMdd}'
                    ORDER BY sta.state";

            return AmazonAthenaClient.QueryAndGoAsync(queryString);
        }


        /// <summary>
        /// Provide last 100 days records of COVID status and progress in by State
        /// </summary>
        /// <param name="stateAbbreviation">2 diggits State Abbreviation </param>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        public Task<string> ProgressAsync(string stateAbbreviation)
        {
            if (string.IsNullOrWhiteSpace(stateAbbreviation) || stateAbbreviation.Length > 2) return default;

            var queryString = $@"{baseQuery}
                    WHERE sta.state ='{stateAbbreviation}'
                    ORDER BY sta.date DESC
                    LIMIT 100";

            return AmazonAthenaClient.QueryAndGoAsync(queryString);
        }

        public Task<IEnumerable<CovidTestingStatesDaily>> GetTestingQueryResult(string queryId)
        {
            if (string.IsNullOrWhiteSpace(queryId)) return default;

            return AmazonAthenaClient.ProcessQueryResultsAsync<CovidTestingStatesDaily>(queryId);
        }
    }
}
