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
    public class HospitalRepository : BaseRepository
    {

        public async Task<IEnumerable<HospitalBeds>> ListHospitalsBedsAsync()
        {   
            return await AmazonAthenaClient.QueryAsync<HospitalBeds>("SELECT * FROM  \"covid-19\".\"hospital_beds\" LIMIT 1000;");
        }

    }
}

