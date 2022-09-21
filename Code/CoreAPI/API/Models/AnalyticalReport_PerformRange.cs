using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class AnalyticalReport_PerformRange
    {
        public int PK_PerformerID { get; set; }
        public string TypeName { get; set; }
        public string PerformName { get; set; }
        public int? LowerValue { get; set; }
        public int? UpperValue { get; set; }
        public int? OrderBy { get; set; }
    }
}
