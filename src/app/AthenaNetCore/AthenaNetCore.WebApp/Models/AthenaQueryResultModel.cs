using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AthenaNetCore.WebApp.Models
{
    public class AthenaQueryResultModel
    {
        public string QueryId { get; set; }
        public bool IsStillRunning { get; set; }
        public string Message { get; set; } 
    }
}
