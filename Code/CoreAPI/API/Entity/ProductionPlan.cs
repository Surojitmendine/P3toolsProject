using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public static class ProductionPlan
    {
        public class ImportExcel_TallyProductBatch
        {
            public String CompanyId { get; set; }
            public string ProductName { get; set; }
            public string UOM { get; set; }
            public string BatchSize { get; set; }
            public string BOMName { get; set; }
        }

        public class ImportExcel_PhysicianSamplePlan
        {
            public int ID { get; set; }
            public Int32 ForYear { get; set; }
            public Int32 ForMonth { get; set; }
            public string MonthName { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public decimal PhysicianSampleQTY { get; set; }
        }

        public class ImportExcel_FactoryClosingStock
        {
            public string CompanyId { get; set; }
            //public long SLNO { get; set; }
            public string Stock_date { get; set; }
            public string St_group { get; set; }
            public string St_category { get; set; }

            public string product_name { get; set; }
            public Decimal quantity { get; set; }
            public string UOM { get; set; }
            public Decimal rate { get; set; }
            public Decimal amount { get; set; }
        }

        public class ImportExcel_DepotTransitStock
        {
            public string CompanyId { get; set; }
            public string Stock_date { get; set; }
            public string St_group { get; set; }
            public string St_category { get; set; }
            public string product_name { get; set; }
            public Decimal quantity { get; set; }
            public string UOM { get; set; }
            public Decimal rate { get; set; }
            public Decimal amount { get; set; }
        }

        public class ImportExcel_DepotClosingStock
        {
            public string CompanyId { get; set; }
            public string Stock_date { get; set; }
            public string St_group { get; set; }
            public string St_category { get; set; }
            public string product_name { get; set; }
            public Decimal quantity { get; set; }
            public string UOM { get; set; }
            public Decimal rate { get; set; }
            public Decimal amount { get; set; }
        }

        public class ImportExcel_PhysicianSampleForecast
        {
            public string CompanyId { get; set; }
            public int ForecastForYear { get; set; }
            public string ForecastForMonth { get; set; }
            public string Depot { get; set; }
            public string Division { get; set; }
            public string St_group { get; set; }
            public string St_category { get; set; }
            public string Product_Name { get; set; }
            public Decimal Quantity { get; set; }
            public int Pack { get; set; }
            public string UOM { get; set; }
            public Decimal Rate { get; set; }
            public Decimal Amount { get; set; }
        }

        public class ProductionPlan_VolumeConversion
        {
            public String ForecastingForMonth { get; set; }
            public int ForecastingForYear { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public int PackUnit { get; set; }
            public decimal NextMonth_FinalForecastingQTY { get; set; }
            public decimal NoOfPCS { get; set; }
            public decimal LTR { get; set; }
        }

        public class ProductionPlan_VolumeCharge
        {
            public string ForMonth { get; set; }
            public string ProductName { get; set; }
            public decimal VolumeInLtrs { get; set; }
            public decimal WIPInLtrs { get; set; }
            public decimal ChargeableVolumeInLtrs { get; set; }
            public decimal BatchSize { get; set; }
            public decimal FinalChargeInLtrs { get; set; }
        }

        public class ProductionPlan_FinalChargeUnit
        {
            public string ForMonth { get; set; }
            public string ProductType { get; set; }
            public string Category { get; set; }
            public string ProductName { get; set; }
            public int PackUnit { get; set; }
            public decimal FactorProjectionForecastQTY { get; set; }
            public decimal FinalChargeInUnit { get; set; }

        }


        public class Sync_ProductionPlan_Task
        {
            public string TaskName { get; set; }
            public bool? IsProcessed { get; set; }
        }

        public class ProductWise_BatchSize
        {
            public int BatchSize { get; set; }
        }
        public class ProductionPlan_ProductName
        {
            public String ProductName { get; set; }
        }

        public class ProductionPlan_ChargeableBatchList
        {
            public long SLNO { get; set; }
            public string CompanyId { get; set; }
            public int ForYear { get; set; }
            public string ForMonth { get; set; }
            public string ProductName { get; set; }
            public string ProductUOM { get; set; }
            public Decimal Final_Forecasted_Production_Volume_LT { get; set; }
            public Decimal Batchsize { get; set; }
            public int UnitFactor { get; set; }
            public int UserUnitFactor { get; set; }
            
        }
    }
}
