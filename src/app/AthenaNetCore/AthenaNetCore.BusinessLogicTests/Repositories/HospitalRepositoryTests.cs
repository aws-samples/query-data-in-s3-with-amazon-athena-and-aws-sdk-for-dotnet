using Xunit;
using AthenaNetCore.BusinessLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AthenaNetCore.BusinessLogic.Entities;

namespace AthenaNetCore.BusinessLogic.Repositories.Tests
{
    public class HospitalRepositoryTests
    {
        [Fact()]
        public async void CountCaseTest()
        {
            IEnumerable<HospitalBeds> result;
            using (var repo = new HospitalRepository())
            {
                result = await repo.ListHospitalsBedsAsync();
            }

            Assert.Equal(999, result.Count());
        }
    }
}