using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_SaleForecasting
    {
        public int PK_SaleForecastingID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? FK_CreatedByID { get; set; }
        public DateTime? CurrentDate { get; set; }
        public string Frequency { get; set; }
        public DateTime? ForecastingDate { get; set; }
        public int? ForecastingForMonth { get; set; }
        public int? ForecastingForYear { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string HQ { get; set; }
        public string Customer { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public decimal? Projected_SaleQTY { get; set; }
        public decimal? Current_SalesQTY { get; set; }
        public decimal? Next_ProjectionSalesQTY { get; set; }
        public string ForecastingType { get; set; }
        public decimal? NRVRate { get; set; }
        public decimal? ProjectionValue { get; set; }
    }
}
