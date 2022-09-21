using API.BusinessLogic;
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
    public class SalesForecastController : ApiBase
    {
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private readonly IMapper _mapper;
        private SalesForecastLogic salesForecastLogic;
        readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMemoryCache memoryCache;
        private readonly Functions functions = new Functions();
        private readonly ExcelReader excelReader;
        public SalesForecastController(ILoggerFactory loggerFactory, DBContext db,
            IMapper mapper, UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment, IMemoryCache memoryCache) : base(loggerFactory)
        {
            this._mapper = mapper;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            this.memoryCache = memoryCache;
            excelReader = new ExcelReader();

            this.salesForecastLogic = new SalesForecastLogic(db, mapper, userManager, memoryCache);
        }

        [HttpGet]
        [SwaggerOperation(
                           Summary = "Sales Forecast Summary",
                           Description = "Sales Forecast Summary",
                           OperationId = "SalesForecastSummary",
                           Tags = new[] { "SalesForecast" }
                       )]

        [SwaggerResponse(200, "Sales Forecast Summary")]
        [SwaggerResponse(204, "Sales Forecast Summary", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult SalesForecastSummary([FromQuery, SwaggerParameter("Month", Required = true)] int Month, [FromQuery, SwaggerParameter("Year", Required = true)] int Year)
        {
            var records = this.salesForecastLogic.SalesForecastSummary(Month, Year);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Sales Forecast Summary", data = records });
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

        [HttpGet]
        // [ResponseCache(Duration =3600,Location =ResponseCacheLocation.Any, VaryByHeader = "User-Agent",NoStore =false)]

        [SwaggerOperation(
                           Summary = "Sales Forecasting",
                           Description = "Sales Forecasting",
                           OperationId = "SalesForecasting",
                           Tags = new[] { "SalesForecast" }
                       )]
        [SwaggerResponse(200, "Sales Forecasting")]
        [SwaggerResponse(204, "Sales Forecasting", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> SalesForecasting([FromQuery, SwaggerParameter("Month", Required = true)] int Month, [FromQuery, SwaggerParameter("Year", Required = true)] int Year
            , string Division, string Depot, string Product)

        {

            var records = await this.salesForecastLogic.SalesForecasting(Month, Year, Division, Depot, Product);

            if (records != null)
            {
                return Ok(new { success = 1, message = "Sales Forecasting", data = records });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet]
        [SwaggerOperation(
                           Summary = "Sales Forecasting",
                           Description = "Sales Forecasting",
                           OperationId = "ListImportProjection",
                           Tags = new[] { "SalesForecast" }
                       )]

        [SwaggerResponse(200, "Sales Forecasting")]
        [SwaggerResponse(204, "Sales Forecasting", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public async Task<IActionResult> ListImportProjection([FromQuery, SwaggerParameter("Month", Required = true)] int Month,
            [FromQuery, SwaggerParameter("Year", Required = true)] int Year, [FromQuery, SwaggerParameter("ForecastingType", Required = true)] string ForecastingType)
        {
            var records =this.salesForecastLogic.ListImportProjection(Month, Year, ForecastingType);

            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Sales Forecasting", data = records });
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

        [HttpPost]
        [SwaggerOperation(
                           Summary = "Sales Forecasting",
                           Description = "Sales Forecasting",
                           OperationId = "UploadProjection",
                           Tags = new[] { "SalesForecast" }
                       )]

        [SwaggerResponse(200, "Sales Forecasting")]
        [SwaggerResponse(204, "Sales Forecasting", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> UploadProjection()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                string fname;
                fname = file.FileName;
                string path1 = string.Empty;
                FileHelper fileHelper = new FileHelper(this.webHostEnvironment);
                //bool b = fileHelper.createDirectory("Uploads");
                //path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/SalesProjection/", fname);


                bool b1 = fileHelper.createDirectory("ExcelUpload");
                bool b = fileHelper.createDirectory("ExcelUpload/SalesProjection");
                path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/SalesProjection", fname);

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
                    if (column.ColumnName.Contains("ProjectedQTYFor") == true)
                    {
                        //column.ColumnName = "ProjectedTotalSalesQTY";
                        column.ColumnName = "ProjectedTotalSalesQTY";
                    }
                }
                string dttojson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var projectionlist = JsonConvert.DeserializeObject<List<SalesForecastEntity.ImportProjection>>(dttojson, settings);
                return Ok(new { success = 1, message = "Sales Projection", data = projectionlist });
            }
            catch (Exception ex)
            {
                return Ok(new { success = 0, message = "Sales Projection" });
            }
        }


        [HttpPost]
        [SwaggerOperation(
                           Summary = "Sales Forecasting",
                           Description = "Sales Forecasting",
                           OperationId = "UpdateProjection",
                           Tags = new[] { "SalesForecast" }
                       )]

        [SwaggerResponse(200, "Sales Forecasting")]
        [SwaggerResponse(204, "Sales Forecasting", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> UpdateProjection([FromBody, SwaggerParameter("ProjectionData", Required = true)] JObject body)
        {
            dynamic jsonData = body;
            JArray ProjectionData = jsonData.ProjectionData;
            var projectiondata = JsonConvert.DeserializeObject<List<SalesForecastEntity.ImportProjection>>(ProjectionData.ToString());
            await this.salesForecastLogic.UpdateProjection(projectiondata);
            return Ok(new { success = 1, message = "Uploaded Projection", data = "Sales Projection Excel Data Saved Successfully!!!!" });
            //return Ok();
        }


        [HttpGet]
        [SwaggerOperation(
                          Summary = "Sales Forecasting",
                          Description = "Sales Forecasting",
                          OperationId = "ForcastingDetailsSearchFields",
                          Tags = new[] { "SalesForecast" }
                      )]

        [SwaggerResponse(200, "Sales Forecasting")]
        [SwaggerResponse(204, "Sales Forecasting", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> ForcastingDetailsSearchFields()
        {

            var records = await this.salesForecastLogic.ForcastingDetailsSearchFields();

            if (records != null && records.Length > 0)
            {
                return Ok(new { success = 1, message = "Sales Forecast Summary", data = new {  divisions = records[0], depotnames = records[1] , products = records[2] } });
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

        #region Sales Forecasting Comparison 
        #region Search Fields
        [HttpGet]
        [SwaggerOperation(
                        Summary = "Sales Forecasting Comparison ",
                        Description = "Sales Forecasting Comparison ",
                        OperationId = "UpdateProjection",
                        Tags = new[] { "SalesForecast" }
                    )]

        [SwaggerResponse(200, "Sales Forecasting Comparison ")]
        [SwaggerResponse(204, "Sales Forecasting Comparison ", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> ForcastingComparison_SearchFields()
        {
            var records = await this.salesForecastLogic.ForcastingComparison_SearchFields();
            if (records != null && records.Length > 0)
            {
                return Ok(new { success = 1, message = "Sales Forecast Comparison", data = new { products = records[0], divisions = records[1], stocklocations = records[2], packunits = records[3] } });
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

        #region Forecasting Comparison  Summary
        [HttpGet]
        // [ResponseCache(Duration =3600,Location =ResponseCacheLocation.Any, VaryByHeader = "User-Agent",NoStore =false)]
        [SwaggerOperation(
                        Summary = "Sales Forecasting Comparison Summary",
                        Description = "Sales Forecasting Comparison Summary",
                        OperationId = "SalesForecastingComparison Summary",
                        Tags = new[] { "SalesForecastComparison Summary" }
                    )]
        [SwaggerResponse(200, "Sales Forecasting Comparison")]
        [SwaggerResponse(204, "Sales Forecasting Comparison", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> SalesForecastingComparison_Summary([FromQuery, SwaggerParameter("Month", Required = true)] int Month, [FromQuery, SwaggerParameter("Year", Required = true)] int Year
         , string Product, string Division, string Location, string PackUint, bool ReInitializeCache = true)
        {
            var records = await this.salesForecastLogic.SalesForecastingComparison_Summary(Month, Year, Product, Division, Location, PackUint,  ReInitializeCache);
            if (records != null)
            {
                return Ok(new { success = 1, message = records.Item2, data = records.Item1 });
            }
            else
            {
                return NoContent();
            }
        }
        #endregion

        #region Forecasting Comparison 
        [HttpGet]
        // [ResponseCache(Duration =3600,Location =ResponseCacheLocation.Any, VaryByHeader = "User-Agent",NoStore =false)]
        [SwaggerOperation(
                        Summary = "Sales Forecasting Comparison",
                        Description = "Sales Forecasting Comparison",
                        OperationId = "SalesForecastingComparison",
                        Tags = new[] { "SalesForecastComparison" }
                    )]
        [SwaggerResponse(200, "Sales Forecasting Comparison")]
        [SwaggerResponse(204, "Sales Forecasting Comparison", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult SalesForecastingComparison([FromQuery, SwaggerParameter("Month", Required = true)] int Month, [FromQuery, SwaggerParameter("Year", Required = true)] int Year
         , string Product, string Division, string Location, string PackUnit, bool ReInitializeCache = false)

        {
            var records =  this.salesForecastLogic.SalesForecastingComparison(Month, Year, Product, Division, Location, PackUnit, ReInitializeCache);

            if (records != null)
            {
                return Ok(new { success = 1, message = records.Item2, data = records.Item1 });
            }
            else
            {
                return NoContent();
            }
        }
        #endregion

        #region SAVE - Forecasting Comparison 
        [HttpPost]
        // [ResponseCache(Duration =3600,Location =ResponseCacheLocation.Any, VaryByHeader = "User-Agent",NoStore =false)]
        [SwaggerOperation(
                        Summary = "Sales Forecasting Comparison",
                        Description = "Sales Forecasting Comparison",
                        OperationId = "SalesForecastingComparison",
                        Tags = new[] { "SalesForecastComparison" }
                    )]
        [SwaggerResponse(200, "Sales Forecasting Comparison")]
        [SwaggerResponse(204, "Sales Forecasting Comparison", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> SalesForecastingComparisonSave(SalesForecastEntity.ForecastingComparison forecastingComparison)
        {
            var result = await this.salesForecastLogic.SalesForecastingComparisonSave(forecastingComparison);
            return Ok(new { success = 1, message = "Sales Forecasting Comparison", data = result });
        }
        #endregion

        #endregion

        #region Primary Sales

        #region List Primary Sales
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Primary Sales",
                         Description = "Primary Sales",
                         OperationId = "List_PrimarySales",
                         Tags = new[] { "SalesForecast" }
                     )]

        [SwaggerResponse(200, "Primary Sales")]
        [SwaggerResponse(204, "Primary Sales", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public async Task<IActionResult> List_PrimarySales([FromQuery, SwaggerParameter("FromDate date format DD/MM/YYYY", Required = true)] string FromDate,
           [FromQuery, SwaggerParameter("ToDate date format DD/MM/YYYY", Required = true)] string ToDate)
        {
            //var records = await this.salesForecastLogic.List_PrimarySales(FromDate, ToDate);
            var records = this.salesForecastLogic.List_PrimarySales(FromDate, ToDate);
            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Primary Sales", data = records });
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

        #region Secondary Sales

        #region List Secondary Sales
        [HttpGet]
        [SwaggerOperation(
                         Summary = "Secondary Sales",
                         Description = "Secondary Sales",
                         OperationId = "List_SecondarySales",
                         Tags = new[] { "SalesForecast" }
                     )]

        [SwaggerResponse(200, "Secondary Sales")]
        [SwaggerResponse(204, "Secondary Sales", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public async Task<IActionResult> List_SecondarySales([FromQuery, SwaggerParameter("Year", Required = true)] int Year, 
            [FromQuery, SwaggerParameter("Month", Required = true)] int Month)
        {
            //var records = await this.salesForecastLogic.List_SecondarySales( Year, Month);
            var records = this.salesForecastLogic.List_SecondarySales(Year, Month);
            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Secondary Sales", data = records });
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

        #region Upload Secondary Data
        [HttpPost]
        [SwaggerOperation(
                         Summary = "Import Secondary Sales",
                         Description = "Import Secondary Sales",
                         OperationId = "Upload_SecondarySales",
                         Tags = new[] { "SalesForecast" }
                     )]

        [SwaggerResponse(200, "Import Secondary Sales")]
        [SwaggerResponse(204, "Import Secondary Sales", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> Upload_SecondarySales()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                string fname;
                fname = file.FileName;
                string path1 = string.Empty;
                FileHelper fileHelper = new FileHelper(this.webHostEnvironment);
                bool b1 = fileHelper.createDirectory("ExcelUpload");
                bool b = fileHelper.createDirectory("ExcelUpload/SecondarySales");
                path1 = string.Format("{0}/{1}", this.webHostEnvironment.ContentRootPath + "/ExcelUpload/SecondarySales/", fname);

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
                //foreach (DataColumn column in dt.Columns)
                //{
                //    if (column.ColumnName.Contains("ProjectedQTYFor") == true)
                //    {
                //        //column.ColumnName = "ProjectedTotalSalesQTY";
                //        column.ColumnName = "ProjectedTotalSalesQTY";
                //    }
                //}
                string dttojson = JsonConvert.SerializeObject(dt, Formatting.Indented);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                var salesData = JsonConvert.DeserializeObject<List<SalesForecastEntity.Import_SecondarySales>>(dttojson, settings);
                return Ok(new { success = 1, message = "Import Secondary Sales", data = salesData });
            }
            catch (Exception ex)
            {
                return Ok(new { success = 0, message = "Import Secondary Sales" });
            }
        }
        #endregion

        #region Save Secondary Sales
        [HttpPost]
        [SwaggerOperation(
                           Summary = "Secondary Sales",
                           Description = "Secondary Sales",
                           OperationId = "Update_SecondarySales",
                           Tags = new[] { "SecondarySales" }
                       )]

        [SwaggerResponse(200, "Sales Forecasting")]
        [SwaggerResponse(204, "Sales Forecasting", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> SaveExcel_SecondarySales([FromBody, SwaggerParameter("SecondarySales", Required = true)] JObject body)
        {

            dynamic jsonData = body;
            JArray SecondarySales = jsonData.SecondarySales;
            Int32 Year = Convert.ToInt32(jsonData.year);
            Int32 Month = Convert.ToInt32(jsonData.month);
            var secondarySales = JsonConvert.DeserializeObject<List<SalesForecastEntity.Import_SecondarySales>>(SecondarySales.ToString());
            bool bresult = false;
            bresult = await this.salesForecastLogic.SaveExcel_SecondarySales(Year, Month, secondarySales);

            //return Ok(new { data = "Secondary Sales Data Uploaded Successfully !!!!!" });
            return Ok(new { success = 1,  data = "Secondary Sales Data Uploaded Successfully !!!!!" });

        }
        #endregion

        #endregion

      

        #region Sales Team Projection 
        #region Search Fields
        [HttpGet]
        [SwaggerOperation(
                        Summary = "Sales Team Projection ",
                        Description = "Sales Team Projection ",
                        OperationId = "UpdateProjection",
                        Tags = new[] { "SalesForecast" }
                    )]

        [SwaggerResponse(200, "Sales Team Projection ")]
        [SwaggerResponse(204, "Sales Team Projection ", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> SalesTeamProjection_SearchFields()
        {
            var records = await this.salesForecastLogic.SalesTeamProjection_SearchFields();
            if (records != null && records.Length > 0)
            {
                return Ok(new { success = 1, message = "Sales Team Projection", data = new { divisions = records[0], depots = records[1], products = records[2], packunits = records[3] } });
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

        #region Sales Team Projection

        #region Show Sales Team Projection
        [HttpGet]
        // [ResponseCache(Duration =3600,Location =ResponseCacheLocation.Any, VaryByHeader = "User-Agent",NoStore =false)]
        [SwaggerOperation(
                        Summary = "Sales Team Projection",
                        Description = "Sales Team Projection",
                        OperationId = "SalesForecastingComparison",
                        Tags = new[] { "SalesForecastComparison" }
                    )]
        [SwaggerResponse(200, "Sales Team Projection")]
        [SwaggerResponse(204, "Sales Team Projection", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult SalesTeamProjection_Show([FromQuery, SwaggerParameter("Month", Required = true)] int Month, [FromQuery, SwaggerParameter("Year", Required = true)] int Year
         , string Division, string DepotName, string Product, string PackUnit, bool IsManual=false, bool ReInitializeCache = false)
        {
            var records = this.salesForecastLogic.SalesTeamProjection_Show(Month, Year, Division, DepotName, Product, PackUnit, IsManual, ReInitializeCache);

            if (records != null)
            {
                return Ok(new { success = 1, message = records.Item2, data = records.Item1 });
            }
            else
            {
                return NoContent();
            }
        }
        #endregion

        #region Update - SalesTeamProjection
        [HttpPost]
        // [ResponseCache(Duration =3600,Location =ResponseCacheLocation.Any, VaryByHeader = "User-Agent",NoStore =false)]
        [SwaggerOperation(
                        Summary = "Sales Team Projection",
                        Description = "Sales Team Projection",
                        OperationId = "SalesForecastingComparison",
                        Tags = new[] { "SalesForecastComparison" }
                    )]
        [SwaggerResponse(200, "Sales Team Projection")]
        [SwaggerResponse(204, "Sales Team Projection", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult>SalesTeamProjection_Save(SalesForecastEntity.SaleProjection_SalesTeam saleProjection_SalesTeam)
        {
            var result = await this.salesForecastLogic.SalesTeamProjection_Save(saleProjection_SalesTeam);
            return Ok(new { success = 1, message = "Sales Team Projection", data = result });
        }
        #endregion

        #region Add New  - SalesTeamProjection
        [HttpPost]
        // [ResponseCache(Duration =3600,Location =ResponseCacheLocation.Any, VaryByHeader = "User-Agent",NoStore =false)]
        [SwaggerOperation(
                        Summary = "Sales Team Projection",
                        Description = "Sales Team Projection",
                        OperationId = "SalesForecastingComparison",
                        Tags = new[] { "SalesForecastComparison" }
                    )]
        [SwaggerResponse(200, "Sales Team Projection")]
        [SwaggerResponse(204, "Sales Team Projection", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> AddNew_SalesTeamProjection(SalesForecastEntity.SaleProjection_SalesTeam saleProjection_SalesTeam)
        {
            var result = await this.salesForecastLogic.AddNew_SalesTeamProjection(saleProjection_SalesTeam);
            return Ok(new { success = 1, message = "Sales Team Projection", data = result });
        }
        #endregion

        #region Update - Manual SalesTeamProjection
        [HttpPost]
        // [ResponseCache(Duration =3600,Location =ResponseCacheLocation.Any, VaryByHeader = "User-Agent",NoStore =false)]
        [SwaggerOperation(
                        Summary = "Sales Team Projection",
                        Description = "Sales Team Projection",
                        OperationId = "SalesForecastingComparison",
                        Tags = new[] { "SalesForecastComparison" }
                    )]
        [SwaggerResponse(200, "Sales Team Projection")]
        [SwaggerResponse(204, "Sales Team Projection", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> Update_SalesTeamProjection(SalesForecastEntity.SaleProjection_SalesTeam saleProjection_SalesTeam)
        {
            var result = await this.salesForecastLogic.Update_SalesTeamProjection(saleProjection_SalesTeam);
            return Ok(new { success = 1, message = "Sales Team Projection", data = result });
        }
        #endregion

        #endregion

        #endregion
    }
}