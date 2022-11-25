using API.Context;
using API.Entity;
using API.Helper;
using API.IdentityModels;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static API.Entity.SalesForecastEntity;

namespace API.BusinessLogic
{
    public class SalesForecastLogic
    {
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private DBContext db;
        private readonly IMapper _mapper;
        private readonly Functions functions;
        private CommonLogic common;
        //private PumpSetup pumpSetup;
        readonly UserManager<ApplicationUser> userManager;
        private readonly IMemoryCache memoryCache;

        public SalesForecastLogic()
        {
            this.functions = new Functions();
        }

        public SalesForecastLogic(DBContext db, IMapper mapper) : this()
        {

            this.db = db;
            this._mapper = mapper;
            this.common = new CommonLogic(db, mapper);

        }
        public SalesForecastLogic(DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager) : this(db, mapper)
        {
            this.userManager = userManager;
        }
        public SalesForecastLogic(DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager, IMemoryCache memoryCache) : this(db, mapper, userManager)
        {
            this.memoryCache = memoryCache;
        }


        public List<SalesForecastEntity.Summary> SalesForecastSummary(int ForecastingForMonth, int ForecastingForYear)
        {
            /*
             SELECT  ProductName, ProductCode, PackUnit, SUM(Next_ProjectionSalesQTY) AS ProjectedQTY
            FROM     dbo.tbl_P3_SaleForecasting
            WHERE  (ForecastingForMonth = 5) AND (ForecastingForYear = 2020)
            GROUP BY ProductName, ProductCode, PackUnit
            ORDER BY ProductName, PackUnit
             
             */

            var result = (from sf in db.tbl_P3_SaleForecastingComparison
                          join MasterMonth in db.tbl_Master_Month on sf.ForecastingForMonth equals MasterMonth.PK_MonthID into tmpMasterMonth
                          from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()

                          where sf.ForecastingForMonth == ForecastingForMonth && sf.ForecastingForYear == ForecastingForYear
                          && sf.DivisionName != "CONTRACTUAL" && sf.DepotName != "Factory"
                          && sf.NextMonth_FinialForecastingQTY>0
                          orderby sf.ProductName, sf.PackUnit
                          group new { sf } by new
                          {
                              lfttmpMasterMonth.MonthName,
                              sf.ForecastingForYear,
                              sf.ProductName,
                              //sf.ProductCode,
                              sf.PackUnit,
                              sf.NRVRate,
                              //sf.ProjectionValue,
                          } into groupsf

                          select new SalesForecastEntity.Summary
                          {
                              Month = groupsf.Key.MonthName + "-" + Convert.ToInt32(groupsf.Key.ForecastingForYear).ToString(),
                              ProductName = groupsf.Key.ProductName,
                              //ProductCode = groupsf.Key.ProductCode,
                              PackUnit = groupsf.Key.PackUnit,
                              Next_ProjectionSalesQTY = groupsf.Sum(s => (decimal)s.sf.NextMonth_FinialForecastingQTY),
                              NRVRate =  (decimal)groupsf.Key.NRVRate,
                              ProjectionValue = groupsf.Sum(s => (decimal)s.sf.ProjectionValue),
                          }).ToList();

            return result;

        }

        /// <summary>
        /// This Fucntion Use In Memory Caching
        /// </summary>
        /// <param name="ForecastingForMonth"></param>
        /// <param name="ForecastingForYear"></param>
        /// <param name="Product"></param>
        /// <param name="Division"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public async Task<List<SalesForecastEntity.Forecasting>> SalesForecasting(int ForecastingForMonth, int ForecastingForYear,  string Division, string Depot, string Product)
        {
            /*
            
                SELECT TOP (100) PERCENT HQ, DivisionName, DepotName, ProductCode, ProductName, PackUnit, SUM(Next_ProjectionSalesQTY) AS ProjectedQTY
                FROM     dbo.tbl_P3_SaleForecasting
                WHERE  (ForecastingForMonth = 5) AND (ForecastingForYear = 2020)
                GROUP BY ProductName, ProductCode, PackUnit, DivisionName, DepotName, HQ
                ORDER BY HQ, ProductName, PackUnit
             
             */
            //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            string[] arrProduct = string.IsNullOrEmpty(Product) == true ? new string[] { } : Product.Split(",");
            string[] arrDivision = string.IsNullOrEmpty(Division) == true ? new string[] { } : Division.Split(",");
            string[] arrDepot = string.IsNullOrEmpty(Depot) == true ? new string[] { } : Depot.Split(",");

            var result = (from sf in db.tbl_P3_SaleForecasting.AsQueryable()
                          join MasterMonth in db.tbl_Master_Month on sf.ForecastingForMonth equals MasterMonth.PK_MonthID into tmpMasterMonth
                          from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()

                          where sf.ForecastingForMonth == ForecastingForMonth && sf.ForecastingForYear == ForecastingForYear
                          // &&( string.IsNullOrEmpty(Product) == true ? sf.ProductName == sf.ProductName : arrProduct.Any(x => x.Contains(sf.ProductName)))
                          //&& (string.IsNullOrEmpty(Division) == true ? sf.DivisionName == sf.DivisionName :  arrDivision.Any(x => x.Contains(sf.DivisionName)))
                          //&& (string.IsNullOrEmpty(Depot) == true ? sf.DepotName == sf.DepotName : arrDepot.Any(x => x.Contains(sf.DepotName)))
                          orderby sf.HQ, sf.DepotName, sf.DivisionName, sf.ProductName, sf.PackUnit
                          group new { sf } by new
                          {
                              sf.ProductName,
                              sf.ProductCode,
                              sf.PackUnit,
                              sf.DivisionName,
                              sf.DepotName,
                              sf.HQ,
                              sf.ForecastingType,
                              sf.ForecastingForYear,
                              lfttmpMasterMonth.MonthName
                          } into groupsf

                          select new SalesForecastEntity.Forecasting
                          {

                              HQ = groupsf.Key.HQ,
                              Month = groupsf.Key.MonthName + "-" + Convert.ToInt32(groupsf.Key.ForecastingForYear).ToString(),
                              DepotName = groupsf.Key.DepotName,
                              DivisionName = groupsf.Key.DivisionName,
                              ProductCode = groupsf.Key.ProductCode,
                              ProductName = groupsf.Key.ProductName,
                              PackUnit = groupsf.Key.PackUnit,
                              ProjectedQTY = groupsf.Sum(s => (decimal)s.sf.Projected_SaleQTY),
                              ForecastingType = groupsf.Key.ForecastingType,
                          });


            var cacherecords = await this.memoryCache.GetOrCreateAsync($"SalesForecasting{ForecastingForYear}{ForecastingForMonth}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return Task.FromResult(result.ToList());
            });

            List<SalesForecastEntity.Forecasting> forcasting = new List<SalesForecastEntity.Forecasting>();

            if (cacherecords != null)
            {
                forcasting = cacherecords.Where(x =>
                        (string.IsNullOrEmpty(Product) == true ? x.ProductName == x.ProductName : arrProduct.Any(ax => ax.Contains(x.ProductName)))
                      && (string.IsNullOrEmpty(Division) == true ? x.DivisionName == x.DivisionName : arrDivision.Any(ax => ax.Contains(x.DivisionName)))
                      && (string.IsNullOrEmpty(Depot) == true ? x.DepotName == x.DepotName : arrDepot.Any(ax => ax.Contains(x.DepotName)))
                    ).ToList();
            }
            return forcasting;
        }

        public async Task<dynamic[]> ForcastingDetailsSearchFields()
        {

            dynamic[] dynamics = new dynamic[3];

            var divisions = db.tbl_P3_SaleForecasting.GroupBy(x => new { x.DivisionName })
               .Select(s => new
               {
                   id = s.Key.DivisionName,
                   text = s.Key.DivisionName,

               }).OrderBy(o => o.text).ToList();

            var depotnames = db.tbl_P3_SaleForecasting.GroupBy(x => new { x.DepotName })
               .Select(s => new
               {
                   id = s.Key.DepotName,
                   text = s.Key.DepotName,

               }).OrderBy(o => o.text).ToList();

            var products = db.tbl_P3_SaleForecasting.AsEnumerable().GroupBy(x => new { x.ProductName, x.PackUnit })
            .Select(s => new
            {
                id = s.Key.ProductName,
                text = $"{s.Key.ProductName }({s.Key.PackUnit})",
                packunit = s.Key.PackUnit

            }).OrderBy(o => o.text).ToList();

            dynamics[0] = divisions;
            dynamics[1] = depotnames;
            dynamics[2] = products;
            return await Task.FromResult(dynamics);
        }

        #region Forcasting Comparison

        #region Forcasting Comparison SearchFields
        public async Task<dynamic[]> ForcastingComparison_SearchFields()
        {

            dynamic[] dynamics = new dynamic[4];

            var products = db.tbl_P3_SaleForecastingComparison.AsEnumerable()
                .GroupBy(x => new { x.ProductName})
                .Select(s => new
                {
                    id = s.Key.ProductName,
                    //text = $"{s.Key.ProductName }({s.Key.PackUnit})",
                    text = s.Key.ProductName,
                    //packunit = s.Key.PackUnit

                }).OrderBy(o => o.text).ToList();

            var divisions = db.tbl_P3_SaleForecastingComparison
               .Where(u => u.DivisionName != "CONTRACTUAL")
               .GroupBy(x => new { x.DivisionName })
               .Select(s => new
               {
                   id = s.Key.DivisionName,
                   text = s.Key.DivisionName,

               }).OrderBy(o => o.text).ToList();

            var stocklocations = db.tbl_P3_SaleForecastingComparison
               .Where(u => u.DepotName != "FACTORY")
               .GroupBy(x => new { x.DepotName })
               .Select(s => new
               {
                   id = s.Key.DepotName,
                   text = s.Key.DepotName,

               }).OrderBy(o => o.text).ToList();

            var packunit = db.tbl_Master_Product.AsEnumerable().GroupBy(x => new { x.PackUnit, x.ProductUOM })
            .Select(s => new
            {
                id = s.Key.PackUnit,
                text = $"{s.Key.PackUnit }({s.Key.ProductUOM})",
                productUOM = s.Key.ProductUOM

            }).OrderBy(o => o.text).ToList();

            dynamics[0] = products;
            dynamics[1] = divisions;
            dynamics[2] = stocklocations;
            dynamics[3] = packunit;

            return await Task.FromResult(dynamics);
        }
        #endregion

        #region SalesForecastingComparison
        /// <summary>
        /// This Fucntion Use In Memory Caching
        /// </summary>
        /// <param name="ForecastingForMonth"></param>
        /// <param name="ForecastingForYear"></param>
        /// <param name="Product"></param>
        /// <param name="Division"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public Tuple<List<SalesForecastEntity.ForecastingComparison>, string> SalesForecastingComparison(int ForecastingForMonth, int ForecastingForYear, string Product, string Division, string Location, string PackUnit,
            bool ReInitializeCache)
        {
            /*
            
                SELECT TOP (100) PERCENT HQ, DivisionName, DepotName, ProductCode, ProductName, PackUnit, SUM(Next_ProjectionSalesQTY) AS ProjectedQTY
                FROM     dbo.tbl_P3_SaleForecasting
                WHERE  (ForecastingForMonth = 5) AND (ForecastingForYear = 2020)
                GROUP BY ProductName, ProductCode, PackUnit, DivisionName, DepotName, HQ
                ORDER BY HQ, ProductName, PackUnit
             
             */
            //  System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            string[] arrProduct = string.IsNullOrEmpty(Product) == true ? new string[] { } : Product.Split(",");
            string[] arrDivision = string.IsNullOrEmpty(Division) == true ? new string[] { } : Division.Split(",");
            string[] arrLocation = string.IsNullOrEmpty(Location) == true ? new string[] { } : Location.Split(",");
            string[] arrPackUnit = string.IsNullOrEmpty(PackUnit) == true ? new string[] { } : PackUnit.Split(",");

            var result = (from sf in db.tbl_P3_SaleForecastingComparison.AsEnumerable()
                          join MasterMonth in db.tbl_Master_Month on sf.ForecastingForMonth equals MasterMonth.PK_MonthID into tmpMasterMonth
                          from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()

                          where sf.ForecastingForMonth == ForecastingForMonth && sf.ForecastingForYear == ForecastingForYear
                           && (string.IsNullOrEmpty(Division) == true ? sf.DivisionName == sf.DivisionName : arrDivision.Any(ax => ax.Contains(sf.DivisionName)))
                           && (string.IsNullOrEmpty(Location) == true ? sf.DepotName == sf.DepotName : arrLocation.Any(ax => ax.Contains(sf.DepotName)))
                           && (string.IsNullOrEmpty(Product) == true ? sf.ProductName == sf.ProductName : arrProduct.Any(ax => ax.Equals(sf.ProductName)))
                           && (string.IsNullOrEmpty(PackUnit) == true ? sf.PackUnit == sf.PackUnit : arrPackUnit.Any(ax => ax.Contains(sf.PackUnit)))

                          orderby sf.HQ, sf.DepotName, sf.DivisionName, sf.ProductName, sf.PackUnit
                          //group new { sf } by new
                          //{
                          //    sf.ProductName,
                          //    sf.ProductCode,
                          //    sf.PackUnit,
                          //    sf.DivisionName,
                          //    sf.DepotName,
                          //    sf.HQ,
                          //    sf.IsAutoCalculate,
                          //} into groupsf

                          select new SalesForecastEntity.ForecastingComparison
                          {
                              SaleComparisonID = sf.PK_SaleComparisonID,
                              Month = lfttmpMasterMonth.MonthName + "-" + Convert.ToInt32(sf.ForecastingForYear).ToString(),
                              HQ = sf.HQ,
                              DepotName = sf.DepotName,
                              DivisionName = sf.DivisionName,
                              ProductCode = sf.ProductCode,
                              ProductName = sf.ProductName,
                              PackUnit = sf.PackUnit,
                              Logistics_ProjectionSalesQTY = (decimal)sf.Logistics_ProjectionSalesQTY,
                              Marketing_ProjectedSaleQTY = (decimal)sf.Marketing_ProjectedSaleQTY,
                              Sales_ProjectedSaleQTY = (decimal)sf.Sales_ProjectedSaleQTY,
                              DifferencePersentage = (decimal)sf.DifferencePersentage,
                              NextMonth_ForecastingQTY = (decimal)sf.NextMonth_ForecastingQTY,
                              NextMonth_FinialForecastingQTY = (decimal)sf.NextMonth_FinialForecastingQTY,
                              IsAutoCalculate = (bool)sf.IsAutoCalculate,
                              NRVRate = (decimal)sf.NRVRate,
                              ProjectionValue = (decimal)sf.ProjectionValue,
                          }).ToList();

            string message = string.Empty;
            //if (ReInitializeCache == true)
            //{
            //    message = "Cache Removed";
            //    this.memoryCache.Remove($"_SalesForecastingComparison{ForecastingForYear}{ForecastingForMonth}");
            //}

            //var cacherecords = await this.memoryCache.GetOrCreateAsync($"_SalesForecastingComparison{ForecastingForYear}{ForecastingForMonth}", entry =>
            //{
            //    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            //    message = message + " Create Cache";
            //    return Task.FromResult(result.ToList());
            //});

            //List<SalesForecastEntity.ForecastingComparison> forecastingComparisons = new List<SalesForecastEntity.ForecastingComparison>();

            //if (cacherecords != null)
            //{
            //    forecastingComparisons = cacherecords.Where(x =>
            //            (string.IsNullOrEmpty(Product) == true ? x.ProductName == x.ProductName : arrProduct.Any(ax => ax.Contains(x.ProductName)))
            //          && (string.IsNullOrEmpty(Division) == true ? x.DivisionName == x.DivisionName : arrDivision.Any(ax => ax.Contains(x.DivisionName)))
            //          && (string.IsNullOrEmpty(Location) == true ? x.DepotName == x.DepotName : arrLocation.Any(ax => ax.Contains(x.DepotName)))
            //        ).ToList();
            //}
            //return new Tuple<List<SalesForecastEntity.ForecastingComparison>, string>(forecastingComparisons, message);


            return new Tuple<List<SalesForecastEntity.ForecastingComparison>, string>(result, message);

        }
        #endregion

        #region Sales Forecastin Comparison Summary
        /// <summary>
        /// This Fucntion Use In Memory Caching
        /// </summary>
        /// <param name="ForecastingForMonth"></param>
        /// <param name="ForecastingForYear"></param>
        /// <param name="Product"></param>
        /// <param name="Division"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public async Task<Tuple<List<SalesForecastEntity.ForecastingComparison>, string>> SalesForecastingComparison_Summary(int ForecastingForMonth, int ForecastingForYear, string Product, string Division, string Location, string PackUint,
            bool ReInitializeCache)
        {
            /*            
                SELECT TOP (100) PERCENT HQ, DivisionName, DepotName, ProductCode, ProductName, PackUnit, SUM(Next_ProjectionSalesQTY) AS ProjectedQTY
                FROM     dbo.tbl_P3_SaleForecasting
                WHERE  (ForecastingForMonth = 5) AND (ForecastingForYear = 2020)
                GROUP BY ProductName, ProductCode, PackUnit, DivisionName, DepotName, HQ
                ORDER BY HQ, ProductName, PackUnit             
             */
            //  System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            string[] arrProduct = string.IsNullOrEmpty(Product) == true ? new string[] { } : Product.Split(",");
            string[] arrDivision = string.IsNullOrEmpty(Division) == true ? new string[] { } : Division.Split(",");
            string[] arrLocation = string.IsNullOrEmpty(Location) == true ? new string[] { } : Location.Split(",");
            string[] arrPackUint = string.IsNullOrEmpty(PackUint) == true ? new string[] { } : PackUint.Split(",");

            var result = (from sf in db.tbl_P3_SaleForecastingComparison.AsQueryable()
                          where sf.ForecastingForMonth == ForecastingForMonth && sf.ForecastingForYear == ForecastingForYear
                          orderby sf.DepotName, sf.DivisionName, sf.ProductName, sf.PackUnit
                          group new { sf } by new
                          {
                              sf.DepotName,
                              sf.DivisionName,
                              sf.ProductName,
                              sf.ProductCode,
                              sf.PackUnit,
                          } into groupsf
                          select new SalesForecastEntity.ForecastingComparison
                          {
                              DepotName = groupsf.Key.DepotName,
                              DivisionName = groupsf.Key.DivisionName,
                              ProductCode = groupsf.Key.ProductCode,
                              ProductName = groupsf.Key.ProductName,
                              PackUnit = groupsf.Key.PackUnit,
                              Logistics_ProjectionSalesQTY = groupsf.Sum(s => (decimal)s.sf.Logistics_ProjectionSalesQTY),
                              Marketing_ProjectedSaleQTY = groupsf.Sum(s => (decimal)s.sf.Marketing_ProjectedSaleQTY),
                              DifferencePersentage = groupsf.Sum(s => (decimal)s.sf.DifferencePersentage),
                              NextMonth_ForecastingQTY = groupsf.Sum(s => (decimal)s.sf.NextMonth_ForecastingQTY),
                              NextMonth_FinialForecastingQTY = groupsf.Sum(s => (decimal)s.sf.NextMonth_FinialForecastingQTY),
                          });//.ToList();

            string message = string.Empty;
            if (ReInitializeCache == true)
            {
                message = "Cache Removed";
                this.memoryCache.Remove($"_SalesForecastingComparison{ForecastingForYear}{ForecastingForMonth}");
            }

            var cacherecords = await this.memoryCache.GetOrCreateAsync($"_SalesForecastingComparison{ForecastingForYear}{ForecastingForMonth}", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                message = message + " Create Cache";
                return Task.FromResult(result.ToList());
            });

            List<SalesForecastEntity.ForecastingComparison> forecastingComparisons = new List<SalesForecastEntity.ForecastingComparison>();

            if (cacherecords != null)
            {
                forecastingComparisons = cacherecords.Where(x =>
                        (string.IsNullOrEmpty(Product) == true ? x.ProductName == x.ProductName : arrProduct.Any(ax => ax.Equals(x.ProductName)))
                      && (string.IsNullOrEmpty(Division) == true ? x.DivisionName == x.DivisionName : arrDivision.Any(ax => ax.Contains(x.DivisionName)))
                      && (string.IsNullOrEmpty(Location) == true ? x.DepotName == x.DepotName : arrLocation.Any(ax => ax.Contains(x.DepotName)))
                      && (string.IsNullOrEmpty(PackUint) == true ? x.PackUnit == x.PackUnit : arrPackUint.Any(ax => ax.Contains(x.PackUnit)))
                    ).ToList();
            }
            return new Tuple<List<SalesForecastEntity.ForecastingComparison>, string>(forecastingComparisons, message);
        }



        #endregion

        public async Task<bool> SalesForecastingComparisonSave(SalesForecastEntity.ForecastingComparison forecastingComparison)
        {

            var tmp = new tbl_P3_SaleForecastingComparison()
            {
                PK_SaleComparisonID = forecastingComparison.SaleComparisonID,
                NextMonth_FinialForecastingQTY = forecastingComparison.NextMonth_FinialForecastingQTY,
                ProjectionValue = forecastingComparison.NextMonth_FinialForecastingQTY * forecastingComparison.NRVRate,
                //NextMonth_FinialForecastingQTY = forecastingComparison.NextMonth_FinialForecastingQTY
            };

            db.tbl_P3_SaleForecastingComparison.Attach(tmp);
            db.Entry(tmp).Property(p => p.NextMonth_FinialForecastingQTY).IsModified = true;
            db.Entry(tmp).Property(p => p.ProjectionValue).IsModified = true;
            var result = await this.db.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        #endregion

        #region Third Party Forcasting Comparison

        #region Third Party Forcasting Comparison SearchFields
        public async Task<dynamic[]> ThirdPartyForcastingComparison_SearchFields()
        {

            dynamic[] dynamics = new dynamic[4];

            var products = db.tbl_P3_SaleForecastingComparison.AsEnumerable()
                .Where(u => u.DivisionName == "CONTRACTUAL")
                .GroupBy(x => new { x.ProductName })
                .Select(s => new
                {
                    id = s.Key.ProductName,
                    text = s.Key.ProductName

                }).OrderBy(o => o.text).ToList();

            var divisions = db.tbl_P3_SaleForecastingComparison
               .Where(u => u.DivisionName == "CONTRACTUAL")
               .GroupBy(x => new { x.DivisionName })
               .Select(s => new
               {
                   id = s.Key.DivisionName,
                   text = s.Key.DivisionName,

               }).OrderBy(o => o.text).ToList();

            var stocklocations = db.tbl_P3_SaleForecastingComparison
               .Where(u => u.DepotName == "FACTORY")
               .GroupBy(x => new { x.DepotName })
               .Select(s => new
               {
                   id = s.Key.DepotName,
                   text = s.Key.DepotName,

               }).OrderBy(o => o.text).ToList();

            var packunit = db.tbl_Master_Product.AsEnumerable().GroupBy(x => new { x.PackUnit, x.ProductUOM })
            .Select(s => new
            {
                id = s.Key.PackUnit,
                text = $"{s.Key.PackUnit }({s.Key.ProductUOM})",
                productUOM = s.Key.ProductUOM

            }).OrderBy(o => o.text).ToList();

            dynamics[0] = products;
            dynamics[1] = divisions;
            dynamics[2] = stocklocations;
            dynamics[3] = packunit;

            return await Task.FromResult(dynamics);
        }
        #endregion

        #region ThirdPartyForecastingComparison
       
        public Tuple<List<SalesForecastEntity.ForecastingComparison>, string> ThirdPartyForecastingComparison(int ForecastingForMonth, int ForecastingForYear, string Product, string Division, string Location, string PackUnit,
            bool ReInitializeCache)
        {
            string[] arrProduct = string.IsNullOrEmpty(Product) == true ? new string[] { } : Product.Split(",");
            string[] arrDivision = string.IsNullOrEmpty(Division) == true ? new string[] { } : Division.Split(",");
            string[] arrLocation = string.IsNullOrEmpty(Location) == true ? new string[] { } : Location.Split(",");
            string[] arrPackUnit = string.IsNullOrEmpty(PackUnit) == true ? new string[] { } : PackUnit.Split(",");

            var result = (from sf in db.tbl_P3_SaleForecastingComparison.AsEnumerable()
                          join MasterMonth in db.tbl_Master_Month on sf.ForecastingForMonth equals MasterMonth.PK_MonthID into tmpMasterMonth
                          from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()

                          where sf.ForecastingForMonth == ForecastingForMonth && sf.ForecastingForYear == ForecastingForYear
                          && sf.DivisionName == "CONTRACTUAL" && sf.DepotName == "FACTORY"
                           && (string.IsNullOrEmpty(Division) == true ? sf.DivisionName == sf.DivisionName : arrDivision.Any(ax => ax.Contains(sf.DivisionName)))
                           && (string.IsNullOrEmpty(Location) == true ? sf.DepotName == sf.DepotName : arrLocation.Any(ax => ax.Contains(sf.DepotName)))
                           && (string.IsNullOrEmpty(Product) == true ? sf.ProductName == sf.ProductName : arrProduct.Any(ax => ax.Equals(sf.ProductName)))
                           && (string.IsNullOrEmpty(PackUnit) == true ? sf.PackUnit == sf.PackUnit : arrPackUnit.Any(ax => ax.Contains(sf.PackUnit)))

                          orderby sf.HQ, sf.DepotName, sf.DivisionName, sf.ProductName, sf.PackUnit
                          

                          select new SalesForecastEntity.ForecastingComparison
                          {
                              SaleComparisonID = sf.PK_SaleComparisonID,
                              Month = lfttmpMasterMonth.MonthName + "-" + Convert.ToInt32(sf.ForecastingForYear).ToString(),
                              HQ = sf.HQ,
                              DepotName = sf.DepotName,
                              DivisionName = sf.DivisionName,
                              ProductCode = sf.ProductCode,
                              ProductName = sf.ProductName,
                              PackUnit = sf.PackUnit,
                              Logistics_ProjectionSalesQTY = (decimal)sf.Logistics_ProjectionSalesQTY,
                              Marketing_ProjectedSaleQTY = (decimal)sf.Marketing_ProjectedSaleQTY,
                              Sales_ProjectedSaleQTY = (decimal)sf.Sales_ProjectedSaleQTY,
                              DifferencePersentage = (decimal)sf.DifferencePersentage,
                              NextMonth_ForecastingQTY = (decimal)sf.NextMonth_ForecastingQTY,
                              NextMonth_FinialForecastingQTY = (decimal)sf.NextMonth_FinialForecastingQTY,
                              IsAutoCalculate = (bool)sf.IsAutoCalculate,
                              NRVRate = (decimal)sf.NRVRate,
                              ProjectionValue = (decimal)sf.ProjectionValue,
                          }).ToList();

            string message = string.Empty;
            
            return new Tuple<List<SalesForecastEntity.ForecastingComparison>, string>(result, message);

        }
        #endregion

        #region

        public async Task<bool> ThirdPartyForecastingComparisonSave(SalesForecastEntity.ForecastingComparison forecastingComparison)
        {

            var tmp = new tbl_P3_SaleForecastingComparison()
            {
                PK_SaleComparisonID = forecastingComparison.SaleComparisonID,
                NextMonth_FinialForecastingQTY = forecastingComparison.NextMonth_FinialForecastingQTY,
                ProjectionValue = forecastingComparison.NextMonth_FinialForecastingQTY * forecastingComparison.NRVRate,
                //NextMonth_FinialForecastingQTY = forecastingComparison.NextMonth_FinialForecastingQTY
            };

            db.tbl_P3_SaleForecastingComparison.Attach(tmp);
            db.Entry(tmp).Property(p => p.NextMonth_FinialForecastingQTY).IsModified = true;
            db.Entry(tmp).Property(p => p.ProjectionValue).IsModified = true;
            var result = await this.db.SaveChangesAsync();

            return result > 0 ? true : false;
        }


        #endregion

        #endregion



        public List<SalesForecastEntity.ImportProjection> ListImportProjection(int ImportProjectionForMonth, int ImportProjectionForYear, string ForecastingType)
        {
            if (ForecastingType== "SalesTeam")
            {
                var result = (from sp in db.tbl_P3_SaleProjection_SalesTeam
                              join MasterMonth in db.tbl_Master_Month on sp.ForMonth equals MasterMonth.PK_MonthID into tmpMasterMonth
                              from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()

                              where sp.ForMonth == ImportProjectionForMonth && sp.ForYear == ImportProjectionForYear 
                              && sp.DivisionName != "CONTRACTUAL" && sp.DepotName != "Factory"
                              //&& sp.ForecastingType == ForecastingType
                              orderby sp.DivisionName, sp.DepotName, sp.ProductName, sp.PackUnit
                              select new SalesForecastEntity.ImportProjection
                              {
                                  ID = sp.PK_SalesTeamProjectionID,
                                  ForecastingType = sp.ForecastingType,
                                  Month = lfttmpMasterMonth.MonthName + "-" + Convert.ToInt32(sp.ForYear).ToString(),
                                  HQ = sp.HQ,
                                  Depot = sp.DepotName,
                                  Division = sp.DivisionName,
                                  ProductCode = sp.ProductCode,
                                  ProductName = sp.ProductName,
                                  PackUnit = sp.PackUnit,
                                  ProjectedTotalSalesQTY = (decimal)sp.ProjectedTotalSalesQTY
                              }).ToList();
                return result;
            }
            else
            {
                var result = (from sp in db.tbl_P3_SaleProjection_Uploaded
                              join MasterMonth in db.tbl_Master_Month on sp.ProjectionForMonth equals MasterMonth.PK_MonthID into tmpMasterMonth
                              from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()

                              where sp.ProjectionForMonth == ImportProjectionForMonth && sp.ProjectionForYear == ImportProjectionForYear
                              && sp.ForecastingType == ForecastingType
                              orderby sp.HQ, sp.DepotName, sp.DivisionName, sp.ProductName, sp.PackUnit
                              select new SalesForecastEntity.ImportProjection
                              {
                                  ID = sp.PK_ProjectedSalesID,
                                  ForecastingType = sp.ForecastingType,
                                  Month = lfttmpMasterMonth.MonthName + "-" + Convert.ToInt32(sp.ProjectionForYear).ToString(),
                                  HQ = sp.HQ,
                                  Depot = sp.DepotName,
                                  Division = sp.DivisionName,
                                  ProductCode = sp.ProductCode,
                                  ProductName = sp.ProductName,
                                  PackUnit = sp.PackUnit,
                                  ProjectedTotalSalesQTY = (decimal)sp.ProjectedTotalSalesQTY
                              }).ToList();
                return result;
            }             
        }

        #region -- Add New Sales Team Projection  --
        public async Task<bool> AddNew_Projection(ImportProjection projection)
        {
            bool brecordcreated = false;
            ImportProjection sanitized = this.functions.SetFKValueNullIfZero(projection);
            var tmpprojection = this._mapper.Map<tbl_P3_SaleProjection_SalesTeam>(sanitized);
            this.db.tbl_P3_SaleProjection_SalesTeam.Add(tmpprojection);
            var result = await db.SaveChangesAsync();
            if (result == 1)
            {
                brecordcreated = true;
            }
            return brecordcreated;
        }
        #endregion

        public async Task<bool> UpdateProjection(List<SalesForecastEntity.ImportProjection> projections)
        {
            // NEED TO UPDATE SINGLE FIELD ProjectedTotalSalesQTY BASED ON ID

            var newprojections = projections.Where(projection => string.IsNullOrEmpty(projection.ID.ToString()) == true || projection.ID <= 0).ToList();

            var updatedprojections = projections.Where(projection => projection.ID > 0).ToList();

            var listnewprojections = this._mapper.Map<List<tbl_P3_SaleProjection_Uploaded>>(newprojections);

            var listupdatedprojections = this._mapper.Map<List<tbl_P3_SaleProjection_Uploaded>>(updatedprojections);

            this.db.tbl_P3_SaleProjection_Uploaded.AddRange(listnewprojections);

            foreach (var projection in listupdatedprojections)
            {

                var tmpprojection = new tbl_P3_SaleProjection_Uploaded()
                {
                    PK_ProjectedSalesID = projection.PK_ProjectedSalesID,
                    ProjectedTotalSalesQTY = projection.ProjectedTotalSalesQTY
                };

                db.tbl_P3_SaleProjection_Uploaded.Attach(tmpprojection);
                db.Entry(tmpprojection).Property(x => x.ProjectedTotalSalesQTY).IsModified = true;

            }

            var result = await this.db.SaveChangesAsync();

            return true;

        }

     

        #region Secondary Sales

        #region List Secondary Sales
        public List<SalesForecastEntity.Import_SecondarySales> List_SecondarySales(int ForYear, int ForMonth)
        {
            var result = (from sp in db.tbl_P3_SecondarySaleTransDetails_Uploaded

                          join MasterMonth in db.tbl_Master_Month on sp.ForMonth equals MasterMonth.PK_MonthID into tmpMasterMonth
                          from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()

                          where sp.ForYear == ForYear && sp.ForMonth == ForMonth
                          orderby sp.HQ, sp.DepotName, sp.DivisionName, sp.ProductName, sp.PackUnit
                          select new SalesForecastEntity.Import_SecondarySales
                          {
                              ID = sp.PK_SalesTransactionID,
                              ForMonth =Convert.ToInt32(sp.ForMonth),
                              ForYear = Convert.ToInt32(sp.ForYear),
                              Month= lfttmpMasterMonth.MonthName +"-"+ Convert.ToInt32(sp.ForYear).ToString(),
                              DivisionName = sp.DivisionName,
                              DepotName = sp.DepotName,
                              HQ = sp.HQ,
                              CustomerCode = sp.CustomerCode,
                              CustomerName = sp.CustomerName,
                              ProductCode = sp.ProductCode,
                              ProductName = sp.ProductName,
                              PackUnit = sp.PackUnit,
                              UOM = sp.UOM,
                              SalesQTY = (decimal)sp.SalesQTY,
                              FreeSampleQTY = (decimal)sp.FreeSampleQTY,
                              ClosingStockQTY = (decimal)sp.ClosingStockQTY
                          }).ToList();

            return result;
        }
        #endregion

        #region  Update Secondary Sales
        public async Task<bool> SaveExcel_SecondarySales(int Year, int Month, List<SalesForecastEntity.Import_SecondarySales> secondarySalesData)
        {
            var existingrecords = db.tbl_P3_SecondarySaleTransDetails_Uploaded.Where(x => x.ForYear == Year && x.ForMonth == Month).Select(s => s).ToList();
            if (existingrecords.Count > 0)
            {
                db.RemoveRange(existingrecords);
                // var deleted = await db.SaveChangesAsync();
            }
            // ADD NEW RECORDS ....
            bool bresult = false;
            var new_secondarySalesData = secondarySalesData.Where(Ps => string.IsNullOrEmpty(Ps.ID.ToString()) == true || Ps.ID <= 0).ToList();
            //var updated_secondarySalesData = secondarySalesData.Where(s => s.ID > 0).ToList();
            var listnew_secondarySalesData = this._mapper.Map<List<tbl_P3_SecondarySaleTransDetails_Uploaded>>(new_secondarySalesData);
            //var listupdated_secondarySalesData = this._mapper.Map<List<tbl_P3_SecondarySaleTransDetails_Uploaded>>(updated_secondarySalesData);

            listnew_secondarySalesData.ForEach(x =>
            {
                x.ForMonth = Month;
                x.ForYear = Year;
                x.FK_ClientID = 1;
                x.IsProcessed = false;
                x.FreeSampleQTY = 0;
            });

            this.db.tbl_P3_SecondarySaleTransDetails_Uploaded.AddRange(listnew_secondarySalesData);
            var result = await this.db.SaveChangesAsync();

            if (result > 0)
            {
                bresult = true;
            }
            return bresult;
        }
        #endregion

        #endregion

        #region List Primary  Sales
        public List<SalesForecastEntity.PrimarySales> List_PrimarySales(string FromDate, string ToDate)
        {
            DateTime fromdate = this.functions.ConvertStringToDate(FromDate);
            DateTime todate = this.functions.ConvertStringToDate(ToDate);

            var result = (from sp in db.tbl_P3_PrimarySaleTransDetails_Uploaded
                          where (sp.SaleDate.Value.Date >= fromdate.Date  && sp.SaleDate.Value.Date <=todate.Date )
                          orderby sp.HQ, sp.DepotName, sp.DivisionName, sp.ProductName, sp.PackUnit
                          select new SalesForecastEntity.PrimarySales
                          {
                              ID = sp.PK_SalesTransactionID,
                              SaleDate = sp.SaleDate.Value.Date.ToString("dd/MM/yyyy"),
                              Month = (sp.SaleDate.Value.Year.ToString())+"-"+ (sp.SaleDate.Value.Month.ToString()),
                              //Month = lfttmpMasterMonth.MonthName + "-" + Convert.ToInt32(sp.ForYear).ToString(),
                              DivisionName = sp.DivisionName,
                              DepotName = sp.DepotName,
                              HQ = sp.HQ,
                              CustomerCode = sp.CustomerCode,
                              CustomerName = sp.CustomerName,
                              ProductCode = sp.ProductCode,
                              ProductName = sp.ProductName,
                              PackUnit = sp.PackUnit,
                              SalesQTY = (decimal)sp.SalesQTY,
                              FreeSampleQTY = (decimal)sp.FreeSampleQTY,
                          }).ToList();
            return result;
        }
        #endregion

        //#region Secondary ClosingStock

        //#region List Secondary ClosingStock
        //public List<SalesForecastEntity.Import_SecondaryClosingStock> List_SecondaryClosingStock(int ForYear, int ForMonth)
        //{
        //    var result = (from sp in db.tbl_P3_SecondarySaleTransDetails_ClosingStock

        //                  join MasterMonth in db.tbl_Master_Month on sp.ForMonth equals MasterMonth.PK_MonthID into tmpMasterMonth
        //                  from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()

        //                  where sp.ForYear == ForYear && sp.ForMonth == ForMonth
        //                  orderby sp.HQ, sp.DepotName, sp.DivisionName, sp.ProductName, sp.PackUnit
        //                  select new SalesForecastEntity.Import_SecondaryClosingStock
        //                  {
        //                      ID = sp.PK_SSClosingStockID,
        //                      ForMonth = Convert.ToInt32(sp.ForMonth),
        //                      ForYear = Convert.ToInt32(sp.ForYear),
        //                      Month = lfttmpMasterMonth.MonthName + "-" + Convert.ToInt32(sp.ForYear).ToString(),
        //                      DivisionName = sp.DivisionName,
        //                      DepotName = sp.DepotName,
        //                      HQ = sp.HQ,
        //                      CustomerCode = sp.CustomerCode,
        //                      CustomerName = sp.CustomerName,
        //                      ProductCode = sp.ProductCode,
        //                      ProductName = sp.ProductName,
        //                      PackUnit = sp.PackUnit,
        //                      UOM = sp.UOM,
        //                      ClosingStockQTY = (decimal)sp.ClosingStockQTY,

        //                  }).ToList();

        //    return result;
        //}
        //#endregion

        //#region  Update Secondary Sales
        //public async Task<bool> SaveExcel_SecondaryClosingStock(int Year, int Month, List<SalesForecastEntity.Import_SecondaryClosingStock> secondaryclosingStock)
        //{
        //    var existingrecords = db.tbl_P3_SecondarySaleTransDetails_ClosingStock.Where(x => x.ForYear == Year && x.ForMonth == Month).Select(s => s).ToList();
        //    if (existingrecords.Count > 0)
        //    {
        //        db.RemoveRange(existingrecords);
        //        // var deleted = await db.SaveChangesAsync();
        //    }
        //    // ADD NEW RECORDS ....
        //    bool bresult = false;
        //    var new_secondaryclosingStock = secondaryclosingStock.Where(Ps => string.IsNullOrEmpty(Ps.ID.ToString()) == true || Ps.ID <= 0).ToList();
        //    //var updated_secondarySalesData = secondarySalesData.Where(s => s.ID > 0).ToList();
        //    var listnew_secondaryclosingStock = this._mapper.Map<List<tbl_P3_SecondarySaleTransDetails_ClosingStock>>(new_secondaryclosingStock);
        //    listnew_secondaryclosingStock.ForEach(x =>
        //    {
        //        x.ForMonth = Month;
        //        x.ForYear = Year;
        //        x.FK_ClientID = 1;
        //        x.IsProcessed = false;
        //        //x.FreeSampleQTY = 0;
        //    });

        //    this.db.tbl_P3_SecondarySaleTransDetails_ClosingStock.AddRange(listnew_secondaryclosingStock);
        //    var result = await this.db.SaveChangesAsync();

        //    if (result > 0)
        //    {
        //        bresult = true;
        //    }
        //    return bresult;
        //}
        //#endregion

        //#endregion

        #region Division Sales Projection by SalesTeam 

        #region Sales Projection by SalesTeam  SearchFields
        public async Task<dynamic[]> SalesTeamProjection_SearchFields()
        {

            dynamic[] dynamics = new dynamic[4];

            var divisions = db.tbl_P3_SaleProjection_SalesTeam
               .Where(u => u.DivisionName != "CONTRACTUAL")
               .GroupBy(x => new { x.DivisionName })
               .Select(s => new
               {
                   id = s.Key.DivisionName,
                   text = s.Key.DivisionName,
               }).OrderBy(o => o.text).ToList();

            var depotName = db.tbl_P3_SaleProjection_SalesTeam
               .Where(u => u.DepotName != "FACTORY")
               .GroupBy(x => new { x.DepotName })
               .Select(s => new
               {
                   id = s.Key.DepotName,
                   text = s.Key.DepotName,
               }).OrderBy(o => o.text).ToList();

            var products = db.tbl_P3_SaleForecastingComparison.AsEnumerable().GroupBy(x => new { x.ProductName })
            .Select(s => new
            {
                id = s.Key.ProductName,
                //text = $"{s.Key.ProductName }({s.Key.PackUnit})",
                text = s.Key.ProductName,
                //packunit = s.Key.PackUnit
            }).OrderBy(o => o.text).ToList();

            var packunits = db.tbl_Master_Product.AsEnumerable().GroupBy(x => new { x.PackUnit, x.ProductUOM })
            .Select(s => new
            {
                id = s.Key.PackUnit,
                text = $"{s.Key.PackUnit }({s.Key.ProductUOM})",
                productUOM = s.Key.ProductUOM
            }).OrderBy(o => o.text).ToList();


            dynamics[0] = divisions;
            dynamics[1] = depotName;
            dynamics[2] = products;
            dynamics[3] = packunits;

            return await Task.FromResult(dynamics);
        }
        #endregion

        #region  Sales Projection by SalesTeam  Comparison
        /// <summary>
        /// This Fucntion Use In Memory Caching
        /// </summary>
        /// <param name="ForecastingForMonth"></param>
        /// <param name="ForecastingForYear"></param>
        /// <param name="Product"></param>
        /// <param name="Division"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public Tuple<List<SalesForecastEntity.SaleProjection_SalesTeam>, string> SalesTeamProjection_Show(int ForecastingForMonth, int ForecastingForYear,  string Division, string DepotName, string Product, string PackUnit,
                bool IsManual,  bool ReInitializeCache)
        {
            try
            {
                string[] arrDivision = string.IsNullOrEmpty(Division) == true ? new string[] { } : Division.Split(",");
                string[] arrDepotName = string.IsNullOrEmpty(DepotName) == true ? new string[] { } : DepotName.Split(",");
                string[] arrProduct = string.IsNullOrEmpty(Product) == true ? new string[] { } : Product.Split(",");
                string[] arrPackUnit = string.IsNullOrEmpty(PackUnit) == true ? new string[] { } : PackUnit.Split(",");

                var result = (from sf in db.tbl_P3_SaleProjection_SalesTeam.AsEnumerable()
                              join MasterMonth in db.tbl_Master_Month on sf.ForMonth equals MasterMonth.PK_MonthID into tmpMasterMonth
                              from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()
                              // Multiple Join Condition ---
                              join cs in db.tbl_P3_SaleForecasting_SwillClosingStock on
                                        new { sf.ForYear, sf.ForMonth, sf.DivisionName, sf.DepotName, sf.ProductName, sf.PackUnit }
                                        equals new {cs.ForYear ,cs.ForMonth, cs.DivisionName, cs.DepotName, cs.ProductName, cs.PackUnit }                                   
                              into tmpSwillClosingStock
                              from lftSwillClosingStock in tmpSwillClosingStock.DefaultIfEmpty()

                              where sf.ForMonth == ForecastingForMonth && sf.ForYear == ForecastingForYear
                              && sf.DivisionName == Division
                              && (IsManual == true ? sf.IsManual == true : sf.IsManual== sf.IsManual)
                              && (string.IsNullOrEmpty(DepotName) == true ? sf.DepotName == sf.DepotName : arrDepotName.Any(ax => ax.Contains(sf.DepotName)))
                              && (string.IsNullOrEmpty(Product) == true ? sf.ProductName == sf.ProductName : arrProduct.Any(ax => ax.Contains(sf.ProductName)))
                              && (string.IsNullOrEmpty(PackUnit) == true ? sf.PackUnit == sf.PackUnit : arrPackUnit.Any(ax => ax.Contains(sf.PackUnit)))
                               
                              orderby sf.DivisionName, sf.DepotName, sf.ProductName, sf.PackUnit

                              select new SalesForecastEntity.SaleProjection_SalesTeam
                              {
                                  ID = sf.PK_SalesTeamProjectionID,
                                  ForMonth = Convert.ToInt32(sf.ForMonth),
                                  ForYear = Convert.ToInt32(sf.ForYear),
                                  Month = lfttmpMasterMonth.MonthName + "-" + Convert.ToInt32(sf.ForYear).ToString(),
                                  DivisionName = sf.DivisionName,
                                  DepotName = sf.DepotName,
                                  ProductCode = sf.ProductCode,
                                  ProductName = sf.ProductName,
                                  PackUnit = sf.PackUnit,
                                  PrimaryTotalSalesQTY =  (decimal)sf.PrimaryTotalSalesQTY,
                                  TotalClosingStockQTY = lftSwillClosingStock==null ? 0: (decimal)lftSwillClosingStock.ClosingStockQTY,
                                  ProjectedTotalSalesQTY = (decimal)sf.ProjectedTotalSalesQTY,
                                  IsProcessed = (bool)sf.IsProcessed,
                                  NRVRate = (decimal)sf.NRVRate,
                                  ProjectionValue = (decimal)sf.ProjectionValue
                              }).ToList();

                string message = string.Empty;

                return new Tuple<List<SalesForecastEntity.SaleProjection_SalesTeam>, string>(result, message);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Update Team Data
        public async Task<bool> SalesTeamProjection_Save(SalesForecastEntity.SaleProjection_SalesTeam saleProjection_SalesTeam)
        {
            var tmp = new tbl_P3_SaleProjection_SalesTeam()
            {
                PK_SalesTeamProjectionID = saleProjection_SalesTeam.ID,
                ProjectedTotalSalesQTY = saleProjection_SalesTeam.ProjectedTotalSalesQTY,
                ProjectionValue = saleProjection_SalesTeam.ProjectedTotalSalesQTY * saleProjection_SalesTeam.NRVRate
            };

            db.tbl_P3_SaleProjection_SalesTeam.Attach(tmp);
            db.Entry(tmp).Property(p => p.ProjectedTotalSalesQTY).IsModified = true;
            db.Entry(tmp).Property(p => p.ProjectionValue).IsModified = true;
            var result = await this.db.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        #endregion

        #region Save New Team Data
        public async Task<bool> AddNew_SalesTeamProjection(SalesForecastEntity.SaleProjection_SalesTeam saleProjection_SalesTeam)
        {
            bool brecordcreated = false;
            saleProjection_SalesTeam.IsManual = true;
            saleProjection_SalesTeam.IsProcessed = false;
            saleProjection_SalesTeam.ProjectionDate = DateTime.Now;
            saleProjection_SalesTeam.ForecastingType = "SalesTeam";
            saleProjection_SalesTeam.HQ = "";
            SalesForecastEntity.SaleProjection_SalesTeam sanitized = this.functions.SetFKValueNullIfZero(saleProjection_SalesTeam);
            var tmpSalesTeamProjection = this._mapper.Map<tbl_P3_SaleProjection_SalesTeam>(sanitized);
            this.db.tbl_P3_SaleProjection_SalesTeam.Add(tmpSalesTeamProjection);
            var result = await db.SaveChangesAsync();
            if (result == 1)
            {
                brecordcreated = true;
            }
            return brecordcreated;
        }


        //public async Task<bool> AddNew_SalesTeamProjection(SalesForecastEntity.SaleProjection_SalesTeamEntry saleProjection_SalesTeam)
        //{
        //    bool brecordcreated = false;
        //    saleProjection_SalesTeam.IsManual = true;
        //    SalesForecastEntity.SaleProjection_SalesTeam sanitized = this.functions.SetFKValueNullIfZero(saleProjection_SalesTeam);
        //    var tmpSalesTeamProjection = this._mapper.Map<tbl_P3_SaleProjection_SalesTeam>(sanitized);
        //    this.db.tbl_P3_SaleProjection_SalesTeam.Add(tmpSalesTeamProjection);
        //    var result = await db.SaveChangesAsync();
        //    if (result == 1)
        //    {
        //        brecordcreated = true;
        //    }
        //    return brecordcreated;
        //}
        #endregion

        #region Update Manual Team Data
        public async Task<bool> Update_SalesTeamProjection(SalesForecastEntity.SaleProjection_SalesTeam saleProjection_SalesTeam)
        {
            var tmp = new tbl_P3_SaleProjection_SalesTeam()
            {
                PK_SalesTeamProjectionID = saleProjection_SalesTeam.ID,
                ForYear = saleProjection_SalesTeam.ForYear,
                ForMonth = saleProjection_SalesTeam.ForMonth,
                DivisionName = saleProjection_SalesTeam.DivisionName,
                DepotName = saleProjection_SalesTeam.DepotName,
                ProductCode = saleProjection_SalesTeam.ProductCode,
                ProductName = saleProjection_SalesTeam.ProductName,
                PackUnit = saleProjection_SalesTeam.PackUnit,
                PrimaryTotalSalesQTY = saleProjection_SalesTeam.PrimaryTotalSalesQTY,
                TotalClosingStockQTY = saleProjection_SalesTeam.TotalClosingStockQTY,
                ProjectedTotalSalesQTY = saleProjection_SalesTeam.ProjectedTotalSalesQTY,
                IsManual=true
            };

            db.tbl_P3_SaleProjection_SalesTeam.Attach(tmp);
            db.Entry(tmp).Property(p => p.ForYear).IsModified = true;
            db.Entry(tmp).Property(p => p.ForMonth).IsModified = true;
            db.Entry(tmp).Property(p => p.DivisionName).IsModified = true;
            db.Entry(tmp).Property(p => p.DepotName).IsModified = true;
            db.Entry(tmp).Property(p => p.ProductCode).IsModified = true;
            db.Entry(tmp).Property(p => p.PackUnit).IsModified = true;
            db.Entry(tmp).Property(p => p.PrimaryTotalSalesQTY).IsModified = true;
            db.Entry(tmp).Property(p => p.TotalClosingStockQTY).IsModified = true;
            db.Entry(tmp).Property(p => p.ProjectedTotalSalesQTY).IsModified = true;
            db.Entry(tmp).Property(p => p.IsManual).IsModified = true;

            var result = await this.db.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        #endregion
        #endregion

    }
}
