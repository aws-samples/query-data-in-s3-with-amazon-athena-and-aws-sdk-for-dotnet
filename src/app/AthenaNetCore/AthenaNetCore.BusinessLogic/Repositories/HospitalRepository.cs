using Amazon;
using Amazon.Athena;
using Amazon.Athena.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AthenaNetCore.BusinessLogic.Extentions;
using AthenaNetCore.BusinessLogic.Entities;

namespace AthenaNetCore.BusinessLogic.Repositories
{
    public class HospitalRepository : BaseRepository, IHospitalRepository
    {

        private const string QUERY_HOSPITALS_MOST_AFFECTED = "SELECT * FROM  \"covid-19\".\"hospital_beds\" ORDER BY potential_increase_in_bed_capac LIMIT 500;";

        public Task<IEnumerable<HospitalBeds>> HospitalsBedsWaitResultAsync()
        {
            return AmazonAthenaClient.QueryAsync<HospitalBeds>(QUERY_HOSPITALS_MOST_AFFECTED);
        }

        public Task<string> HospitalsBedsAsync()
        {
            return AmazonAthenaClient.QueryAndGoAsync(QUERY_HOSPITALS_MOST_AFFECTED);
        }


        public Task<IEnumerable<HospitalBeds>> HospitalsBedsAsync(string queryId)
        {
            if (string.IsNullOrWhiteSpace(queryId))
            {
                return default;
            }

            return AmazonAthenaClient.ProcessQueryResultsAsync<HospitalBeds>(queryId);
        }
    }
}

