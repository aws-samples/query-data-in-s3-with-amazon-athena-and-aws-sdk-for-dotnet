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

            return await base.AmazonAthenaClient.QueryAsync<CovidTestingStatesDaily>(queryString);
        }

        /// <summary>
        /// Provide COVID-19 status and progress by a given date
        /// </summary>
        /// <param name="date">date to filter</param>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        public async Task<IEnumerable<CovidTestingStatesDaily>> ProgressByDateAsync(DateTime date)
        {
            // '{date:yyyyMMdd}' is a simplified code of date.ToString("yyyyMMdd")
            var queryString = $@"{baseQuery} 
                    WHERE sta.date ='{date:yyyyMMdd}'
                    ORDER sta.state";

            return await base.AmazonAthenaClient.QueryAsync<CovidTestingStatesDaily>(queryString);
        }


        /// <summary>
        /// Provide last 100 days records of COVID status and progress in by State
        /// </summary>
        /// <param name="stateAbbreviation">2 diggits State Abbreviation </param>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        public async Task<IEnumerable<CovidTestingStatesDaily>> PorgressByStateAsync(string stateAbbreviation)
        {
            if (string.IsNullOrWhiteSpace(stateAbbreviation) || stateAbbreviation.Length > 2)
            {
                return default;
            }

            var queryString = $@"{baseQuery}
                    WHERE sta.state ='{stateAbbreviation}'
                    ORDER BY sta.date DESC
                    LIMIT 100";

            return await base.AmazonAthenaClient.QueryAsync<CovidTestingStatesDaily>(queryString);
        }
    }
}
