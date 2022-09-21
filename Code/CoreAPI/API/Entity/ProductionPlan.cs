using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public static class ProductionPlan
    {
        public class ImportExcel_FactoryProductionTarget
        {
            public int ID { get; set; }
            public Int32 ForYear { get; set; }
            public Int32 ForMonth { get; set; }
            public string MonthName { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public decimal FinalUnits_QTY { get; set; }
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

        public class ProductionPlan_VolumeConversion
        {
            public string ForMonth { get; set; }
            public string ProductType { get; set; }
            public string Category { get; set; }
            public string ProductName { get; set; }
            public int PackUnit { get; set; }
            public decimal ProjectionForecastQTY { get; set; }
            public decimal Factor { get; set; }
            public decimal FactorProjectionForecastQTY { get; set; }
            public decimal VolumeInLtrs { get; set; }
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
    }
}
