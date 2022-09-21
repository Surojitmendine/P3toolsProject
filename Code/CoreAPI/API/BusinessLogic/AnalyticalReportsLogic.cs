using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Helper;
using API.Helper.ReqObject;
using API.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.IdentityModels;
using static API.Entity.CommonEntity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using API.Entity;
using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;

namespace API.BusinessLogic
{
    public class AnalyticalReportsLogic
    {
        #region -Init-
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private DBContext db;
        private readonly IMapper _mapper;
        private readonly Functions functions;
        private CommonLogic common;
        readonly UserManager<ApplicationUser> userManager;

        public AnalyticalReportsLogic()
        {
            this.functions = new Functions();
        }

        public AnalyticalReportsLogic(DBContext db, IMapper mapper) : this()
        {
            this.db = db;
            this._mapper = mapper;
            this.common = new CommonLogic(db, mapper);
        }
        public AnalyticalReportsLogic(DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager) : this(db, mapper)
        {
            this.userManager = userManager;
        }
        #endregion

        #region -- Depot Replenishment Indent Summary --
        #region -- Search Fields --
        public async Task<dynamic[]> Reports_SearchFields()
        {
            dynamic[] dynamics = new dynamic[4];
            var division = db.tbl_P3_SaleForecastingComparison.GroupBy(x => new { x.DivisionName })
            .Select(s => new
            {
                id = s.Key.DivisionName,
                text = s.Key.DivisionName,

            }).OrderBy(o => o.text).ToList();


            var depot = db.tbl_P3_SaleForecastingComparison.GroupBy(x => new { x.DepotName })
             .Select(s => new
             {
                 id = s.Key.DepotName,
                 text = s.Key.DepotName,

             }).OrderBy(o => o.text).ToList();


            var product = db.tbl_P3_SaleForecastingComparison.AsEnumerable().GroupBy(x => new { x.ProductName })
            .Select(s => new
            {
                id = s.Key.ProductName,
                text = s.Key.ProductName,
            }).OrderBy(o => o.text).ToList();


            var packunit = db.tbl_Master_Product.AsEnumerable().GroupBy(x => new { x.PackUnit, x.ProductUOM })
            .Select(s => new
            {
                id = s.Key.PackUnit,
                text = $"{s.Key.PackUnit }({s.Key.ProductUOM})",
                productUOM = s.Key.ProductUOM

            }).OrderBy(o => o.text).ToList();

            dynamics[0] = division;
            dynamics[1] = depot;
            dynamics[2] = product;
            dynamics[3] = packunit;

            return await Task.FromResult(dynamics);
        }
        #endregion

        #region Sales Forecastin Comparison Summary
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ForYear"></param>
        /// <param name="ForMonth"></param>
        /// <param name="Divisions"></param>
        /// <param name="Depots"></param>
        /// <param name="Products"></param>
        /// <param name="PackUints"></param>
        /// <param name="ReInitializeCache"></param>
        /// <returns></returns>
        public async Task<Tuple<List<AnalyticalReportsEntity.DepotReplenishmentIndentSummary>, string>> DepotReplenishment_IndentSummary(int ForYear, int ForMonth,
            string Divisions, string Depots, string Products, string PackUints, bool ReInitializeCache)
        {

            string[] arrProduct = string.IsNullOrEmpty(Products) == true ? new string[] { } : Products.Split(",");
            string[] arrDivision = string.IsNullOrEmpty(Divisions) == true ? new string[] { } : Divisions.Split(",");
            string[] arrLocation = string.IsNullOrEmpty(Depots) == true ? new string[] { } : Depots.Split(",");
            string[] arrPackUint = string.IsNullOrEmpty(PackUints) == true ? new string[] { } : PackUints.Split(",");
            string message = string.Empty;
            try
            {
                DbCommand cmd = db.Database.GetDbConnection().CreateCommand();

                cmd.CommandText = "usp_Report_DepotReplishmentIndentSummary";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(new SqlParameter[]
                {
                new SqlParameter(){DbType=DbType.Int32,ParameterName="@ForYear",Value=ForYear},
                new SqlParameter(){DbType=DbType.Int32,ParameterName="@ForMonth",Value=ForMonth},
                new SqlParameter(){DbType=DbType.String,ParameterName="@Division",Value=Divisions},
                new SqlParameter(){DbType=DbType.String,ParameterName="@Depot",Value=Depots},
                new SqlParameter(){DbType=DbType.String,ParameterName="@ProductName",Value=Products},
                new SqlParameter(){DbType=DbType.String,ParameterName="@PackSize",Value=PackUints},
                });

                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                var reader = cmd.ExecuteReader();
                DataTable result = new DataTable();
                result.Load(reader);

                List<AnalyticalReportsEntity.DepotReplenishmentIndentSummary> indentsummary = new List<AnalyticalReportsEntity.DepotReplenishmentIndentSummary>();
                indentsummary = (from DataRow dr in result.Rows
                                select new AnalyticalReportsEntity.DepotReplenishmentIndentSummary()
                                {
                                    ForYear = ForYear,
                                    ForMonth =  new DateTime(ForYear, ForMonth, 1).ToString("MMM", CultureInfo.InvariantCulture),
                                    DivisionName = dr["Division"].ToString(),
                                    DepotName = dr["Depot"].ToString(),
                                    ProductName = dr["ProductName"].ToString(),
                                    PackUnit = dr["Pack"].ToString(),
                                    FinalProjection = Convert.ToDecimal(dr["finalprojection"]),
                                    ClosingStock = Convert.ToDecimal(dr["closingstock"]),
                                    LTFactor = Convert.ToDecimal(dr["ltfactor"]),
                                    DepotReplenishmentIndent = Convert.ToDecimal(dr["depotreplenishmentindent"]),
                                    CumulativeDepotSentQTY = Convert.ToDecimal(dr["cumulativedepotsentquanytity"]),
                                    PendingQTY = Convert.ToDecimal(dr["Pending"]),
                                }).ToList();
                return new Tuple<List<AnalyticalReportsEntity.DepotReplenishmentIndentSummary>, string>(indentsummary, message);
            }
            catch (Exception ex)
            {
                return null;
            }

            //List<AnalyticalReportsEntity.DepotReplenishmentIndentSummary> indentsummary = new List<AnalyticalReportsEntity.DepotReplenishmentIndentSummary>();
            //return new Tuple<List<AnalyticalReportsEntity.DepotReplenishmentIndentSummary>, string>(indentsummary, message);
        }



        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }

        #endregion


        #endregion
    }
}
