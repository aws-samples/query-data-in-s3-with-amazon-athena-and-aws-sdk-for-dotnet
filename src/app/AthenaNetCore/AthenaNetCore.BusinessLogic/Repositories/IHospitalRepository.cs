using AthenaNetCore.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AthenaNetCore.BusinessLogic.Repositories
{
    public interface IHospitalRepository : IDisposable
    {
        Task<IEnumerable<HospitalBeds>> ListHospitalsBedsAsync();
    }
}