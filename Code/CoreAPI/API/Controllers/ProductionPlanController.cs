﻿using API.BusinessLogic;
using API.Context;
using API.Entity;
using API.Helper;
using API.IdentityModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public ProductionPlanController(ILoggerFactory loggerFactory, DBContext db,
            IMapper mapper, UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment, IMemoryCache memoryCache) : base(loggerFactory)
        {
            this._mapper = mapper;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.memoryCache = memoryCache;
            excelReader = new ExcelReader();

            this.productionPlanLogic = new ProductionPlanLogic(db, mapper, userManager, memoryCache);
        }
        #endregion

        #region "FactoryProductionTarget"

        #region UploadExcel_FactoryProductionTarget
        [HttpPost]
        [SwaggerOperation(
                     Summary = "Production Plan",
                     Description = "Production Plan",
                     OperationId = "UploadExcel_FactoryProductionTarget",
                     Tags = new[] { "Production Plan" }
                 )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> UploadExcel_FactoryProductionTarget()
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
                bool b = fileHelper.createDirectory("ExcelUpload/FactoryProduction");
                path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/FactoryProduction/", fname);

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
                        column.ColumnName = "FinalUnits_QTY";
                    }
                }
                string dttojson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var  FactoryProductionTargetlist = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_FactoryProductionTarget>>(dttojson, settings);

                var month = Convert.ToInt32(parametars.Find(x => x.Key == "Month").Value.ToString());
                var year = Convert.ToInt32(parametars.Find(x => x.Key == "Year").Value.ToString());

                 FactoryProductionTargetlist.ForEach(x =>
                {
                    x.ForMonth = month;
                    x.ForYear = year;
                    x.MonthName = this.functions.MonthName(month - 1) + " " + year.ToString();
                });

                return Ok(new { success = 1, message = "Factory Production Target", data =  FactoryProductionTargetlist });
            }
            catch (Exception ex)
            {
                return Ok(new { success = 0, message = "Factory Production Target" });
            }
        }
        #endregion

        #region SaveExcel_FactoryProductionTarget
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

        public async Task<IActionResult> SaveExcel_FactoryProductionTarget([FromBody, SwaggerParameter("FactoryProductionTarget_Data", Required = true)] JObject body)
        {
            var user = await this.GetCurrentUser();
            dynamic jsonData = body;

            JArray factoryProductionTarget_Data = jsonData.FactoryProductionTarget_Data;

            Int32 year = Convert.ToInt32(jsonData.year);
            Int32 month = Convert.ToInt32(jsonData.month);

            var FactoryProductionTarget_Data = JsonConvert.DeserializeObject<List<ProductionPlan.ImportExcel_FactoryProductionTarget>>(factoryProductionTarget_Data.ToString());

            var result = await this.productionPlanLogic.SaveExcel_FactoryProductionTarget(user, year, month, FactoryProductionTarget_Data);
            return Ok(new { success = result, message = "" });
        }
        #endregion

        #region List Factory Production Target
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Production Plan",
                         Description = "Production Plan",
                         OperationId = "List_FactoryProductionTarget",
                         Tags = new[] { "List_FactoryProductionTarget" }
                     )]

        [SwaggerResponse(200, "Production Plan")]
        [SwaggerResponse(204, "Production Plan", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public IActionResult List_FactoryProductionTarget([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = this.productionPlanLogic.List_FactoryProductionTarget(Month, Year);

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
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year, string Product)
        {
            var records = this.productionPlanLogic.List_ProductionPlan_VolumeConversion(Month, Year);

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
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year, string Product)
        {
            var records = this.productionPlanLogic.List_ProductionPlan_VolumeCharge(Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Volume Charge", data = records });
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
          [FromQuery, SwaggerParameter("Year", Required = true)] int Year, string Product)
        {
            var records = this.productionPlanLogic.List_ProductionPlan_FinalChargeUnit(Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Volume Final Charge Unit", data = records });
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

        [HttpGet]
        public async Task<IActionResult> ProductionPlaning_DataProcessing(Int32 Year, Int32 Month, string SpToCall)
        {
            await this.productionPlanLogic.ProductionPlaning_DataProcessing(Year, Month, SpToCall);
            return Ok(new { success = true, message = "" });
        }

        #endregion
    }
}
