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
using Microsoft.Extensions.Logging;
using API.BusinessLogic;
using API.IdentityModels;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class DashboardController : ApiBase
    {        
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        
       
        readonly UserManager<ApplicationUser> userManager;
        private DashboardLogic _dashboard;
        public DashboardController(ILoggerFactory loggerFactory, DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager) : base(loggerFactory)
        {
            
            this.userManager = userManager;
            this._dashboard = new DashboardLogic(db,mapper,userManager);
        }

   




    }
}