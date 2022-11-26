using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using API.Filters;
using API.IdentityModels;

namespace API.Helper
{
    [ServiceFilter(typeof(LoggingFilter))]
    public abstract class ApiBase : ControllerBase
    {
        private readonly ILogger _logger;

        public ApiBase(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("BaseController");
        }

        protected async Task<ApplicationUser> GetCurrentUserIdentity()
        {

            UserManager<ApplicationUser> userManager = this.HttpContext.RequestServices.GetService<UserManager<ApplicationUser>>();
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            Int32 UserID;
            int.TryParse(identityClaims.FindFirst("sub").Value, out UserID);

            var user = await userManager.FindByIdAsync(UserID.ToString());

            return user;
        }

        protected async Task<ApplicationUser> GetCurrentUser()
        {

            var r = await this.GetCurrentUserIdentity();

            return r;
        }
    }
}