using AthenaNetCore.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AthenaNetCore.BusinessLogic.Repositories
{
    public interface IHospitalRepository : IBaseRepository
    {
        Task<string> HospitalsBedsAsync();
        Task<IEnumerable<HospitalBeds>> HospitalsBedsAsync(string queryId);
        Task<IEnumerable<HospitalBeds>> HospitalsBedsWaitResultAsync();
    }
}