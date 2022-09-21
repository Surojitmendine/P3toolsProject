using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_SecondarySaleTransDetails_ClosingStock
    {
        public int PK_SSClosingStockID { get; set; }
        public int? FK_ClientID { get; set; }
        public bool? IsProcessed { get; set; }
        public int? ForYear { get; set; }
        public int? ForMonth { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string HQ { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public string UOM { get; set; }
        public decimal? ClosingStockQTY { get; set; }
    }
}
