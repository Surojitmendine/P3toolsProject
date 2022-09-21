using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_SaleTransDetails_Uploaded
    {
        public int PK_SalesTransactionID { get; set; }
        public int? FK_ClientID { get; set; }
        public bool? IsProcessed { get; set; }
        public DateTime? SaleDate { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string HQ { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public decimal? SalesQTY { get; set; }
        public decimal? FreeSampleQTY { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? NetAmount { get; set; }
    }
}
