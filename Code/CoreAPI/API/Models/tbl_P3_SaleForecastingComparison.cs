using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_SaleForecastingComparison
    {
        public int PK_SaleComparisonID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? FK_CreatedByID { get; set; }
        public int? ForecastingForMonth { get; set; }
        public int? ForecastingForYear { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string HQ { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public decimal? Logistics_ProjectionSalesQTY { get; set; }
        public decimal? Marketing_ProjectedSaleQTY { get; set; }
        public decimal? Sales_ProjectedSaleQTY { get; set; }
        public decimal? DifferencePersentage { get; set; }
        public bool? IsAutoCalculate { get; set; }
        public decimal? NextMonth_ForecastingQTY { get; set; }
        public decimal? NextMonth_FinialForecastingQTY { get; set; }
        public decimal? NRVRate { get; set; }
        public decimal? ProjectionValue { get; set; }
    }
}
