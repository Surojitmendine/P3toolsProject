using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public class MasterSetupEntity
    {
        public class ProductMaster
        {
            
            public int PK_ProductID { get; set; }
            public string Category { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public int? PackUnit { get; set; }
            public int? FK_ProductCategoryID { get; set; }
            public string ProductType { get; set; }
            public string ProductCategory { get; set; }
            public string ProductUOM { get; set; }
            public decimal FactorValue { get; set; }
            public decimal BatchSize { get; set; }
            public decimal NRVRate { get; set; }
            public DateTime? NRVEffectiveRateFrom { get; set; }
            public string NRVEffectiveDate { get; set; }
            public string TallyProductName { get; set; }
            public string TallyUOM { get; set; }
        }

        public class Divisionwise_ProductEntity
        {
            public int PK_ID { get; set; }
            public string DivisionName { get; set; }
            public string DepotName { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public Boolean IsActive { get; set; }

            public string SyncDate { get; set; }
            public decimal ClosingStockQTY { get; set; }
            public int? ForYear { get; set; }
            public int? ForMonth { get; set; }
        }

        public class ProductMasterMapping
        {
            //                       
            public int PK_ProductID { get; set; }
            public string ProductCategory { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public int? PackUnit { get; set; }
            public string TallyProductName { get; set; }
            public string TallyUOM { get; set; }
        }
    }
}
