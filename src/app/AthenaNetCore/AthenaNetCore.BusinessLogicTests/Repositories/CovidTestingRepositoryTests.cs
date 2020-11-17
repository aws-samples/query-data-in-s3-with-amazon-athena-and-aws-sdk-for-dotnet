/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * SPDX-License-Identifier: MIT-0
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify,
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using Xunit;
using AthenaNetCore.BusinessLogic.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using AthenaNetCore.BusinessLogic.Entities;
using System.Linq;

namespace AthenaNetCore.BusinessLogicIntegrationTests.Repositories.Tests
{
    public class CovidTestingRepositoryTests
    {
        [Fact()]
        public async void Test_Yesterday_Covid_Progress_In_USA()
        {
            string result;
            var filterDate = DateTime.Now.AddDays(-1);
            using (ICovidTestingRepository repo = new CovidTestingRepository())
            {
                result = await repo.ProgressAsync(filterDate);
            }

            Assert.NotNull(result);
        }

        [Fact()]
        public async void Test_Last_500_Covid_Records()
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
        public async void Test_Covid_Progress_by_State()
        {
            string result;
            var filterState = "CA";
            using (ICovidTestingRepository repo = new CovidTestingRepository())
            {
                result = await repo.ProgressAsync(filterState);
            }

            Assert.NotNull(result);
        }
    }
}