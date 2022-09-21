using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Production_WIPStock_Raw
    {
        public int PK_WIPStockRawID { get; set; }
        public DateTime? StockDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BatchNo { get; set; }
        public decimal? WIPStock_QTY { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
