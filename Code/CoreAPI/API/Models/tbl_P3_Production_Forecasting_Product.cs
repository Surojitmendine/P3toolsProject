using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Production_Forecasting_Product
    {
        public int PK_ProductionForecastProductID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? FK_CreatedByID { get; set; }
        public int? ForMonth { get; set; }
        public int? ForYear { get; set; }
        public string ProductType { get; set; }
        public string ProductCategory { get; set; }
        public string ProductName { get; set; }
        public decimal? FactorValue { get; set; }
        public decimal? Volumn { get; set; }
        public decimal? WIPQTY { get; set; }
        public decimal? ChargeableVolume_InLtr { get; set; }
        public decimal? BatchSize { get; set; }
        public decimal? FinalChargeableVolume_InLtr { get; set; }
    }
}
