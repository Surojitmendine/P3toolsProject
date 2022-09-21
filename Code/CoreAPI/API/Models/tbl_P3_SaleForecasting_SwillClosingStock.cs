using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_SaleForecasting_SwillClosingStock
    {
        public int PK_SwillID { get; set; }
        public bool? IsProcessed { get; set; }
        public int? ForYear { get; set; }
        public int? ForMonth { get; set; }
        public DateTime? SyncDate { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public decimal? ClosingStockQTY { get; set; }
    }
}
