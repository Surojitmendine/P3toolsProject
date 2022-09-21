using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Context;
using API.Helper;
using API.IdentityModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SetupProcessController : ApiBase
    {
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private readonly IMapper _mapper;
        private SetupProcessLogic setupProcessLogic;
        readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Functions functions = new Functions();
        private readonly ExcelReader excelReader;
        public SetupProcessController(ILoggerFactory loggerFactory, DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment) : base(loggerFactory)
        {
            this._mapper = mapper;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            excelReader = new ExcelReader();

            this.setupProcessLogic = new SetupProcessLogic(db, mapper, userManager);
        }

        #region -Primary Data Sync-
        [HttpGet]
        public async Task<IActionResult> DataTransfer(string FromDate, string ToDate, string SpToCall)
        {
            await this.setupProcessLogic.DataTransfer(FromDate, ToDate, SpToCall);
            return Ok(new { data = "Primary Sales Data Transfer Successfully !!!!!" });
        }
        #endregion

        #region -Closing Stock Data Sync-
        [HttpGet]
        public async Task<IActionResult> SyncClosingStockData(string FromDate, Int32 Year, Int32 Month)
        {
            await this.setupProcessLogic.SyncClosingStockData(FromDate, Year, Month);
            return Ok(new { data = "Closing Stock Data Transfer Successfully !!!!!" });
        }


        [HttpGet]
        [SwaggerOperation(
                        Summary = "Closing Stock",
                        Description = "Closing Stock",
                        OperationId = "List_SecondarySales",
                        Tags = new[] { "SalesForecast" }
                    )]

        [SwaggerResponse(200, "Closing Stock")]
        [SwaggerResponse(204, "Closing Stock", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        //string ForecastingType
        public async Task<IActionResult> List_ClosingStock([FromQuery, SwaggerParameter("Year", Required = true)] int Year,
           [FromQuery, SwaggerParameter("Month", Required = true)] int Month)
        {
            //var records = await this.salesForecastLogic.List_SecondarySales( Year, Month);
            var records = this.setupProcessLogic.List_ClosingStock(Year, Month);
            if (records != null && records.Count() > 0)
            {
                return Ok(new { success = 1, message = "Closing Stock", data = records });
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
        public async Task<IActionResult> DataProcessing(Int32 Year, Int32 Month, string ForecastingType, string SpToCall)
        {
            var result = await this.setupProcessLogic.DataProcessing(Year, Month, ForecastingType, SpToCall);
            return Ok(new { success = true, message = "",data=result });
        }
    }
}