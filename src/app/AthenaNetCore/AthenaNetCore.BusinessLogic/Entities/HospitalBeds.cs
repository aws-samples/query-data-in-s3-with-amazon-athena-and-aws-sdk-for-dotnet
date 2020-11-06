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
    public class HospitalBeds
    {
        [AthenaColumn("hospital_name")]
        public string Name { get; set; }

        [AthenaColumn("hospital_type")]
        public string HospitalType { get; set; }

        [AthenaColumn("hq_address")]
        public string HqAddress { get; set; }

        [AthenaColumn("hq_address1")]
        public string HqAddress1 { get; set; }
        
        [AthenaColumn("hq_city")]
        public string HqCity { get; set; }

        [AthenaColumn("hq_state")]
        public string HqState { get; set; }

        [AthenaColumn("hq_zip_code")]
        public string HqZipCode { get; set; }

        [AthenaColumn("county_name")]
        public string CountyName { get; set; }

        [AthenaColumn("state_name")]
        public string StateName { get; set; }

        [AthenaColumn("state_fips")]
        public string StateFips { get; set; }

        [AthenaColumn("num_licensed_beds")]
        public int LicencedBeds { get; set; }

        [AthenaColumn("num_staffed_beds")]
        public int StaffedBeds { get; set; }

        [AthenaColumn("num_icu_beds")]
        public int IcuBeds { get; set; }

        [AthenaColumn("potential_increase_in_bed_capac")]
        public int PotentialIncreaseInBedCapac { get; set; }

        //[AthenaColumn("latitude")]
        public double Latitude { get; set; }

        //[AthenaColumn("longtitude")]
        public double Longtitude { get; set; }
    }
}
