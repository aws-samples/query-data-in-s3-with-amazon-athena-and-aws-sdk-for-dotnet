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
            string result;
            var filterDate = DateTime.Now.AddDays(-1);
            using (ICovidTestingRepository repo = new CovidTestingRepository())
            {
                result = await repo.ProgressAsync(filterDate);
            }


            Assert.NotNull(result);
            Assert.True(Guid.TryParse(result, out Guid queryId), "It's not a valid Athena Query Execution Id");
            Assert.NotEqual(Guid.Empty, queryId);
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
            string result;
            var filterState = "CA";
            using (ICovidTestingRepository repo = new CovidTestingRepository())
            {
                result = await repo.ProgressAsync(filterState);
            }

            Assert.NotNull(result);
            Assert.True(Guid.TryParse(result, out Guid queryId), "It's not a valid Athena Query Execution Id");
            Assert.NotEqual(Guid.Empty, queryId);
        }
    }
}