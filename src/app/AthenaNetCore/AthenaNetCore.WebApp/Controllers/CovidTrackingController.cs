using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AthenaNetCore.BusinessLogic.Entities;
using AthenaNetCore.BusinessLogic.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AthenaNetCore.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CovidTrackingController : Controller
    {

        public CovidTrackingController(ICovidTestingRepository testingRepository, IHospitalRepository hospitalRepository)
        {
            TestingRepository = testingRepository;
            HospitalRepository = hospitalRepository;
        }

        public ICovidTestingRepository TestingRepository { get; }
        public IHospitalRepository HospitalRepository { get; }


        [HttpGet("hospitals")]
        public async Task<IEnumerable<HospitalBeds>> Hospitals()
        {
            return await HospitalRepository.ListHospitalsBedsAsync();
        }
    }
}
