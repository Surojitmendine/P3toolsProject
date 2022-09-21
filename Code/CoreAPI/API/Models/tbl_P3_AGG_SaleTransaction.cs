using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_AGG_SaleTransaction
    {
        public int PK_AGG_SaleID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? FK_ClientID { get; set; }
        public bool? IsProcessed { get; set; }
        public DateTime? SaleDate { get; set; }
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
        public decimal? TotalSalesQTY { get; set; }
        public decimal? TotalFreeSampleQTY { get; set; }
        public decimal? GrossAmount { get; set; }
        public decimal? TotalDiscountAmount { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public decimal? TotalNetAmount { get; set; }
        public string ForecastingType { get; set; }
        public decimal? ClosingStockQTY { get; set; }
    }
}
