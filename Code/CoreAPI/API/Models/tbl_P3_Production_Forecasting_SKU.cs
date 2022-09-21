using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Production_Forecasting_SKU
    {
        public int PK_ProductionForecastSKUID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? FK_CreatedByID { get; set; }
        public int? ForMonth { get; set; }
        public int? ForYear { get; set; }
        public string ProductType { get; set; }
        public string ProductCategory { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? PackUnit { get; set; }
        public decimal? ProductionForecastQTY { get; set; }
        public decimal? FactorValue { get; set; }
        public decimal? FactorForecastQTY { get; set; }
        public decimal? ProductionForecastVolume_InLtr { get; set; }
        public decimal? ChargeableVolume_InLtr { get; set; }
        public decimal? BatchSize { get; set; }
        public decimal? FinalChargeableVolume_InLtr { get; set; }
    }
}
