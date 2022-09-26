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
using Microsoft.EntityFrameworkCore;
using static API.Entity.ProductionPlan;
using System.Data;
using Common.Utility;

namespace API.BusinessLogic
{
    public class ProductionPlanLogic
    {
        #region "Declaration"
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private DBContext db;
        private readonly IMapper _mapper;
        private readonly Functions functions;
        private CommonLogic common;
        readonly UserManager<ApplicationUser> userManager;
        private readonly IMemoryCache memoryCache;

        public ProductionPlanLogic()
        {
            this.functions = new Functions();
        }

        public ProductionPlanLogic(DBContext db, IMapper mapper) : this()
        {
            this.db = db;
            this._mapper = mapper;
            this.common = new CommonLogic(db, mapper);
        }
        public ProductionPlanLogic(DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager) : this(db, mapper)
        {
            this.userManager = userManager;
        }
        public ProductionPlanLogic(DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager, IMemoryCache memoryCache) : this(db, mapper, userManager)
        {
            this.memoryCache = memoryCache;
        }

        #endregion

        #region "Tally Product Batch"

        #region  -- List TallyProductBatch --
        public static List<ProductionPlan.ImportExcel_TallyProductBatch> List_TallyProductBatch(String Connection)
        {
            List<ImportExcel_TallyProductBatch> mlist = new List<ImportExcel_TallyProductBatch>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "uspGet_Tally_Product_Batch_List");
            foreach (DataRow DR in DT.Rows)
            {
                ImportExcel_TallyProductBatch obj = new ImportExcel_TallyProductBatch();
                obj.CompanyId = DR["CompanyId"].ToString();
                obj.ProductName = DR["product_name"].ToString();
                obj.UOM = DR["UOM"].ToString();
                obj.BatchSize = DR["Batch_size"].ToString();
                obj.BOMName = DR["BOM_name"].ToString();
                mlist.Add(obj);
            }
            return mlist;

        }
        #endregion

        #region SaveExcel Tally Product Batch
        private static DataTable DTTallyProductBatch()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("CompanyId", typeof(System.String));
            DT.Columns.Add("ProductName", typeof(System.String));
            DT.Columns.Add("UOM", typeof(System.String));
            DT.Columns.Add("BatchSize", typeof(System.String));
            DT.Columns.Add("BOMName", typeof(System.String));
            return DT;
        }
        public static String SaveExcel_TallyProductBatch(String Connection, List<ProductionPlan.ImportExcel_TallyProductBatch> tallyproductbatch)
        {
            DataTable tallyproductbatchDT = DTTallyProductBatch();
            foreach (var item in tallyproductbatch)
            {
                DataRow DR = tallyproductbatchDT.NewRow();
                DR["CompanyId"] = item.CompanyId;
                DR["ProductName"] = item.ProductName;
                DR["UOM"] = item.UOM;
                DR["BatchSize"] = item.BatchSize;
                DR["BOMName"] = item.BOMName;
                tallyproductbatchDT.Rows.Add(DR);
            }
            return clsDatabase.fnDBOperation(Connection, "Proc_Insert_TallyProductBatch_Excel", tallyproductbatchDT);
        }

        #endregion

        #endregion

        #region Factory Closing Stock
        #region  -- List FactoryClosingStock --
        public static List<ProductionPlan.ImportExcel_FactoryClosingStock> List_FactoryClosingStock(String Connection, int MonthNo, int YearNo)
        {
            List<ImportExcel_FactoryClosingStock> mlist = new List<ImportExcel_FactoryClosingStock>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "usp_Get_factory_closing_stock_List", MonthNo, YearNo);
            foreach (DataRow DR in DT.Rows)
            {
                ImportExcel_FactoryClosingStock obj = new ImportExcel_FactoryClosingStock();
                obj.CompanyId = DR["CompanyId"].ToString();
                //obj.SLNO = clsHelper.fnConvert2Int(DR["SLNO"]);
                obj.Stock_date = DR["Stock_date"].ToString();
                obj.St_group = DR["St_group"].ToString();
                obj.St_category = DR["St_category"].ToString();
                obj.product_name = DR["product_name"].ToString();
                obj.quantity = clsHelper.fnConvert2Decimal(DR["quantity"]);
                obj.UOM = DR["UOM"].ToString();
                obj.rate = clsHelper.fnConvert2Decimal(DR["rate"]);
                obj.amount = clsHelper.fnConvert2Decimal(DR["amount"]);
                mlist.Add(obj);
            }
            return mlist;

        }
        #endregion

        #region SaveExcel Factory Closing Stock
        private static DataTable DTFactoryClosingStock()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("CompanyId", typeof(System.String));
            DT.Columns.Add("Stock_date", typeof(System.String));
            DT.Columns.Add("St_group", typeof(System.String));
            DT.Columns.Add("St_category", typeof(System.String));
            DT.Columns.Add("product_name", typeof(System.String));
            DT.Columns.Add("quantity", typeof(System.Decimal));
            DT.Columns.Add("UOM", typeof(System.String));
            DT.Columns.Add("rate", typeof(System.Decimal));
            DT.Columns.Add("amount", typeof(System.Decimal));
            return DT;
        }
        public static String SaveExcel_FactoryClosingStock(String Connection, List<ProductionPlan.ImportExcel_FactoryClosingStock> factoryClosingStock)
        {
            DataTable factoryClosingStockDT = DTFactoryClosingStock();
            foreach (var item in factoryClosingStock)
            {
                DataRow DR = factoryClosingStockDT.NewRow();
                DR["CompanyId"] = item.CompanyId;
                DR["Stock_date"] = item.Stock_date;
                DR["St_group"] = item.St_group;
                DR["St_category"] = item.St_category;
                DR["product_name"] = item.product_name;
                DR["quantity"] = item.quantity;
                DR["UOM"] = item.UOM;
                DR["rate"] = item.rate;
                DR["amount"] = item.amount;
                factoryClosingStockDT.Rows.Add(DR);
            }
            return clsDatabase.fnDBOperation(Connection, "Proc_Insert_FactoryClosingStock_Excel", factoryClosingStockDT);
        }

        #endregion
        
        #endregion

        #region "PhysicianSamplePlan"

        #region -- SaveExcel --
        public async Task<bool> SaveExcel_PhysicianSamplePlan(ApplicationUser user, Int32 Year, Int32 Month, List<ProductionPlan.ImportExcel_PhysicianSamplePlan> PhysicianSamplePlans)
        {

            var existingrecords = db.tbl_P3_Production_PhysicianSample_AGG.Where(x => x.ForYear == Year && x.ForMonth == Month).Select(s => s).ToList();
            if (existingrecords.Count > 0)
            {
                db.RemoveRange(existingrecords);
               // var deleted = await db.SaveChangesAsync();
            }

            // ADD NEW RECORDS ....
            bool bresult = false;
            var newprojections = PhysicianSamplePlans.Where(Ps => string.IsNullOrEmpty(Ps.ID.ToString()) == true || Ps.ID <= 0).ToList();
            var updatedprojections = PhysicianSamplePlans.Where(projection => projection.ID > 0).ToList();
            var listnewprojections = this._mapper.Map<List<tbl_P3_Production_PhysicianSample_AGG>>(newprojections);
            var listupdatedprojections = this._mapper.Map<List<tbl_P3_Production_PhysicianSample_AGG>>(updatedprojections);

            listnewprojections.ForEach(x =>
            {
                x.CreatedDate = DateTime.Now;
                x.FK_CreatedByID = user.Id;
            });

            this.db.tbl_P3_Production_PhysicianSample_AGG.AddRange(listnewprojections);
            var result = await this.db.SaveChangesAsync();

            if (result > 0)
            {
                bresult = true;
            }
            return bresult;
        }
        #endregion

        #region  -- List --
        public List<ProductionPlan.ImportExcel_PhysicianSamplePlan> List_PhysicianSamplePlan(int MonthNo, int YearNo)
        {
            var result = (from sp in db.tbl_P3_Production_PhysicianSample_AGG

                          join m in db.tbl_Master_Month on sp.ForMonth equals m.PK_MonthID into tmpm
                          from lftm in tmpm.DefaultIfEmpty()
                          where sp.ForYear == YearNo && sp.ForMonth == MonthNo
                          orderby sp.ProductName, sp.PackUnit
                          select new ProductionPlan.ImportExcel_PhysicianSamplePlan
                          {
                              ID = sp.PK_PhysicianSampleID,
                              MonthName = lftm.ShortMonthName + "-" + YearNo,// MonthNo.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")) + "-" + YearNo,
                              ProductName = sp.ProductName,
                              PackUnit = sp.PackUnit,
                              PhysicianSampleQTY = (decimal)sp.PhysicianSampleQTY

                          }).ToList();

            return result;

        }

        #endregion

        #endregion

        #region Production Plan

        #region  -- Volume Conversion --
        public List<ProductionPlan.ProductionPlan_VolumeConversion> List_ProductionPlan_VolumeConversion(int MonthNo, int YearNo)
        {
            var result = (from sp in db.tbl_P3_Production_Forecasting_SKU

                          join m in db.tbl_Master_Month on sp.ForMonth equals m.PK_MonthID into tmpm
                          from lftm in tmpm.DefaultIfEmpty()

                          where sp.ForYear == YearNo && sp.ForMonth == MonthNo
                          orderby sp.ProductName, sp.PackUnit
                          select new ProductionPlan.ProductionPlan_VolumeConversion
                          {
                              ForMonth = lftm.MonthName + "-" + YearNo,
                              ProductType = sp.ProductType,
                              Category = sp.ProductCategory,
                              ProductName = sp.ProductName,
                              PackUnit = Convert.ToInt32(sp.PackUnit),
                              ProjectionForecastQTY = Convert.ToDecimal(sp.ProductionForecastQTY),
                              Factor = Convert.ToDecimal(sp.FactorValue),
                              FactorProjectionForecastQTY = Convert.ToDecimal(sp.FactorForecastQTY),
                              VolumeInLtrs = Convert.ToDecimal(sp.ChargeableVolume_InLtr)
                          }).ToList();
            return result;
        }
        #endregion

        #region  -- Volume Charge --
        public List<ProductionPlan.ProductionPlan_VolumeCharge> List_ProductionPlan_VolumeCharge(int MonthNo, int YearNo)
        {
            var result = (from sp in db.tbl_P3_Production_Forecasting_Product
                          join m in db.tbl_Master_Month on sp.ForMonth equals m.PK_MonthID into tmpm
                          from lftm in tmpm.DefaultIfEmpty()
                          where sp.ForYear == YearNo && sp.ForMonth == MonthNo
                          orderby sp.ProductName
                          select new ProductionPlan.ProductionPlan_VolumeCharge
                          {
                              ForMonth = lftm.MonthName + "-" + YearNo,
                              ProductName = sp.ProductName,
                              VolumeInLtrs = Convert.ToDecimal(sp.Volumn),
                              WIPInLtrs = Convert.ToDecimal(sp.WIPQTY),
                              ChargeableVolumeInLtrs = Convert.ToDecimal(sp.ChargeableVolume_InLtr),
                              BatchSize = Convert.ToDecimal(sp.BatchSize),
                              FinalChargeInLtrs = Convert.ToDecimal(sp.FinalChargeableVolume_InLtr)
                          }).ToList();
            return result;
        }
        #endregion

        #region  -- Volume Final Charge Unit --
        public List<ProductionPlan.ProductionPlan_FinalChargeUnit> List_ProductionPlan_FinalChargeUnit(int MonthNo, int YearNo)
        {
            var result = (from sp in db.tbl_P3_Production_Forecasting_SKU
                          join m in db.tbl_Master_Month on sp.ForMonth equals m.PK_MonthID into tmpm
                          from lftm in tmpm.DefaultIfEmpty()
                          where sp.ForYear == YearNo && sp.ForMonth == MonthNo
                          orderby sp.ProductName, sp.PackUnit
                          select new ProductionPlan.ProductionPlan_FinalChargeUnit
                          {
                              ForMonth = lftm.MonthName + "-" + YearNo,
                              ProductType = sp.ProductType,
                              Category = sp.ProductCategory,
                              ProductName = sp.ProductName,
                              PackUnit = Convert.ToInt32(sp.PackUnit),
                              FactorProjectionForecastQTY = Convert.ToDecimal(sp.FactorForecastQTY),
                              FinalChargeInUnit = Convert.ToDecimal(sp.FinalChargeableVolume_InLtr)
                          }).ToList();
            return result;
        }
        #endregion

        public async Task<Int32> ProductionPlaning_DataProcessing(Int32 Year, Int32 Month, string SpToCall)
        {
            //var startDate = new DateTime(Year, Month, 1);
            //var endDate = startDate.AddMonths(1).AddDays(-1);
            int result = 0;
            switch (SpToCall)
            {
                case "SyncSQLDepotSKUStock":
                    result = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_Production_01_TransferDepotStockFromEasyReport @YearNo={Year},@MonthNo={Month}");
                    break;

                case "SyncSQLWIPStock":
                    result = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_Production_03_TransferWIPStockFromEasyReport @YearNo={Year},@MonthNo={Month}");
                    break;

                case "ProcessAggregateDepotStock":
                    result = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_Production_02_Process_AggregateDepotStock @YearNo={Year},@MonthNo={Month}");
                    break;

                case "ProcessAggregateWIPStock":
                    result = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_Production_04_Process_AggregateWIPStock @YearNo={Year},@MonthNo={Month}");
                    break;

                case "GenerateProductionPlanning":
                    result = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_Production_05_ProcessProductionPlanning @YearNo={Year},@MonthNo={Month}");
                    break;
            }
            return result;
        }

        #endregion

    }
}