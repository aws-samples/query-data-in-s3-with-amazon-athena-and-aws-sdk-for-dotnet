using AthenaNetCore.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AthenaNetCore.BusinessLogic.Repositories
{
    /// <summary>
    /// COVID-19 status and testing progress
    /// </summary>
    public interface ICovidTestingRepository : IBaseRepository
    {
        /// <summary>
        /// Get result of a giving query by ID
        /// </summary>
        /// <param name="queryId"></param>
        /// <returns></returns>
        Task<IEnumerable<CovidTestingStatesDaily>> GetTestingQueryResult(string queryId);

        /// <summary>
        /// Provide last 100 days records of COVID-19 status and testing progress in by State
        /// </summary>
        /// <param name="stateAbbreviation">2 diggits State Abbreviation </param>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        Task<string> ProgressAsync(string stateAbbreviation);

        /// <summary>
        /// Provide last 500 records of COVID-19 status and testing progress in USA
        /// </summary>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        Task<IEnumerable<CovidTestingStatesDaily>> ProgressAsync();

        /// <summary>
        /// Provide COVID-19 status and progress by a given date
        /// </summary>
        /// <param name="date">date to filter</param>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        Task<string> ProgressAsync(DateTime date);
    }
}