using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public static class SalesForecastEntity
    {
        public class Summary
        {
            public string Month { get; set; }
            public string ProductName { get; set; }
            public string ProductCode { get; set; }
            public string PackUnit { get; set; }
            public decimal Next_ProjectionSalesQTY { get; set; }
            public decimal NRVRate { get; set; }
            public decimal ProjectionValue { get; set; }
        }

        public class Forecasting
        {

            public string Month { get; set; }
            public string HQ { get; set; }
            public string DivisionName { get; set; }
            public string DepotName { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public decimal ProjectedQTY { get; set; }
            public string ForecastingType { get; set; }

        }

        public class ForecastingComparison
        {
            public int SaleComparisonID { get; set; }
            public string Month { get; set; }
            public string HQ { get; set; }
            public string DivisionName { get; set; }
            public string DepotName { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public decimal Logistics_ProjectionSalesQTY { get; set; }
            public decimal Marketing_ProjectedSaleQTY { get; set; }
            public decimal Sales_ProjectedSaleQTY { get; set; }
            
            public decimal DifferencePersentage { get; set; }
            public decimal NextMonth_ForecastingQTY { get; set; }
            public decimal NextMonth_FinialForecastingQTY { get; set; }
            public Boolean IsAutoCalculate { get; set; }
            public decimal NRVRate { get; set; }
            public decimal ProjectionValue { get; set; }

        }

        public class SaleProjection
        {
            public int ProjectedSalesID { get; set; }
            public string DivisionName { get; set; }
            public string DepotName { get; set; }
            public string HQ { get; set; }
          
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public decimal ProjectedTotalSalesQTY { get; set; }
            public decimal ProjectedAproxQTY { get; set; }
            public decimal ForecastingType { get; set; }
        }

        public class ImportProjection
        {
            public int ID { get; set; }
            public string ForecastingType { get; set; }

            public string Month { get; set; }
            public string HQ { get; set; }
            public string Depot { get; set; }
            public string Division { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            //public decimal ProjectedAproxQTY { get; set; }
            public decimal ProjectedTotalSalesQTY { get; set; }
        }

        public class PrimarySales
        {
            public int ID { get; set; }
            public string DivisionName { get; set; }
            public string SaleDate { get; set; }
            public string ForMonth { get; set; }
            public string Month { get; set; }
            public string DepotName { get; set; }
            public string HQ { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public string UOM { get; set; }
            public decimal SalesQTY { get; set; }
            public decimal FreeSampleQTY { get; set; }

        }

        public class Import_SecondarySales
        {
            public int ID { get; set; }
            public string DivisionName { get; set; }
            public int ForYear { get; set; }
            public int ForMonth { get; set; }
            public string Month { get; set; }
            public string DepotName { get; set; }
            public string HQ { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public string UOM { get; set; }
            public decimal SalesQTY { get; set; }
            public decimal FreeSampleQTY { get; set; }
            public decimal ClosingStockQTY { get; set; }

        }


        public class SaleProjection_SalesTeam
        {
            public int ID { get; set; }
            public string DivisionName { get; set; }
            public int ForYear { get; set; }
            public int ForMonth { get; set; }
            public string Month { get; set; }
            public string DepotName { get; set; }
            public string HQ { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string PackUnit { get; set; }
            public decimal PrimaryTotalSalesQTY { get; set; }
            public decimal TotalClosingStockQTY { get; set; }
            public decimal ProjectedTotalSalesQTY { get; set; }
            public Boolean IsManual { get; set; }
            public Boolean IsProcessed { get; set; }
            public DateTime? ProjectionDate { get; set; }
            public string ForecastingType { get; set; }

            public decimal ProjectionValue { get; set; }
            public decimal NRVRate { get; set; }
    
        }
    }
}
