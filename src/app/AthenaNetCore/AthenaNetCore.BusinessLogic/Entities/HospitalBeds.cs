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
