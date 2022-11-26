using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Cors;
using API.IdentityModels;
using Common.Utility;
using System.Net.Http;
using System.Configuration;
using API.BusinessLogic;
using System.Data;
using Newtonsoft.Json;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class AuthController : ControllerBase
    {


        [HttpPost]
        public IActionResult ValidLogin([FromBody] Login l)
        {
            String APIBaseurl = "http://salesapi.mendine.co.in/";
            clsAPI objAPI = new clsAPI(APIBaseurl);
            var data = new List<clsEmployeeInfo>();
            String apiMethod = "";
            
            apiMethod = "api/Emp/UserNamePwdcheck?uname=" + l.uname + "&pwd=" + l.pwd;
        
            HttpResponseMessage response = objAPI.APIPost(apiMethod);
            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(responseString))
                {


                    data = JsonConvert.DeserializeObject<List<clsEmployeeInfo>>(responseString);
                    if (data.Count > 0 && data[0].Truefalse == true)
                    {
                        //    String Email = data[0].empemail;
                        //    String Designation = data[0].designationshortform;
                        //    String Empno = data[0].empno;

                        //    // HQ seperate SP as it's not coming from API 
                        //    string constring = configurationmanager.connectionstrings["salesmaestroz"].tostring();
                        //    list<clsloginresultinfo> mlogin = clslogin.employeedetail(constring, empno);
                        //    session["hq"] = mlogin.count > 0 ? mlogin[0].hq : " ";
                        //    session["division"] = clshelper.fnconvert2string(data[0].division);
                        //    session["empname"] = clshelper.fnconvert2string(data[0].empname);
                        //    session["designation"] = clshelper.fnconvert2string(data[0].designationname);
                        //    session["shortname"] = clshelper.fnconvert2string(data[0].designationshortform);
                        //    session["empemail"] = clshelper.fnconvert2string(data[0].empemail);
                        //    session["empno"] = clshelper.fnconvert2string(data[0].empno);
                        //    session["idcyclegroup"] = mlogin.count > 0 ? mlogin[0].idcyclegroup.tostring() : "0";

                        return new OkObjectResult(new { success = true, data });
                    }
                    else
                    {
                        return new OkObjectResult(new { success = false, data });
                    }
                }
                else
                {
                    return new OkObjectResult(new { success = false, data });
                }
            }
            else
            {
                return new OkObjectResult(new { success = false, data });
            }

        }



        //readonly UserManager<ApplicationUser> userManager;
        //readonly SignInManager<ApplicationUser> signInManager;        
        //readonly IConfiguration configuration;
        //readonly ILogger<AuthController> logger;


        //public AuthController(UserManager<ApplicationUser> userManager,
        //   SignInManager<ApplicationUser> signInManager,
        //   IConfiguration configuration,
        //   ILogger<AuthController> logger)
        //{
        //    this.userManager = userManager;
        //    this.signInManager = signInManager;
        //    this.configuration = configuration;
        //    this.logger = logger;

        //}


        //[HttpGet]
        //[Route("token")]
        //[SwaggerOperation(
        //    Summary = "Genarate Auth Token",
        //    Description = "Requires admin privileges",
        //    OperationId = "token",
        //    Tags = new[] { "Auth" }
        //)]
        //[SwaggerResponse(201, "Token Created",typeof(string))]
        //[SwaggerResponse(404, "Invalid User Information", typeof(string))]               
        //public async Task<IActionResult> CreateToken([ FromQuery,SwaggerParameter("User's Email", Required = true)]string username, [FromQuery,SwaggerParameter("Password", Required = true)] string password)
        //{


        //    var loginResult = await signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

        //    if (!loginResult.Succeeded)
        //    {
        //        return BadRequest();
        //    }

        //    var user = await userManager.FindByNameAsync(username);
        //    var tokenString = GetToken(user);
        //    return Ok(new {success=1,message="", access_token = tokenString });

        //}




        /*[HttpPost]
        [Route("register")]
        [AllowAnonymous]

        [SwaggerOperation(
            Summary = "Register User",
            Description = "Registerd user allow .....",
            OperationId = "register",
            Tags = new[] { "User" }
        )]
        [SwaggerResponse(201, "User Created")]
        [SwaggerResponse(404, "Something went wrong")]  
       
        public async Task<IActionResult> Register([FromQuery,SwaggerParameter("User's Email", Required = true)]string Username,
            [FromQuery,SwaggerParameter("User's Firstname", Required = false)]string FirstName, [FromQuery,SwaggerParameter("User's Lastname", Required = false)] string LastName,
            [FromQuery,SwaggerParameter("User's Email", Required = true)]string Email, [FromQuery,SwaggerParameter("User's Password", Required = true)] string Password)
        {

            var user = new ApplicationUser
            {
                //TODO: Use Automapper instaed of manual binding  

                UserName = Username,
                //FirstName = FirstName,
                //LastName = LastName,
                Email = Email
            };

            var identityResult = await this.userManager.CreateAsync(user, Password);
            if (identityResult.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                // return Ok(GetToken(user));
            }
            else
            {
                return BadRequest(identityResult.Errors);
            }

            return BadRequest(ModelState);


        }*/

        //private string GetToken(ApplicationUser user)
        //{
        //    var utcNow = DateTime.UtcNow;

        //    var claims = new Claim[]
        //    {
        //                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        //                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
        //    };


        //    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Tokens:Key"]));
        //    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);


        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Tokens:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var expires = DateTime.Now.AddDays(Convert.ToDouble(this.configuration["Tokens:ExpireDays"]));

        //    var token = new JwtSecurityToken(
        //        this.configuration["Tokens:Issuer"],
        //        this.configuration["Tokens:Issuer"],
        //        claims,
        //        expires: expires,
        //        signingCredentials: creds
        //    );

        //    /*var jwt = new JwtSecurityToken(
        //        signingCredentials: signingCredentials,
        //        claims: claims,
        //        notBefore: utcNow,
        //        expires: utcNow.AddSeconds(this.configuration.GetValue<int>("Tokens:Lifetime")),
        //        audience: this.configuration.GetValue<String>("Tokens:Audience"),
        //        issuer: this.configuration.GetValue<String>("Tokens:Issuer")
        //        );*/



        //    return new JwtSecurityTokenHandler().WriteToken(token);

        //}

    }
}