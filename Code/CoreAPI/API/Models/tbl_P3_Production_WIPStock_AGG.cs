using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Production_WIPStock_AGG
    {
        public int PK_WIPStockAGGID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? FK_CreatedByID { get; set; }
        public DateTime? StockDate { get; set; }
        public int? ForMonth { get; set; }
        public int? ForYear { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? WIPStock_QTY { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
