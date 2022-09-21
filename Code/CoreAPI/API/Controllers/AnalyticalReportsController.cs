using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Helper;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using API.Context;
using AutoMapper;
using System.Reflection;
using System.Globalization;

using Swashbuckle.AspNetCore.Filters;
using Newtonsoft.Json.Converters;
using API.Helper.ReqObject;
using API.BusinessLogic;
using API.IdentityModels;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class AnallyticalReportsController : ApiBase
    {
        #region --Init --
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private readonly IMapper _mapper;
        private AnalyticalReportsLogic analyticalReports;
        readonly UserManager<ApplicationUser> userManager;
        private readonly Functions functions = new Functions();

        public AnallyticalReportsController(ILoggerFactory loggerFactory, DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager) : base(loggerFactory)
        {
            this._mapper = mapper;
            this.userManager = userManager;
            this.analyticalReports = new AnalyticalReportsLogic(db, mapper, userManager);
        }
        #endregion

        #region -- Depot Replenishment --

        #region -- Search Fields --
        [HttpGet]
        [SwaggerOperation(
                        Summary = "Depot Replenishment Indent Summary",
                        Description = "Depot Replenishment Indent Summary",
                        OperationId = "UpdateProjection",
                        Tags = new[] { "AnallyticalReports" }
                    )]

        [SwaggerResponse(200, "Depot Replenishment Indent Summary")]
        [SwaggerResponse(204, "Depot Replenishment Indent Summary", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> Reports_SearchFields()
        {
            var records = await this.analyticalReports.Reports_SearchFields();
            if (records != null && records.Length > 0)
            {
                return Ok(new { success = 1, message = "Depot Replenishment Indent Summary", data = new { division = records[0], depot = records[1], product = records[2], packunit = records[3] } });
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

        #region -- Indent Summary Report--
        [HttpGet]       
        [SwaggerOperation(
                        Summary = "Depot Replenishment Indent Summary",
                        Description = "Depot Replenishment Indent Summary",
                        OperationId = "DepotReplenishment_IndentSummary ",
                        Tags = new[] { "AnallyticalReports " }
                    )]
        [SwaggerResponse(200, "Depot Replenishment Indent Summary")]
        [SwaggerResponse(204, "Depot Replenishment Indent Summary", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        
        public async Task<IActionResult> DepotReplenishment_IndentSummary([FromQuery, SwaggerParameter("Year", Required = true)] int ForYear, [FromQuery, SwaggerParameter("Month", Required = true)] int ForMonth,
         string Divisions, string Depots, string Products, string PackUints, bool ReInitializeCache = true)
        {
            var records = await this.analyticalReports.DepotReplenishment_IndentSummary(ForYear, ForMonth, Divisions, Depots, Products, PackUints, ReInitializeCache);
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

        #endregion
    }
}