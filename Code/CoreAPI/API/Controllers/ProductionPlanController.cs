﻿using API.BusinessLogic;
using API.Context;
using API.Entity;
using API.Helper;
using API.IdentityModels;
using API.Models;
using AutoMapper;
using Common.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static API.Entity.ProductionPlan;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductionPlanController : ApiBase
    {
        #region "Declaration"
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private readonly IMapper _mapper;
        private ProductionPlanLogic productionPlanLogic;
        readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMemoryCache memoryCache;
        private readonly Functions functions = new Functions();
        private readonly ExcelReader excelReader;
        private readonly DBContext _context;
        public IConfiguration Configuration { get; set; }
        public String MSSQLConnection { get; set; }
        public String MendineMasterConnection { get; set; }

        public ProductionPlanController(ILoggerFactory loggerFactory, DBContext db,
            IMapper mapper, UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment, IMemoryCache memoryCache, IConfiguration configuration) : base(loggerFactory)
        {
            Configuration = configuration;
            MSSQLConnection = Configuration.GetConnectionString("MSSQLConnection").ToString();
            MendineMasterConnection = Configuration.GetConnectionString("MendineMasterConnection").ToString();
            this._mapper = mapper;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.memoryCache = memoryCache;
            excelReader = new ExcelReader();
            _context = db;

            this.productionPlanLogic = new ProductionPlanLogic(db, mapper, userManager, memoryCache);
        }
        #endregion

        #region "TallyProductBatch"

        #region UploadExcel_TallyProductBatch
        [HttpPost]
        [SwaggerOperation(
                     Summary = "Production Plan",
                     Description = "Production Plan",
                     OperationId = "UploadExcel_TallyProductBatch",
                     Tags = new[] { "Production Plan" }
                 )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> UploadExcel_TallyProductBatch()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                var parametars = Request.Form.Select(x => new { x.Key, x.Value }).ToList();
                string fname;
                fname = file.FileName;
                string path1 = string.Empty;
                FileHelper fileHelper = new FileHelper(this.webHostEnvironment);

                bool b1 = fileHelper.createDirectory("ExcelUpload");
                bool b = fileHelper.createDirectory("ExcelUpload/TallyProductBatch");
                path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/TallyProductBatch/", fname);

                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                string extension = Path.GetExtension(fname);
                if (fileHelper.checkFileExists(path1))
                {
                    fileHelper.deleteFile(path1);
                }
                using (FileStream fs = System.IO.File.Create(path1))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
                DataTable dt = excelReader.ExtractExcelSheetValuesToDataTable(path1, "");

                string dttojson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var FactoryTallyProductBatchlist = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_TallyProductBatch>>(dttojson, settings);

                return Ok(new { success = 1, message = "Tally Product Batch", data = FactoryTallyProductBatchlist });
            }
            catch (Exception ex)
            {
                return Ok(new { success = 0, message = "Tally Product Batch" });
            }
        }
        #endregion

        #region SaveExcel_TallyProductBatch
        [HttpPost]
        [SwaggerOperation(
                           Summary = "Production Plan",
                           Description = "Production Plan",
                           OperationId = "UpdateTallyProductBatch",
                           Tags = new[] { "Production Plan" }
                       )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult SaveExcel_TallyProductBatch([FromBody, SwaggerParameter("TallyProductBatch", Required = true)] JObject body)
        {
            dynamic jsonData = body;
            JArray tallyproductbatchList_Data = jsonData.TallyProductBatch;

            var tallyproductbatchList = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_TallyProductBatch>>(tallyproductbatchList_Data.ToString());

            var records = ProductionPlanLogic.SaveExcel_TallyProductBatch(MendineMasterConnection, tallyproductbatchList);
            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region List Tally Product Batch
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Production Plan",
                         Description = "Production Plan",
                         OperationId = "List_TallyProductBatch",
                         Tags = new[] { "List_TallyProductBatch" }
                     )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public IActionResult List_TallyProductBatch()
        {
            var records = ProductionPlanLogic.List_TallyProductBatch(MendineMasterConnection);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #endregion

        #region "FactoryClosingStock"
        #region UploadExcel_FactoryClosingStock
        [HttpPost]
        [SwaggerOperation(
                     Summary = "Production Plan",
                     Description = "Production Plan",
                     OperationId = "UploadExcel_FactoryClosingStock",
                     Tags = new[] { "Production Plan" }
                 )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> UploadExcel_FactoryClosingStock()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                var parametars = Request.Form.Select(x => new { x.Key, x.Value }).ToList();
                string fname;
                fname = file.FileName;
                string path1 = string.Empty;
                FileHelper fileHelper = new FileHelper(this.webHostEnvironment);

                bool b1 = fileHelper.createDirectory("ExcelUpload");
                bool b = fileHelper.createDirectory("ExcelUpload/FactoryClosingStock");
                path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/FactoryClosingStock/", fname);

                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                string extension = Path.GetExtension(fname);
                if (fileHelper.checkFileExists(path1))
                {
                    fileHelper.deleteFile(path1);
                }
                using (FileStream fs = System.IO.File.Create(path1))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
                DataTable dt = excelReader.ExtractExcelSheetValuesToDataTable(path1, "");

                string dttojson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var FactoryClosingStocklist = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_FactoryClosingStock>>(dttojson, settings);

                return Ok(new { success = 1, message = "Factory Closing Stock", data = FactoryClosingStocklist });
            }
            catch (Exception ex)
            {
                return Ok(new { success = 0, message = "Factory Closing Stock" });
            }
        }
        #endregion

        #region List Factory Closing Stock
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Production Plan",
                         Description = "Production Plan",
                         OperationId = "List_FactoryClosingStock",
                         Tags = new[] { "List_FactoryClosingStock" }
                     )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public IActionResult List_FactoryClosingStock([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = ProductionPlanLogic.List_FactoryClosingStock(MSSQLConnection,Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region SaveExcel_FactoryClosingStock
        [HttpPost]
        [SwaggerOperation(
                           Summary = "Production Plan",
                           Description = "Production Plan",
                           OperationId = "UpdateProjection",
                           Tags = new[] { "Production Plan" }
                       )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult SaveExcel_FactoryClosingStock([FromBody, SwaggerParameter("FactoryClosingStock", Required = true)] JObject body)
        {
            dynamic jsonData = body;
            JArray factoryclosingstockList_Data = jsonData.FactoryClosingStock;
            
            // Convert JArray To Datatable
            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(factoryProductionTarget_Data.ToString());
            // Convert Datatable to List 
            //List<factory_closing_stock> factoryclosingstockList = new List<factory_closing_stock>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    factory_closing_stock fcs = new factory_closing_stock();
            //    fcs.CompanyId = dt.Rows[i]["CompanyId"].ToString();
            //    fcs.Stock_date = dt.Rows[i]["Stock_date"].ToString();
            //    fcs.St_group = dt.Rows[i]["St_group"].ToString();
            //    fcs.St_category = dt.Rows[i]["St_category"].ToString();
            //    fcs.product_name = dt.Rows[i]["product_name"].ToString();
            //    fcs.quantity = clsHelper.fnConvert2Decimal(dt.Rows[i]["quantity"]);
            //    fcs.UOM = dt.Rows[i]["UOM"].ToString();
            //    fcs.rate = clsHelper.fnConvert2Decimal(dt.Rows[i]["rate"]);
            //    fcs.amount = clsHelper.fnConvert2Decimal(dt.Rows[i]["amount"]);
            //    factoryclosingstockList.Add(fcs);
            //}

            var factoryclosingstockList = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_FactoryClosingStock>>(factoryclosingstockList_Data.ToString());

            var records = ProductionPlanLogic.SaveExcel_FactoryClosingStock(MendineMasterConnection, factoryclosingstockList);
            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #endregion

        #region "DepotTransitStock"

        #region UploadExcel_DepotTransitStock
        [HttpPost]
        [SwaggerOperation(
                     Summary = "Production Plan",
                     Description = "Production Plan",
                     OperationId = "UploadExcel_DepotTransitStock",
                     Tags = new[] { "Production Plan" }
                 )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> UploadExcel_DepotTransitStock()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                var parametars = Request.Form.Select(x => new { x.Key, x.Value }).ToList();
                string fname;
                fname = file.FileName;
                string path1 = string.Empty;
                FileHelper fileHelper = new FileHelper(this.webHostEnvironment);

                bool b1 = fileHelper.createDirectory("ExcelUpload");
                bool b = fileHelper.createDirectory("ExcelUpload/DepotTransitStock");
                path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/DepotTransitStock/", fname);

                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                string extension = Path.GetExtension(fname);
                if (fileHelper.checkFileExists(path1))
                {
                    fileHelper.deleteFile(path1);
                }
                using (FileStream fs = System.IO.File.Create(path1))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
                DataTable dt = excelReader.ExtractExcelSheetValuesToDataTable(path1, "");

                string dttojson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var DepotTransitStocklist = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_DepotTransitStock>>(dttojson, settings);

                return Ok(new { success = 1, message = "Depot Transit Stock", data = DepotTransitStocklist });
            }
            catch (Exception ex)
            {
                return Ok(new { success = 0, message = "Depot Transit Stock" });
            }
        }
        #endregion

        #region SaveExcel_DepotTransitStock
        [HttpPost]
        [SwaggerOperation(
                           Summary = "Production Plan",
                           Description = "Production Plan",
                           OperationId = "SaveExcel_DepotTransitStock",
                           Tags = new[] { "Production Plan" }
                       )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult SaveExcel_DepotTransitStock([FromBody, SwaggerParameter("DepotTransitStock", Required = true)] JObject body)
        {
            dynamic jsonData = body;
            JArray depottransitstockList_Data = jsonData.DepotTransitStock;

            var depottransitstockList = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_DepotTransitStock>>(depottransitstockList_Data.ToString());

            var records = ProductionPlanLogic.SaveExcel_DepotTransitStock(MendineMasterConnection, depottransitstockList);
            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region List Depot Transit Stock
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Production Plan",
                         Description = "Production Plan",
                         OperationId = "List_DepotTransitStock",
                         Tags = new[] { "List_DepotTransitStock" }
                     )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public IActionResult List_DepotTransitStock([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = ProductionPlanLogic.List_DepotTransitStock(MendineMasterConnection, Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #endregion

        #region "DepotClosingStock"

        #region UploadExcel_DepotClosingStock
        [HttpPost]
        [SwaggerOperation(
                     Summary = "Production Plan",
                     Description = "Production Plan",
                     OperationId = "UploadExcel_DepotClosingStock",
                     Tags = new[] { "Production Plan" }
                 )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> UploadExcel_DepotClosingStock()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                var parametars = Request.Form.Select(x => new { x.Key, x.Value }).ToList();
                string fname;
                fname = file.FileName;
                string path1 = string.Empty;
                FileHelper fileHelper = new FileHelper(this.webHostEnvironment);

                bool b1 = fileHelper.createDirectory("ExcelUpload");
                bool b = fileHelper.createDirectory("ExcelUpload/DepotClosingStock");
                path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/DepotClosingStock/", fname);

                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                string extension = Path.GetExtension(fname);
                if (fileHelper.checkFileExists(path1))
                {
                    fileHelper.deleteFile(path1);
                }
                using (FileStream fs = System.IO.File.Create(path1))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
                DataTable dt = excelReader.ExtractExcelSheetValuesToDataTable(path1, "");

                string dttojson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var DepotClosingStocklist = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_DepotClosingStock>>(dttojson, settings);

                return Ok(new { success = 1, message = "Depot Closing Stock", data = DepotClosingStocklist });
            }
            catch (Exception ex)
            {
                return Ok(new { success = 0, message = "Depot Closing Stock" });
            }
        }
        #endregion

        #region SaveExcel_DepotClosingStock
        [HttpPost]
        [SwaggerOperation(
                           Summary = "Production Plan",
                           Description = "Production Plan",
                           OperationId = "SaveExcel_DepotClosingStock",
                           Tags = new[] { "Production Plan" }
                       )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult SaveExcel_DepotClosingStock([FromBody, SwaggerParameter("DepotClosingStock", Required = true)] JObject body)
        {
            dynamic jsonData = body;
            JArray depotclosingstockList_Data = jsonData.DepotClosingStock;

            var depotclosingstockList = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_DepotClosingStock>>(depotclosingstockList_Data.ToString());

            var records = ProductionPlanLogic.SaveExcel_DepotClosingStock(MendineMasterConnection, depotclosingstockList);
            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region List Depot Closing Stock
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Production Plan",
                         Description = "Production Plan",
                         OperationId = "List_DepotClosingStock",
                         Tags = new[] { "List_DepotClosingStock" }
                     )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public IActionResult List_DepotClosingStock([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = ProductionPlanLogic.List_DepotClosingStock(MendineMasterConnection, Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #endregion


        #region "Physician Sample Forecasting"

        #region UploadExcel_PhysicianSampleForecast
        [HttpPost]
        [SwaggerOperation(
                     Summary = "Production Plan",
                     Description = "Production Plan",
                     OperationId = "UploadExcel_PhysicianSampleForecast",
                     Tags = new[] { "Production Plan" }
                 )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> UploadExcel_PhysicianSampleForecast()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                var parametars = Request.Form.Select(x => new { x.Key, x.Value }).ToList();
                string fname;
                fname = file.FileName;
                string path1 = string.Empty;
                FileHelper fileHelper = new FileHelper(this.webHostEnvironment);

                bool b1 = fileHelper.createDirectory("ExcelUpload");
                bool b = fileHelper.createDirectory("ExcelUpload/PhysicianSampleForecast");
                path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/PhysicianSampleForecast/", fname);

                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                string extension = Path.GetExtension(fname);
                if (fileHelper.checkFileExists(path1))
                {
                    fileHelper.deleteFile(path1);
                }
                using (FileStream fs = System.IO.File.Create(path1))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
                DataTable dt = excelReader.ExtractExcelSheetValuesToDataTable(path1, "");

                string dttojson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var PhysicianSampleForecastlist = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_PhysicianSampleForecast>>(dttojson, settings);

                

                return Ok(new { success = 1, message = "Physician Sample Forecast", data = PhysicianSampleForecastlist });
            }
            catch (Exception ex)
            {
                return Ok(new { success = 0, message = "Physician Sample Forecast" });
            }
        }
        #endregion

        #region SaveExcel_PhysicianSampleForecasting
        [HttpPost]
        [SwaggerOperation(
                           Summary = "Production Plan",
                           Description = "Production Plan",
                           OperationId = "SaveExcel_PhysicianSampleForecasting",
                           Tags = new[] { "Production Plan" }
                       )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult SaveExcel_PhysicianSampleForecasting([FromBody, SwaggerParameter("PhysicianSampleForecasting", Required = true)] JObject body)
        {
            dynamic jsonData = body;
            JArray physiciansampleforecastingList_Data = jsonData.PhysicianSampleForecasting;

            var physiciansampleforecastingList = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_PhysicianSampleForecast>>(physiciansampleforecastingList_Data.ToString());

            var records = ProductionPlanLogic.SaveExcel_PhysicianSampleForecasting(MendineMasterConnection, physiciansampleforecastingList);
            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region List Physician Sample Forecasting
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Production Plan",
                         Description = "Production Plan",
                         OperationId = "List_PhysicianSampleForecast",
                         Tags = new[] { "List_PhysicianSampleForecast" }
                     )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public IActionResult List_PhysicianSampleForecast([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = ProductionPlanLogic.List_PhysicianSampleForecast(MendineMasterConnection, Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return Ok(new { success = 1, message = "Production Plan", data = records });
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #endregion



        #region "Physician Sample Plan"

        #region UploadExcel_PhysicianSamplePlan
        [HttpPost]
        [SwaggerOperation(
                     Summary = "Physician Sample Plan",
                     Description = "Physician Sample Plan",
                     OperationId = "UploadExcel_PhysicianSamplePlan",
                     Tags = new[] { "Physician Sample Plan" }
                 )]

        [SwaggerResponse(200, "Physician Sample Plan")]
        [SwaggerResponse(204, "Physician Sample Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> UploadExcel_PhysicianSamplePlan()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];

                var parametars = Request.Form.Select(x => new { x.Key, x.Value }).ToList();

                string fname;
                fname = file.FileName;
                string path1 = string.Empty;
                FileHelper fileHelper = new FileHelper(this.webHostEnvironment);

                bool b1 = fileHelper.createDirectory("ExcelUpload");
                bool b = fileHelper.createDirectory("ExcelUpload/PhysicianSample");
                path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/PhysicianSample/", fname);

                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                string extension = Path.GetExtension(fname);
                if (fileHelper.checkFileExists(path1))
                {
                    fileHelper.deleteFile(path1);
                }
                using (FileStream fs = System.IO.File.Create(path1))
                {
                    await file.CopyToAsync(fs);
                    fs.Flush();
                }
                DataTable dt = excelReader.ExtractExcelSheetValuesToDataTable(path1, "");

                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName.Contains("UnitsQTY") == true)
                    {
                        column.ColumnName = "PhysicianSampleQTY";
                    }
                }
                string dttojson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var PhysicianSamplePlanlist = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_PhysicianSamplePlan>>(dttojson, settings);

                var month =Convert.ToInt32(parametars.Find(x => x.Key == "Month").Value.ToString());
                var year =Convert.ToInt32(parametars.Find(x => x.Key == "Year").Value.ToString());

                PhysicianSamplePlanlist.ForEach(x =>
                {
                    x.ForMonth = month;
                    x.ForYear = year;
                    x.MonthName = this.functions.MonthName(month-1) + " " + year.ToString();
                });

                return Ok(new { success = 1, message = "Physician Sample Plan", data = PhysicianSamplePlanlist });
            }
            catch (Exception ex)
            {
                return Ok(new { success = 0, message = "Physician Sample Plan" });
            }
        }
        #endregion

        #region SaveExcel_PhysicianSamplePlan
        [HttpPost]
        [SwaggerOperation(
                           Summary = "Physician Sample Plan",
                           Description = "Physician Sample Plan",
                           OperationId = "UpdateProjection",
                           Tags = new[] { "Physician Sample Plan" }
                       )]

        [SwaggerResponse(200, "Physician Sample Plan")]
        [SwaggerResponse(204, "Physician Sample Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> SaveExcel_PhysicianSamplePlan([FromBody, SwaggerParameter("PhysicianSamplePlan_Data", Required = true)] JObject body)
        {
            var user = await this.GetCurrentUser();
            dynamic jsonData = body;

            JArray sampledata = jsonData.sampledata;

            Int32 year =Convert.ToInt32(jsonData.year);
            Int32 month = Convert.ToInt32(jsonData.month);

            var physicianSamplePlan_Data = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_PhysicianSamplePlan>>(sampledata.ToString());

          var result=  await this.productionPlanLogic.SaveExcel_PhysicianSamplePlan(user,year,month,physicianSamplePlan_Data);
            return Ok(new { success = result, message = "" });

        }
        #endregion

        #region List Physician Sample Plan
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Physician Sample Plan",
                         Description = "Physician Sample Plan",
                         OperationId = "List_PhysicianSamplePlan",
                         Tags = new[] { "List_PhysicianSamplePlan" }
                     )]

        [SwaggerResponse(200, "Physician Sample Plan")]
        [SwaggerResponse(204, "Physician Sample Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public IActionResult List_PhysicianSamplePlan([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = this.productionPlanLogic.List_PhysicianSamplePlan(Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Physician Sample Plan", data = records });
            }
            else if (records.Count() <= 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #endregion

        #region Production Plan

        // Pallab
        #region -- Production Planning Data Processing --
        [HttpGet]
        public async Task<IActionResult> ProductionPlaning_DataProcessing(Int32 Year, Int32 Month, string SpToCall)
        {
            int status = await this.productionPlanLogic.ProductionPlaning_DataProcessing(Year, Month, SpToCall);
            if(status >0 )
                return Ok(new { success = true, message = "" });
            else
                return Ok(new { success = false, message = "" });
        }
        #endregion

        #region -- Update Sync Production Plan Task  --
        [HttpPost]

        [SwaggerOperation(
                          Summary = "Update Sync ProductionPlan Task",
                          Description = "Update Sync ProductionPlan Task",
                          OperationId = "UpdateSyncProductionPlanTask",
                          Tags = new[] { "Update Sync ProductionPlan Task" }
                      )]
        [SwaggerResponse(201, "Sync ProductionPlan Task Updated", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult Update_SyncProductionPlanTask()
        {
            var result = ProductionPlanLogic.Update_Sync_Production_Plan_Task(MSSQLConnection);

            if (result  =="Success")
            {
                return Ok(new { success = 1, message = "ProductionPlan Task Updated successfully." });
            }
            else
            {
                return BadRequest(new { success = 0, message = "ProductionPlan Task not update due to internal error." });
            }
        }
        #endregion

        #region -- Get ProductName --
        [HttpGet]

        [SwaggerOperation(
                         Summary = "Get ProductName",
                         Description = "Get ProductName",
                         OperationId = "GetProductName",
                         Tags = new[] { "ProductName" }
                     )]
        [SwaggerResponse(201, "ProductName found", typeof(ProductionPlan_ProductName))]
        [SwaggerResponse(204, "ProductName not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult Get_ProductName()
        {
            var ProductName = ProductionPlanLogic.Get_ProductName(MSSQLConnection);

            if (ProductName.Count > 0)
            {
                return Ok(new { success = 1, message = "ProductName found", data = ProductName });
            }
            else if (ProductName.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region -- Get ProductWise BatchSize --
        [HttpGet]

        [SwaggerOperation(
                         Summary = "Get ProductWise BatchSize",
                         Description = "Get ProductWise BatchSize",
                         OperationId = "GetProductWiseBatchSize",
                         Tags = new[] { "ProductWise BatchSize" }
                     )]
        [SwaggerResponse(201, "ProductWise BatchSize found", typeof(ProductWise_BatchSize))]
        [SwaggerResponse(204, "ProductWise BatchSize not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult Get_ProductWise_BatchSize([FromQuery, SwaggerParameter("ProductName", Required = true)] String ProductName)
        {
            var BatchSize = ProductionPlanLogic.Get_ProductWise_BatchSize(MSSQLConnection, ProductName);

            if (BatchSize.Count > 0)
            {
                return Ok(new { success = 1, message = "BatchSize found", data = BatchSize });
            }
            else if (BatchSize.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region -- List of Chargeable Batch Product --
        [HttpGet]
        [SwaggerOperation(
                          Summary = "Get List of Chargeable Batch Product",
                          Description = "Get List of Chargeable Batch Product",
                          OperationId = "ListChargeableBatch",
                          Tags = new[] { "Chargeable Batch" }
                      )]

        [SwaggerResponse(200, "Chargeable Batch Product found"/*,typeof(IEnumerable<Vendor>)*/)]
        [SwaggerResponse(204, "Chargeable Batch Product not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult List_ChargeableBatchProduct(Int32 Month, Int32 Year, String ProductName, String BatchSize)
        {
            var lstData = ProductionPlanLogic.List_ChargeableBatchProduct(MSSQLConnection, Month, Year, ProductName, BatchSize);

            if (lstData != null && lstData.Count() > 0)
            {
                return Ok(new { success = 1, message = "ChargeableBatchProduct list", data = lstData });
            }
            else if (lstData == null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region -- Get Batch Wise Unit Factor By SL NO --
        [HttpGet]

        [SwaggerOperation(
                         Summary = "Get Batch Wise Unit Factor By SL NO",
                         Description = "Get Batch Wise Unit Factor By SL NO",
                         OperationId = "GetBatchWiseUnitFactorBySLNO",
                         Tags = new[] { "Batch Wise Unit Factor By SLNO" }
                     )]
        [SwaggerResponse(201, "Batch Wise Unit Factor found")]
        [SwaggerResponse(204, "Batch Wise Unit Factor not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult GetByID_BatchWiseUnitFactor([FromQuery, SwaggerParameter("SL NO", Required = true)] Int32 SLNO)
        {
            var BatchWiseUnitFactor = ProductionPlanLogic.GetByID_BatchWiseUnitFactor(MSSQLConnection, SLNO);

            if (BatchWiseUnitFactor.Count > 0)
            {
                return Ok(new { success = 1, message = "Batch Wise Unit Factor found", data = BatchWiseUnitFactor });
            }
            else if (BatchWiseUnitFactor.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region -- Update Batch Wise Unit Factor By SL NO --
        [HttpPost]

        [SwaggerOperation(
                   Summary = "Update Batch Wise Unit Factor By SL NO",
                   Description = "Update Batch Wise Unit Factor By SL NO",
                   OperationId = "UpdateBatchWiseUnitFactorBySLNO",
                   Tags = new[] { "Update Batch Wise Unit Factor By SL NO" }
               )]
        [SwaggerResponse(201, "Batch Wise Unit Factor Updated", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult Update_BatchWiseUnitFactorBySLNO([FromBody, SwaggerParameter("Update Batch Wise Unit Factor By SL NO", Required = true)] ProductionPlan_ChargeableBatchList ChargeableBatchID)
        {
            ProductionPlanLogic.Update_BatchWiseUnitFactorBySLNO(MSSQLConnection, ChargeableBatchID);
            return Ok(new { data = "Batch Wise User Unit Factor Data Updated Successfully !!!!!" });
        }
        #endregion

        #region -- Production Frecasting Volume Conversion, Volume Charge & Final Charge
        #region List Volume Conversion
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Volume Conversion",
                         Description = "Volume Conversion",
                         OperationId = "List_PhysicianSamplePlan",
                         Tags = new[] { "List_VolumeConversion" }
                     )]

        [SwaggerResponse(200, "Volume Conversion")]
        [SwaggerResponse(204, "Volume Conversion", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public IActionResult List_ProductionPlan_VolumeConversion([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = ProductionPlanLogic.List_ProductionPlan_VolumeConversion(MSSQLConnection, Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Volume Conversion", data = records });
            }
            else if (records.Count() <= 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region List Volume Charge
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Volume Charge",
                         Description = "Volume Charge",
                         OperationId = "List_ProductionPlan_VolumeCharge",
                         Tags = new[] { "List_VolumeCharge" }
                     )]

        [SwaggerResponse(200, "Volume Charge")]
        [SwaggerResponse(204, "Volume Charge", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public IActionResult List_ProductionPlan_VolumeCharge([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = ProductionPlanLogic.List_ProductionPlan_VolumeCharge(MSSQLConnection, Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Volume Charge", data = records });
            }
            else if (records.Count() <= 0)
            {
                return Ok(new { success = 0, message = "No Data In Volume Charge", data = records });
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region List Volume Final Charge Unit
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Volume Final Charge Unit",
                         Description = "Volume Final Charge Unit",
                         OperationId = "List_ProductionPlan_FinalChargeUnit",
                         Tags = new[] { "List_ProductionPlan_FinalChargeUnit" }
                     )]

        [SwaggerResponse(200, "Volume Final Charge Unit")]
        [SwaggerResponse(204, "Volume Final Charge Unit", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType

        public IActionResult List_ProductionPlan_FinalChargeUnit([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = ProductionPlanLogic.List_ProductionPlan_FinalChargeUnit(MSSQLConnection, Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Final Charge Unit", data = records });
            }
            else if (records.Count() <= 0)
            {
                return Ok(new { success = 0, message = "No Data In Final Charge Unit", data = records });
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #endregion

        #endregion
    }
}
