using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Production_DepotStock_Raw
    {
        public int PK_DepotStockRawID { get; set; }
        public DateTime? StockDate { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public string BatchNo { get; set; }
        public string ProductGroup { get; set; }
        public decimal? ClosingStockQTY { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
