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
using AthenaNetCore.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AthenaNetCore.BusinessLogic.Repositories
{
    /// <summary>
    /// COVID-19 status and testing progress
    /// </summary>
    public interface ICovidTestingRepository : IBaseRepository
    {
        /// <summary>
        /// Get result of a giving query by ID
        /// </summary>
        /// <param name="queryId"></param>
        /// <returns></returns>
        Task<IEnumerable<CovidTestingStatesDaily>> GetTestingQueryResultAsync(string queryId);

        /// <summary>
        /// Provide last 100 days records of COVID-19 status and testing progress in by State
        /// </summary>
        /// <param name="stateAbbreviation">2 diggits State Abbreviation </param>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        Task<string> ProgressAsync(string stateAbbreviation);

        /// <summary>
        /// Provide last 500 records of COVID-19 status and testing progress in USA
        /// </summary>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        Task<IEnumerable<CovidTestingStatesDaily>> ProgressAsync();

        /// <summary>
        /// Provide COVID-19 status and progress by a given date
        /// </summary>
        /// <param name="date">date to filter</param>
        /// <returns>List of COVID-19' numbers of testing, positive, negative, and death </returns>
        Task<string> ProgressAsync(DateTime date);
    }
}