using API.Context;
using API.Entity;
using API.Helper;
using API.IdentityModels;
using API.Models;
using AutoMapper;
using Common.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static API.Entity.MasterSetupEntity;

namespace API.BusinessLogic
{
    public class MasterSetupLogic
    {
        #region Declaration
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private DBContext db;
        private readonly IMapper _mapper;
        private readonly Functions functions;
        private CommonLogic common;
        private MasterSetupLogic masterSetup;
        readonly UserManager<ApplicationUser> userManager;

        public MasterSetupLogic()
        {
            this.functions = new Functions();
        }
        public MasterSetupLogic(DBContext db, IMapper mapper) : this()
        {
            this.db = db;
            this._mapper = mapper;
            this.common = new CommonLogic(db, mapper);

        }
        public MasterSetupLogic(DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager) : this(db, mapper)
        {
            this.userManager = userManager;
        }
        #endregion


        #region PRODUCT MASTER

        #region -- Division Product  SearchFields
        public async Task<dynamic[]> ProductMaster_SearchFields()
        {

            dynamic[] dynamics = new dynamic[1];

            var categories = db.tbl_Master_ProductCategory.GroupBy(x => new {x.PK_ProductCategoryID, x.CategoryName })
               .Select(s => new
               {
                   id = s.Key.PK_ProductCategoryID,
                   text = s.Key.CategoryName,
               }).OrderBy(o => o.text).ToList();

            dynamics[0] = categories;

            return await Task.FromResult(dynamics);
        }
        #endregion

        #region --  Product Master--

        #region -- List of Product Master --
        public List<MasterSetupEntity.ProductMaster> List_ProductMaster()
        {

            var result = (from Pro in db.tbl_Master_Product
                          join MasterMonth in db.tbl_Master_ProductCategory on Pro.FK_ProductCategoryID equals MasterMonth.PK_ProductCategoryID into tmpMasterMonth
                          from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()
                          orderby Pro.ProductCode, Pro.ProductName, Pro.PackUnit                          
                          select new MasterSetupEntity.ProductMaster
                          {
                              PK_ProductID = Pro.PK_ProductID,
                              ProductCode = Pro.ProductCode,
                              Category = lfttmpMasterMonth.CategoryName,
                              ProductName = Pro.ProductName,
                              PackUnit = Pro.PackUnit,
                              ProductType = Pro.ProductType,
                              ProductCategory = Pro.ProductCategory,
                              ProductUOM = Pro.ProductUOM,
                              FactorValue = (decimal)Pro.FactorValue,
                              BatchSize = (decimal)Pro.BatchSize,
                              NRVRate = (decimal)Pro.NRVRate,
                              NRVEffectiveRateFrom = Pro.NRVEffectiveRateFrom,
                          }).ToList();
            return result;
        }
        #endregion

        #region -- Add New Product Master --
        public async Task<bool> AddNew_ProductMaster(MasterSetupEntity.ProductMaster product)
        {
            bool brecordcreated = false;
            MasterSetupEntity.ProductMaster sanitized = this.functions.SetFKValueNullIfZero(product);
            var tmpProductMaster = this._mapper.Map<tbl_Master_Product>(sanitized);
            this.db.tbl_Master_Product.Add(tmpProductMaster);
            var result = await db.SaveChangesAsync();
            if (result == 1)
            {
                brecordcreated = true;
            }
            return brecordcreated;
        }
        #endregion

        #region -- Update Product Master --
        public async Task<bool> Update_ProductMaster(MasterSetupEntity.ProductMaster d, ApplicationUser applicationUser)
        {
            bool brecordupdated = false;
            var tbl_Master_Product = new tbl_Master_Product()
            {
                PK_ProductID = d.PK_ProductID,
                FK_ProductCategoryID = d.FK_ProductCategoryID,
                ProductCode = d.ProductCode,
                ProductName = d.ProductName,
                ProductType = d.ProductType,
                PackUnit = d.PackUnit,
                ProductCategory = d.ProductCategory,
                ProductUOM = d.ProductUOM,
                FactorValue = d.FactorValue,
                BatchSize = d.BatchSize,
                NRVRate = d.NRVRate,
                NRVEffectiveRateFrom = d.NRVEffectiveRateFrom,
            };
            db.tbl_Master_Product.Attach(tbl_Master_Product);
            db.Entry(tbl_Master_Product).Property(x => x.ProductCode).IsModified = true;
            db.Entry(tbl_Master_Product).Property(x => x.ProductName).IsModified = true;
            db.Entry(tbl_Master_Product).Property(x => x.ProductType).IsModified = true;
            db.Entry(tbl_Master_Product).Property(x => x.PackUnit).IsModified = true;
            db.Entry(tbl_Master_Product).Property(x => x.FactorValue).IsModified = true;
            db.Entry(tbl_Master_Product).Property(x => x.BatchSize).IsModified = true;
            db.Entry(tbl_Master_Product).Property(x => x.FactorValue).IsModified = true;
            db.Entry(tbl_Master_Product).Property(x => x.NRVRate).IsModified = true;
            db.Entry(tbl_Master_Product).Property(x => x.NRVEffectiveRateFrom).IsModified = true;
            var result = await db.SaveChangesAsync();
            if (result == 1)
            {
                brecordupdated = true;
            }
            return brecordupdated;
        }

        #endregion

        #region -- Get by Product ID --
        public async Task<MasterSetupEntity.ProductMaster> GetByID_ProductMaster(Int32 Id)
        {
            try
            {
                var tmp_Product = await this.db.tbl_Master_Product.Where(x => x.PK_ProductID == Id).Select(s => s).SingleOrDefaultAsync();
                var ProductMaster = this._mapper.Map<MasterSetupEntity.ProductMaster>(tmp_Product);
                return ProductMaster;
            }
            catch(Exception ex)
            {
                return null;
            }

        }
        #endregion

        #region -- Update Product Mapping Master --
        //public async Task<bool> Update_ProductMappingMaster(MasterSetupEntity.ProductMasterMapping product)
        //{
        //    bool brecordcreated = false;
        //    MasterSetupEntity.ProductMasterMapping sanitized = this.functions.SetFKValueNullIfZero(product);
        //    var tmpProductMaster = this._mapper.Map<tbl_Master_Product>(sanitized);
        //    this.db.tbl_Master_Product.Add(tmpProductMaster);
        //    var result = await db.SaveChangesAsync();
        //    if (result == 1)
        //    {
        //        brecordcreated = true;
        //    }
        //    return brecordcreated;
        //}

        public async Task Update_ProductMappingMaster(MasterSetupEntity.ProductMasterMapping product)
        {
            await this.db.Database.ExecuteSqlInterpolatedAsync($"EXEC usp_Update_Product_Mapping @TallyProductName={product.TallyProductName},@TallyUOM={product.TallyUOM},@ProductID={product.PK_ProductID}");
        }

        #endregion

        #region -- List of Product Mapping Master --
        public List<MasterSetupEntity.ProductMasterMapping> List_ProductMappingMaster(string Type)
        {

            var result = (from Pro in db.tbl_Master_Product
                          join MasterMonth in db.tbl_Master_ProductCategory on Pro.FK_ProductCategoryID equals MasterMonth.PK_ProductCategoryID into tmpMasterMonth
                          from lfttmpMasterMonth in tmpMasterMonth.DefaultIfEmpty()
                          where Pro.TallyProductName == (Type == "Not Mapped" ? null : Pro.TallyProductName)
                          orderby Pro.ProductCode, Pro.ProductName, Pro.PackUnit
                          select new MasterSetupEntity.ProductMasterMapping
                          {
                              PK_ProductID = Pro.PK_ProductID,
                              ProductCode = Pro.ProductCode,
                              ProductCategory = lfttmpMasterMonth.CategoryName,
                              ProductName = Pro.ProductName,
                              PackUnit = Pro.PackUnit,
                              TallyProductName = Pro.TallyProductName,
                              TallyUOM = Pro.TallyUOM
                          }).ToList();
            return result;
        }
        #endregion

        #region -- Get by Product Mapping ID --

        public static List<MasterSetupEntity.ProductMasterMapping> GetByID_ProductMappingMaster(String Connection,Int32 Id)
        {
            List<ProductMasterMapping> mlist = new List<ProductMasterMapping>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "usp_Product_Mapping_GetByID", Id);
            foreach (DataRow DR in DT.Rows)
            {
                ProductMasterMapping obj = new ProductMasterMapping();
                obj.PK_ProductID = clsHelper.fnConvert2Int(DR["PK_ProductID"]);
                obj.ProductCategory = DR["ProductCategory"].ToString();
                obj.ProductCode = DR["ProductCode"].ToString();
                obj.ProductName = DR["ProductName"].ToString();
                obj.PackUnit = clsHelper.fnConvert2Int(DR["PackUnit"]);
                obj.TallyProductName = DR["TallyProductName"].ToString();
                obj.TallyUOM = DR["TallyUOM"].ToString();
                mlist.Add(obj);
            }
            return mlist;
        }

        //public async Task<MasterSetupEntity.ProductMasterMapping> GetByID_ProductMappingMaster(Int32 Id)
        //{
        //    try
        //    {
        //        var tmp_Product = await this.db.tbl_Master_Product.Where(x => x.PK_ProductID == Id).Select(s => new { s.PK_ProductID, s.ProductCategory, s.ProductCode, s.ProductName, s.PackUnit, s.TallyProductName, s.TallyUOM }).SingleOrDefaultAsync();
        //        var ProductMaster = this._mapper.Map<MasterSetupEntity.ProductMasterMapping>(tmp_Product);
        //        return ProductMaster;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}
        #endregion

        #endregion


        #endregion

        #region Division Wise Product

        #region -- Master_Divisionwise_Product --

        #region -- List of Divisionwise_ProductEntity --
        public async Task<List<dynamic>> List_Divisionwise_Product(string Division, string DepotName, string Product, string PackUnit)
        {
            string[] arrDivision = string.IsNullOrEmpty(Division) == true ? new string[] { } : Division.Split(",");
            string[] arrDepotName = string.IsNullOrEmpty(DepotName) == true ? new string[] { } : DepotName.Split(",");
            string[] arrProduct = string.IsNullOrEmpty(Product) == true ? new string[] { } : Product.Split(",");
            string[] arrPackUnit = string.IsNullOrEmpty(PackUnit) == true ? new string[] { } : PackUnit.Split(",");

            List<dynamic> lstData = (from Pro in db.tbl_P3_Master_Divisionwise_Product.AsEnumerable()
                                     where (string.IsNullOrEmpty(Division) == true ? Pro.DivisionName == Pro.DivisionName : arrDivision.Any(ax => ax.Contains(Pro.DivisionName)))
                                     && (string.IsNullOrEmpty(DepotName) == true ? Pro.DepotName == Pro.DepotName : arrDepotName.Any(ax => ax.Contains(Pro.DepotName)))
                                      && (string.IsNullOrEmpty(Product) == true ? Pro.ProductName == Pro.ProductName : arrProduct.Any(ax => ax.Equals(Pro.ProductName)))
                                      && (string.IsNullOrEmpty(PackUnit) == true ? Pro.PackUnit == Pro.PackUnit : arrPackUnit.Any(ax => ax.Contains(Pro.PackUnit)))

                                     orderby Pro.DivisionName, Pro.DepotName, Pro.ProductName, Pro.PackUnit
                                     select new //MasterSetupEntity.Divisionwise_ProductEntity
                                     {
                                         ID = Pro.PK_ID,
                                         DivisionName = Pro.DivisionName,
                                         DepotName = Pro.DepotName,
                                         ProductCode = Pro.ProductCode,
                                         ProductName = Pro.ProductName,
                                         PackUnit = Pro.PackUnit,
                                         IsActive = Pro.IsActive
                                     }).ToList<dynamic>();
            return lstData;
        }
        #endregion

        #region -- Add New Divisionwise_ProductEntity --
        public async Task<bool> AddNew_Divisionwise_Product(Divisionwise_ProductEntity product)
        {
            bool brecordcreated = false;
            Divisionwise_ProductEntity sanitized = this.functions.SetFKValueNullIfZero(product);
            var tmpDivisionwise_Product = this._mapper.Map<tbl_P3_Master_Divisionwise_Product>(sanitized);
            this.db.tbl_P3_Master_Divisionwise_Product.Add(tmpDivisionwise_Product);
            var result = await db.SaveChangesAsync();
            if (result == 1)
            {
                brecordcreated = true;
            }
            return brecordcreated;
        }
        #endregion

        #region -- Update Divisionwise_ProductEntity --
        public async Task<bool> Update_Divisionwise_Product(Divisionwise_ProductEntity d, ApplicationUser applicationUser)
        {
            bool brecordupdated = false;
            var tbl_P3_Master_Divisionwise_Product = new tbl_P3_Master_Divisionwise_Product()
            {
                PK_ID = d.PK_ID,
                DivisionName = d.DivisionName,
                DepotName = d.DepotName,
                ProductCode = d.ProductCode,
                ProductName = d.ProductName,
                PackUnit = d.PackUnit,
                IsActive = d.IsActive,
            };
            db.tbl_P3_Master_Divisionwise_Product.Attach(tbl_P3_Master_Divisionwise_Product);
            db.Entry(tbl_P3_Master_Divisionwise_Product).Property(x => x.DivisionName).IsModified = true;
            db.Entry(tbl_P3_Master_Divisionwise_Product).Property(x => x.DepotName).IsModified = true;
            db.Entry(tbl_P3_Master_Divisionwise_Product).Property(x => x.ProductCode).IsModified = true;
            db.Entry(tbl_P3_Master_Divisionwise_Product).Property(x => x.ProductName).IsModified = true;
            db.Entry(tbl_P3_Master_Divisionwise_Product).Property(x => x.PackUnit).IsModified = true;
            db.Entry(tbl_P3_Master_Divisionwise_Product).Property(x => x.IsActive).IsModified = true;
            var result = await db.SaveChangesAsync();
            if (result == 1)
            {
                brecordupdated = true;
            }
            return brecordupdated;
        }

        #endregion

        #region -- Get by Divisionwise_ProductEntity ID --
        public async Task<Divisionwise_ProductEntity> GetByID_Divisionwise_Product(Int32 Id)
        {
            var tmpDivisionwise_Product = await this.db.tbl_P3_Master_Divisionwise_Product.Where(x => x.PK_ID == Id).Select(s => s).SingleOrDefaultAsync();
            var divisionwise_Product = this._mapper.Map<Divisionwise_ProductEntity>(tmpDivisionwise_Product);
            return divisionwise_Product;
        }
        #endregion

        #region "Delete by ID"
        public async Task<bool> Delete_Divisionwise_Product(Int32 Id)
        {
            bool brecordupdated = false;
            var tmpDivisionwise_Product = await this.db.tbl_P3_Master_Divisionwise_Product.Where(x => x.PK_ID == Id).Select(s => s).SingleOrDefaultAsync();

            db.Entry(tmpDivisionwise_Product).State = EntityState.Modified;

            var result = await db.SaveChangesAsync();
            if (result == 1)
            {
                brecordupdated = true;
            }
            return brecordupdated;
        }
        #endregion

        #endregion


        #region -- Division Product  SearchFields
        public async Task<dynamic[]> DivisionProduct_SearchFields()
        {

            dynamic[] dynamics = new dynamic[4];

            var divisions = db.tbl_P3_Master_Divisionwise_Product.GroupBy(x => new { x.DivisionName })
               .Select(s => new
               {
                   id = s.Key.DivisionName,
                   text = s.Key.DivisionName,
               }).OrderBy(o => o.text).ToList();

            var depotName = db.tbl_P3_Master_Divisionwise_Product.GroupBy(x => new { x.DepotName })
               .Select(s => new
               {
                   id = s.Key.DepotName,
                   text = s.Key.DepotName,
               }).OrderBy(o => o.text).ToList();

            var products = db.tbl_P3_Master_Divisionwise_Product.AsEnumerable().GroupBy(x => new { x.ProductName })
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

        #endregion

    }
}
