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
using System;
using System.Collections.Generic;
using System.Text;

namespace AthenaNetCore.BusinessLogic.Entities
{
    public class CovidTestingStatesDaily
    {

        /// <summary>
        /// reporting date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// US State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// State Abbreviation
        /// </summary>
        [AthenaColumn("state_abbreviation")]
        public string StateAbbreviation { get; set; }

        /// <summary>
        /// number of positive cases
        /// </summary>
        public double Positive { get; set; }

        /// <summary>
        /// number of negative cases
        /// </summary>
        public double Negative { get; set; }

        /// <summary>
        /// tests pending results
        /// </summary>
        public double Pending { get; set; }

        /// <summary>
        /// number of hospitalized patients
        /// </summary>
        public double Hospitalized { get; set; }

        /// <summary>
        /// number of deaths
        /// </summary>
        public double Death { get; set; }

        /// <summary>
        /// number of deaths
        /// </summary>
        [AthenaColumn("total")]
        public double TotalTested { get; set; }

        /// <summary>
        /// increase in deaths vs previous day
        /// </summary>
        public double Deathincrease { get; set; }

        /// <summary>
        /// increase in hospitalized patients vs previous day
        /// </summary>
        public double Hospitalizedincrease { get; set; }

        /// <summary>
        /// increase in negative cases vs previous day
        /// </summary>
        public double Negativeincrease { get; set; }

        /// <summary>
        /// increase in positive cases vs previous day
        /// </summary>
        public double Positiveincrease { get; set; }
    }
}
