using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Helper
{
    public class AppAuthorization : AuthorizeAttribute//, IAsyncAuthorizationFilter
    {

        //public void OnAuthorization(AuthorizationFilterContext filterContext)
        //{
        //    string token=filterContext.HttpContext.Request.Headers["Authorization"];

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        filterContext.Result = new UnauthorizedResult();
        //    }
        //    else
        //    {
        //        filterContext.Result = new AuthorizedResult();
        //    }
            

           
        //}
       
    }
}
