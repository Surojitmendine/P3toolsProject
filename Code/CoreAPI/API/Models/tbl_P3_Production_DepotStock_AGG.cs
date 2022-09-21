using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Production_DepotStock_AGG
    {
        public int PK_AggDepotStockID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? FK_CreatedByID { get; set; }
        public DateTime? StockDate { get; set; }
        public int? ForMonth { get; set; }
        public int? ForYear { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public decimal? ClosingStockQTY { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
