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
