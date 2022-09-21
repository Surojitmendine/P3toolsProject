using API.Context;
using API.Helper;
using API.IdentityModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static API.Entity.MasterSetupEntity;

namespace API.BusinessLogic
{
    public class SetupProcessLogic
    {

        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private DBContext db;
        private readonly IMapper _mapper;
        private readonly Functions functions;
        private CommonLogic common;
        //private PumpSetup pumpSetup;
        readonly UserManager<ApplicationUser> userManager;
        public SetupProcessLogic()
        {
            this.functions = new Functions();
        }

        public SetupProcessLogic(DBContext db, IMapper mapper) : this()
        {

            this.db = db;
            this._mapper = mapper;
            this.common = new CommonLogic(db, mapper);

        }
        public SetupProcessLogic(DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager) : this(db, mapper)
        {
            this.userManager = userManager;
        }

        #region -- Sync Primary Data --
        public async Task DataTransfer(string FromDate, string ToDate, string SpToCall)
        {

            DateTime dtfromdate = this.functions.ConvertStringToDate(FromDate);
            DateTime dttodate = this.functions.ConvertStringToDate(ToDate);
            switch (SpToCall)
            {
                case "LoadPrimarySalesData":
                    var PrimarySalesData = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_01_LoadPrimary_SalesTransactionData @StartDate={FromDate},@EndDate={ToDate}");
                    break;

                case "LoadSecondarySalesData":
                    var SecondarySalesData = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_01_LoadSecondary_SalesTransactionData @StartDate={dtfromdate},@EndDate={dttodate}");
                    break;
            }
        }
        #endregion

        public async Task<dynamic> DataProcessing(Int32 Year, Int32 Month, string ForecastingType, string SpToCall)
        {
            //DateTime dtfromdate = this.functions.ConvertStringToDate(FromDate);
            //DateTime dttodate = this.functions.ConvertStringToDate(ToDate);
            dynamic result=0;
            switch (SpToCall)
            {
                case "AggregateSaleData":
                    result =await  this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_02_Process_AggregateSaleData @YearNo={Year},@MonthNo={Month}");
                    break;

                case "GenerateMonthlyProjection":
                    result = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_03_GenerateProjectionMonthlyData @YearNo={Year},@MonthNo={Month}");
                    break;

                case "GenerateSalesTeamProjection":
                    result = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_03_GenerateProjectionMonthlyData_SalesTeam @YearNo={Year},@MonthNo={Month}");
                    break;

                case "DownloadProjection":
                    result = this.DownloadProjection(Year, Month, ForecastingType, "usp_04_DownloadExcelProjectionMonthlyData"); // await this.db.Database.ex($"EXEC usp_04_DownloadExcelProjectionMonthlyData @YearNo={Year},@MonthNo={Month}, @ForecastingType={ForecastingType}");
                    break;

                case "GenerateSalesForecasting":
                    result = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_05_GenerateSalesForecasting @YearNo={Year},@MonthNo={Month}");
                    break;

                case "SalesForecastingComparison":
                    result = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_06_GenerateSalesForecastingComparison @YearNo={Year},@MonthNo={Month}");
                    break;
            }
            return result;
        }

        private dynamic DownloadProjection(Int32 Year, Int32 Month, string ForecastingType, string SpToCall)
        {
            DbCommand cmd = db.Database.GetDbConnection().CreateCommand();

            cmd.CommandText = SpToCall;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter(){DbType=DbType.Int32,ParameterName="@YearNo",Value=Year},
                new SqlParameter(){DbType=DbType.Int32,ParameterName="@MonthNo",Value=Month},
                new SqlParameter(){DbType=DbType.String,ParameterName="@ForecastingType",Value=ForecastingType},               

            });

            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }

            var reader =  cmd.ExecuteReader();

            DataTable result = new DataTable();
            result.Load(reader);           


            return result;


        }

        #region -- Sync Closing Stock Data --
        public async Task SyncClosingStockData(string FromDate, Int32 Year, Int32 Month)
        {
            DateTime dtfromdate = this.functions.ConvertStringToDate(FromDate);
            var PrimarySalesData = await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_01_LoadSwill_ClosingData @StartDate={FromDate},@ForYear={Year},@ForMonth={Month}");
        }

        #region List Primary  Sales
        public List<Divisionwise_ProductEntity> List_ClosingStock(Int32 Year, Int32 Month)
        {
            var result = (from sp in db.tbl_P3_SaleForecasting_SwillClosingStock
                          where (sp.ForYear == Year && sp.ForMonth== Month)
                          orderby sp.DivisionName,  sp.DepotName,  sp.ProductName, sp.PackUnit
                          select new Divisionwise_ProductEntity
                          {
                              //ForYear, ForMonth, SyncDate, DivisionName, DepotName, ProductCode, ProductName, PackUnit, ClosingStockQTY
                              PK_ID = sp.PK_SwillID,
                              SyncDate = sp.SyncDate.Value.Date.ToString("dd/MM/yyyy"),
                              ForYear = sp.ForYear,
                              ForMonth=sp.ForMonth,
                              DivisionName = sp.DivisionName,
                              DepotName = sp.DepotName,
                              ProductName = sp.ProductName,
                              PackUnit = sp.PackUnit,
                              ClosingStockQTY = (decimal)sp.ClosingStockQTY,
                          }).ToList();
            return result;
        }
        #endregion

        #endregion

    }
}
