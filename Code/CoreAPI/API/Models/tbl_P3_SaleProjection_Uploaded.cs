using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_SaleProjection_Uploaded
    {
        public int PK_ProjectedSalesID { get; set; }
        public DateTime? ProjectionDate { get; set; }
        public string SalesPersonCode { get; set; }
        public string SalesPersonName { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string HQ { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public int? ProjectionForMonth { get; set; }
        public int? ProjectionForYear { get; set; }
        public decimal? ProjectedTotalSalesQTY { get; set; }
        public decimal? ProjectedAproxQTY { get; set; }
        public string ForecastingType { get; set; }
        public decimal? NRVRate { get; set; }
        public decimal? ProjectionValue { get; set; }
    }
}
