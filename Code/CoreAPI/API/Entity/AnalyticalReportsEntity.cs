using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public class AnalyticalReportsEntity
    {
        #region -- Depot Replenishment Indent Summary --
        public class DepotReplenishmentIndentSummary
        {
            public int ForYear { get; set; }
            public string ForMonth { get; set; }
            public string DivisionName { get; set; }
            public string DepotName { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public decimal FinalProjection { get; set; }
            public decimal ClosingStock { get; set; }
            public decimal LTFactor { get; set; }
            public decimal DepotReplenishmentIndent { get; set; }
            public decimal CumulativeDepotSentQTY { get; set; }
            public decimal PendingQTY { get; set; }               
        }
        #endregion

    }
}
