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

        #region -- Get Product Type Name --
        public static List<MasterSetupEntity.Product_Master_ProductTypeName> Get_ProductTypeName(String Connection)
        {
            List<Product_Master_ProductTypeName> mlist = new List<Product_Master_ProductTypeName>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "PRC_Get_ProductTypeName");
            foreach (DataRow DR in DT.Rows)
            {
                Product_Master_ProductTypeName obj = new Product_Master_ProductTypeName();
                obj.ProductTypeName = DR["ProductTypeName"].ToString();
                mlist.Add(obj);
            }
            return mlist;
        }
        #endregion

        #region -- Get ProductWise BatchSize --
        public static List<MasterSetupEntity.ProductTypeWise_CategoryName> Get_ProductTypeWise_Category(String Connection, String ProductTypeName)
        {
            List<ProductTypeWise_CategoryName> mlist = new List<ProductTypeWise_CategoryName>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "PRC_Get_CategoryName", ProductTypeName);
            foreach (DataRow DR in DT.Rows)
            {
                ProductTypeWise_CategoryName obj = new ProductTypeWise_CategoryName();
                obj.CategoryName = DR["CategoryName"].ToString();
                mlist.Add(obj);
            }
            return mlist;
        }
        #endregion

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

        #region -- List of Product Master --
        public static List<MasterSetupEntity.ProductMasterList> List_ProductMaster(String Connection, string ProductTypeName, string CategoryName)
        {
            List<ProductMasterList> mlist = new List<ProductMasterList>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "PRC_Get_ProductMaster_List", ProductTypeName, CategoryName);
            foreach (DataRow DR in DT.Rows)
            {
                ProductMasterList obj = new ProductMasterList();
                obj.PK_ProductID = clsHelper.fnConvert2Long(DR["PK_ProductID"]);
                obj.CategoryName = DR["CategoryName"].ToString();
                obj.ProductCode = DR["ProductCode"].ToString();
                obj.ProductName = DR["ProductName"].ToString();
                obj.ProductUOM = DR["ProductUOM"].ToString();
                obj.PackUnit = DR["PackUnit"].ToString();
                obj.ProductCategory = DR["ProductCategory"].ToString();
                obj.FactorValue = clsHelper.fnConvert3Decimal(DR["FactorValue"]);
                obj.BatchSize = clsHelper.fnConvert3Decimal(DR["BatchSize"]);
                obj.NRVRate = clsHelper.fnConvert3Decimal(DR["NRVRate"]);
                obj.NRVEffectiveRateFrom = DR["NRVEffectiveRateFrom"].ToString();
                mlist.Add(obj);
            }
            return mlist;
        }
        #endregion

        #region -- Add New Product Master --
        public static String AddNew_ProductMaster(String Connection, MasterSetupEntity.ProductMasterList product)
        {
            return clsDatabase.fnDBOperation(Connection, "PRC_Add_Product_Master", product.CategoryName,
                product.ProductCode, product.ProductName, product.ProductUOM, product.PackUnit, product.ProductCategory,
                product.FactorValue, product.BatchSize, product.NRVRate, product.NRVEffectiveRateFrom);
        }
        #endregion

        #region -- Update Product Master --
        public static String Update_ProductMaster(String Connection, MasterSetupEntity.ProductMasterList product)
        {
            return clsDatabase.fnDBOperation(Connection, "PRC_Update_Product_Master", product.PK_ProductID, product.CategoryName,
                product.ProductCode,product.ProductName,product.ProductUOM,product.PackUnit,product.ProductCategory,
                product.FactorValue,product.BatchSize,product.NRVRate,product.NRVEffectiveRateFrom);
        }

        #endregion

        #region -- Get by Product ID --
        public static List<MasterSetupEntity.ProductMasterList> GetByID_ProductMaster(String Connection, Int32 ID,String ProductCategory)
        {
            List<ProductMasterList> mlist = new List<ProductMasterList>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "PRC_Product_GetByID", ID, ProductCategory);
            foreach (DataRow DR in DT.Rows)
            {
                ProductMasterList obj = new ProductMasterList();
                obj.PK_ProductID = clsHelper.fnConvert2Long(DR["PK_ProductID"]);
                obj.CategoryName = DR["CategoryName"].ToString();
                obj.ProductCode = DR["ProductCode"].ToString();
                obj.ProductName = DR["ProductName"].ToString();
                obj.ProductUOM = DR["ProductUOM"].ToString();
                obj.PackUnit = DR["PackUnit"].ToString();
                obj.ProductCategory = DR["ProductCategory"].ToString();
                obj.FactorValue = clsHelper.fnConvert3Decimal(DR["FactorValue"]);
                obj.BatchSize = clsHelper.fnConvert3Decimal(DR["BatchSize"]);
                obj.NRVRate = clsHelper.fnConvert3Decimal(DR["NRVRate"]);
                obj.NRVEffectiveRateFrom = DR["NRVEffectiveRateFrom"].ToString();
                mlist.Add(obj);
            }
            return mlist;
        }
        #endregion

        #endregion

        #region -- BATCH SIZE MASTER -- 
        #region -- Add New BatchSize Master --
        public static String AddNew_BatchSizeMaster(String Connection, MasterSetupEntity.BatchSizeMasterList batchsize)
        {
            return clsDatabase.fnDBOperation(Connection, "PRC_Add_BatchSize_Master", 
                batchsize.ProductType, batchsize.ProductName,batchsize.UOM,batchsize.BatchSize);
        }
        #endregion

        #region -- List of Batch Size Master --
        public static List<MasterSetupEntity.BatchSizeMasterList> List_BatchSizeMaster(String Connection)
        {
            List<BatchSizeMasterList> mlist = new List<BatchSizeMasterList>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "PRC_Get_BatchSize_Master_List");
            foreach (DataRow DR in DT.Rows)
            {
                BatchSizeMasterList obj = new BatchSizeMasterList();
                obj.BatchSizeID = clsHelper.fnConvert2Long(DR["BatchSizeID"]);
                obj.ProductType = DR["ProductType"].ToString();
                obj.ProductName = DR["ProductName"].ToString();
                obj.UOM = DR["UOM"].ToString();
                obj.BatchSize = clsHelper.fnConvert3Decimal(DR["BatchSize"]);
                mlist.Add(obj);
            }
            return mlist;
        }
        #endregion

        #region -- Update Batch Size Master --
        public static String Update_BatchSizeMaster(String Connection, MasterSetupEntity.BatchSizeMasterList batchsize)
        {
            return clsDatabase.fnDBOperation(Connection, "PRC_Update_BatchSize_Master", batchsize.BatchSizeID, batchsize.ProductName, batchsize.UOM,
                batchsize.BatchSize);
        }

        #endregion

        #region -- Get by Batch SIze ID --
        public static List<MasterSetupEntity.BatchSizeMasterList> GetByID_BatchSizeMaster(String Connection, Int32 BatchSizeID)
        {
            List<BatchSizeMasterList> mlist = new List<BatchSizeMasterList>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "PRC_BatchSize_GetByID", BatchSizeID);
            foreach (DataRow DR in DT.Rows)
            {
                BatchSizeMasterList obj = new BatchSizeMasterList();
                obj.BatchSizeID = clsHelper.fnConvert2Long(DR["BatchSizeID"]);
                obj.ProductName = DR["ProductName"].ToString();
                obj.UOM = DR["UOM"].ToString();
                obj.ProductType = DR["ProductType"].ToString();
                obj.BatchSize = clsHelper.fnConvert3Decimal(DR["BatchSize"]);
                
                mlist.Add(obj);
            }
            return mlist;
        }
        #endregion

        #endregion

        #region -- Product Mapping Master --
        #region -- Update Product Mapping Master --
        public static String Update_ProductMappingMaster(String Connection, MasterSetupEntity.ProductMasterMapping product)
        {
            return clsDatabase.fnDBOperation(Connection, "usp_Update_Product_Mapping", product.TallyProductName, product.TallyUOM, product.PK_ProductID);
        }

        #endregion

        #region -- List of Product Mapping Master --
        public static List<MasterSetupEntity.ProductMasterMapping> List_ProductMappingMaster(String Connection, string Type)
        {
            List<ProductMasterMapping> mlist = new List<ProductMasterMapping>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "usp_Product_Mapping_ListByType", Type);
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
        #endregion

        #region -- Get by Product Mapping ID --
        public static List<MasterSetupEntity.ProductMasterMapping> GetByID_ProductMappingMaster(String Connection, Int32 Id)
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
