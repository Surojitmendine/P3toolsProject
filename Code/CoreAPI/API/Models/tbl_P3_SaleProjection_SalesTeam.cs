using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_SaleProjection_SalesTeam
    {
        public int PK_SalesTeamProjectionID { get; set; }
        public bool? IsManual { get; set; }
        public bool? IsProcessed { get; set; }
        public DateTime? ProjectionDate { get; set; }
        public int? ForMonth { get; set; }
        public int? ForYear { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string HQ { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public decimal? PrimaryTotalSalesQTY { get; set; }
        public decimal? TotalClosingStockQTY { get; set; }
        public decimal? ProjectedTotalSalesQTY { get; set; }
        public string ForecastingType { get; set; }
        public decimal? NRVRate { get; set; }
        public decimal? ProjectionValue { get; set; }
    }
}
