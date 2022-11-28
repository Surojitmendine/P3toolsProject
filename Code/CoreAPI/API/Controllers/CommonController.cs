using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Context;
using API.Helper;
using API.IdentityModels;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using API.BusinessLogic;
using System.Data;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class CommonController : ApiBase
    {



        #region -- Declaration --
        //BusinessLogic.CommonLogic common;
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private ILogger logger;
        //private readonly RoleManager<tbl_SYS_AspNet_Roles> _roleManager;
        //private readonly UserManager<ApplicationUser> _userManager;

        public IConfiguration Configuration { get; set; }
        public String ESSPConnection { get; set; }
        public String MSSQLConnection { get; set; }

        public CommonController(ILoggerFactory loggerFactory, DBContext db, IMapper mapper, ILogger<CommonController> logger, IConfiguration configuration /*, UserManager<ApplicationUser> userManager, ILogger<CommonController> logger,RoleManager<tbl_SYS_AspNet_Roles> roleManager*/) : base(loggerFactory)
        {
            Configuration = configuration;
            ESSPConnection = Configuration.GetConnectionString("ESSPConnection").ToString();
            MSSQLConnection = Configuration.GetConnectionString("MSSQLConnection").ToString();
            this.logger = logger;
            //this._roleManager = roleManager;
            //this._userManager = userManager;
            //common = new BusinessLogic.CommonLogic(db, mapper, userManager);
        }
        #endregion


        #region " Current User"
        [HttpGet]
        [SwaggerOperation(
                            Summary = "Get Current User Details",
                            Description = "Get Current User Details",
                            OperationId = "GetLoggedinUser",
                            Tags = new[] { "Common" }
                        )]
        [SwaggerResponse(201, "Get Current User Details", typeof(string))]
        [SwaggerResponse(204, "Get Current User Details not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> GetLoggedinUser(String UserEmail)
        {
            DataTable DT = clsLogin.Login_User_Information(ESSPConnection,UserEmail);
            return Ok(new { data = DT.Rows[0]["Name"], empno = DT.Rows[0]["Empno"] });
        }

        #endregion

        #region Menu 
        #region Menu List
        [HttpGet]
        public IActionResult EmployeeWiseMenuList(String EmployeeNo)
        {
            var d = clsLogin.Employee_Wise_Menu_List(MSSQLConnection, EmployeeNo);
            return Ok(d);
        }
        #endregion

        #region P3 Role List
        [HttpGet]
        public IActionResult P3RoleList()
        {
            var d = clsLogin.P3RoleList(MSSQLConnection);
            return Ok(new { success = 1, message = "Role List", data = d });
        }
        #endregion

        #region Employee List
        [HttpGet]
        public IActionResult EmployeeList(long IDRole)
        {
            var d = clsLogin.EmployeeList(MSSQLConnection);
            return Ok(new { success = 1, message = "Active Employee List", data = d });
        }
        #endregion

        #region Save Employee Role Mapping 
        [HttpPost]
        public IActionResult EmployeeRoleMappingSave(clsRoleMappingInfo info)
        {
            var d = clsLogin.EmployeeRoleMappingSave(MSSQLConnection, info.EmployeeNo, info.IDRole);
            return Ok(new { success = 1, message = "Save Employee Role Mapping Successfully", data = d });
        }
        #endregion

        #endregion


        #region "CurrentDatetime"
        [HttpGet]
        [SwaggerOperation(
                            Summary = "Current Datetime",
                            Description = "Current Datetime",
                            OperationId = "CurrentDatetime",
                            Tags = new[] { "Common" }
                        )]
        [SwaggerResponse(201, "Current Datetime found", typeof(string))]
        [SwaggerResponse(204, "Current Datetime not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult CurrentDatetime()
        {
            this.logger.LogInformation("", "");
            return Ok(new { success = 1, message = "Current Datetime", data = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") });
        }
        [HttpGet]
        [SwaggerOperation(
                            Summary = "Current Date",
                            Description = "Current Date",
                            OperationId = "CurrentDate",
                            Tags = new[] { "Common" }
                        )]
        [SwaggerResponse(201, "Current Date found", typeof(string))]
        [SwaggerResponse(204, "Current Date not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult CurrentDate()
        {
            return Ok(new { success = 1, message = "Current Date", data = DateTime.Now.ToString("dd/MM/yyyy") });
        }

        [HttpGet]
        [SwaggerOperation(
                            Summary = "Current Time",
                            Description = "Current Time",
                            OperationId = "CurrentTime",
                            Tags = new[] { "Common" }
                        )]
        [SwaggerResponse(201, "Current Time found", typeof(string))]
        [SwaggerResponse(204, "Current Time not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult CurrentTime()
        {
            return Ok(new { success = 1, message = "Current Time", data = DateTime.Now.ToString("hh:mm tt") });
        }

        [HttpGet]
        [SwaggerOperation(
                            Summary = "Month List",
                            Description = "Month List",
                            OperationId = "MonthList",
                            Tags = new[] { "Common" }
                        )]
        [SwaggerResponse(201, "Current Time found", typeof(string))]
        [SwaggerResponse(204, "Current Time not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult MonthList()
        {

            string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;

            List<dynamic> months = new List<dynamic>();

            for(int i = 0; i <= names.Length - 1; i++)
            {
                if (string.IsNullOrEmpty(names[i]) == false)
                {
                    months.Add(new { id = i + 1, text = names[i] });
                }
            }

            return Ok(new { success = 1, message = "Month List", data = months});
        }

        [HttpGet]
        [SwaggerOperation(
                            Summary = "Year List",
                            Description = "Year List",
                            OperationId = "YearList",
                            Tags = new[] { "Common" }
                        )]
        [SwaggerResponse(201, "Current Time found", typeof(string))]
        [SwaggerResponse(204, "Current Time not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult YearList()
        {

            var yearlist = Enumerable.Range(DateTime.Now.Year-1, 3).ToList();

            List<dynamic> years = new List<dynamic>();

            foreach(var item in yearlist)
            {
                years.Add(new { id = item, text = item });
            }

            return Ok(new { success = 1, message = "Year List", data = years });
        }

        #endregion

        #region Get Bing Image of the Day

        [AllowAnonymous]
        [HttpGet]
        [SwaggerOperation(
                            Summary = "Get Bing Image of The Day",
                            Description = "Get Bing Image of TheDay",
                            OperationId = "GetBingImageofTheDay",
                            Tags = new[] { "Common" }
                        )]
        [SwaggerResponse(201, "GetBingImageofTheDay found", typeof(string))]
        [SwaggerResponse(204, "GetBingImageofTheDay not found", typeof(string))]
        [SwaggerResponse(400, "GetBingImageofTheDay Request", typeof(string))]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 60, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetBingImageofTheDay()
        {


            //string ImageURL = string.Empty;

            //string imagesource = string.Empty;
            //Random rnd = new Random();

            //int toss = rnd.Next(2);
            //if (toss <= 0)
            //{
            //    ImageURL = BackgroundImage.Spotlight.GetImageUrls();
            //    imagesource = "spotlightImage";
            //}
            //else
            //{
            //    ImageURL = await BackgroundImage.BingImageOfTheDay.ImageOfTheDay();
            //    imagesource = "bingImage";
            //}

            // We can specify the region we want for the Bing Image of the Day.
            //try
            //{
            //    return Ok(new { data = new { imageurl = ImageURL, source = imagesource } });
            //}
            //catch (Exception)
            //{

            //    return NoContent();
            //}
             
            // Fixed Image
            string ImageURL = "dist/img/background.jpg";

            string imagesource = "ImageFixed";
            try
            {
                return Ok(new { data = new { imageurl = ImageURL, source = imagesource } });
            }
            catch (Exception)
            {

                return NoContent();
            }

        }

        #endregion

        //#region User Role
        //[HttpGet]
        //[SwaggerOperation(
        //                    Summary = "Get List of User Roles",
        //                    Description = "Get List User Roles",
        //                    OperationId = "GetUserRoles",
        //                    Tags = new[] { "Common" }
        //                )]
        //[SwaggerResponse(201, "User Roles", typeof(string))]
        //[SwaggerResponse(204, "User Roles not found", typeof(string))]
        //[SwaggerResponse(400, "Bad Request", typeof(string))]
        //public IActionResult GetUserRoles()
        //{
        //    var records = this._roleManager.Roles.Select(s => new { id = s.Id, text = s.Name }).ToList();

        //    if (records != null)
        //    {
        //        return Ok(new { success = 1, message = "User Roles found", data = records });
        //    }
        //    else if (records == null)
        //    {
        //        return NoContent();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        //[HttpGet]
        //[SwaggerOperation(
        //                    Summary = "GetUsersByRole",
        //                    Description = "GetUsersByRole",
        //                    OperationId = "GetUsersByRole",
        //                    Tags = new[] { "Common" }
        //                )]
        //[SwaggerResponse(201, "User Roles", typeof(string))]
        //[SwaggerResponse(204, "User Roles not found", typeof(string))]
        //[SwaggerResponse(400, "Bad Request", typeof(string))]
        //public IActionResult GetUsersByRole([FromQuery, SwaggerParameter("Role ID", Required = true)]Int32 RoleID)
        //{


        //    List<dynamic> userlist = this.common.GetUsersByRole(RoleID);

        //    if (userlist != null)
        //    {
        //        return Ok(new { success = 1, message = "GetUsersByRole", data = userlist });
        //    }
        //    else if (userlist == null)
        //    {
        //        return NoContent();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        //#endregion
    }
}