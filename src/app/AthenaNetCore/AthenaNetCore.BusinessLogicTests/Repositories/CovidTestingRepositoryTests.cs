using Xunit;
using AthenaNetCore.BusinessLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using AthenaNetCore.BusinessLogic.Entities;
using System.Linq;

namespace AthenaNetCore.BusinessLogic.Repositories.Tests
{
    public class CovidTestingRepositoryTests
    {
        [Fact()]
        public async void SatesTestingProgressAsyncTest()
        {
            IEnumerable<CovidTestingStatesDaily> result;
            var filterDate = DateTime.Now.AddDays(-1);
            using (ICovidTestingRepository repo = new CovidTestingRepository())
            {
                result = await repo.ProgressByDateAsync(filterDate);
            }


            Assert.NotNull(result);
            Assert.Equal(filterDate.ToShortDateString(), result.FirstOrDefault()?.Date.ToShortDateString());
        }

        [Fact()]
        public async void ProgressAsyncTest()
        {
            IEnumerable<CovidTestingStatesDaily> result;
            using (ICovidTestingRepository repo = new CovidTestingRepository())
            {
                result = await repo.ProgressAsync();
            }

            Assert.NotNull(result);
            Assert.Equal(500, result.Count());
        }

        [Fact()]
        public async void PorgressByStateAsyncTest()
        {
            IEnumerable<CovidTestingStatesDaily> result;
            var filterState = "CA";
            using (ICovidTestingRepository repo = new CovidTestingRepository())
            {
                result = await repo.PorgressByStateAsync(filterState);
            }

            Assert.NotNull(result);
            Assert.Equal(filterState, result.FirstOrDefault()?.StateAbbreviation);
        }
    }
}