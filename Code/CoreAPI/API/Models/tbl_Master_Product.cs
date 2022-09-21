using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_Master_Product
    {
        public int PK_ProductID { get; set; }
        public int? FK_ProductCategoryID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? PackUnit { get; set; }
        public string ProductType { get; set; }
        public string ProductCategory { get; set; }
        public string ProductUOM { get; set; }
        public decimal? FactorValue { get; set; }
        public decimal? BatchSize { get; set; }
        public decimal? NRVRate { get; set; }
        public DateTime? NRVEffectiveRateFrom { get; set; }
        public string TallyProductName { get; set; }
        public string TallyUOM { get; set; }

        public virtual tbl_Master_ProductCategory FK_ProductCategory { get; set; }
    }
}
