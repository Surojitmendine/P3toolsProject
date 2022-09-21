using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Entity;
using API.Helper;
using API.Context;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.IdentityModels;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserControlController : ApiBase
    {
        UserControlLogic userControl;
        public UserControlController(ILoggerFactory loggerFactory,UserManager<ApplicationUser> userManager,
        RoleManager<tbl_SYS_AspNet_Roles> roleManager, DBContext db, IMapper mapper) : base(loggerFactory)
        {
            userControl = new UserControlLogic(userManager, roleManager, db, mapper);
        }

        #region User Control

        [HttpPost]

        [SwaggerOperation(
                    Summary = "Add New User",
                    Description = "Add New User",
                    OperationId = "AddUser",
                    Tags = new[] { "User" }
                )]
        [SwaggerResponse(201, "User Created", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> AddUser([FromBody, SwaggerParameter("Add User's Details", Required = true)]UserEntity user)
        {
            var createduser = await this.userControl.AddUser(user);

            if (createduser[0])
            {
                return Ok(new { success = 1, message = "User Created successfully" });
            }
            else if (!createduser[0])
            {
                return Ok(new { success = 0, message = createduser[1] });
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]

        [SwaggerOperation(
                    Summary = "Update Existing User",
                    Description = "Update Existing User",
                    OperationId = "UpdateUser",
                    Tags = new[] { "User" }
                )]
        [SwaggerResponse(201, "User Created", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> UpdateUser([FromBody, SwaggerParameter("Update User's Details", Required = true)]UserEntity user)
        {


            var updateduser = await this.userControl.UpdateUser(user);


            if (updateduser)
            {

                return Ok(new { success = 1, message = "User Updated successfully" });
            }
            else
            {
                return BadRequest();
            }

        }


        [HttpGet]

        [SwaggerOperation(
                            Summary = "Get List of all User",
                            Description = "Get List of all User",
                            OperationId = "ListUser",
                            Tags = new[] { "User" }
                        )]
        [SwaggerResponse(201, "Users found", typeof(string))]
        [SwaggerResponse(204, "Users not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> ListUser()
        {

            List<dynamic> userlist = await this.userControl.ListUser();

            if (userlist != null && userlist.Count() > 0)
            {
                return Ok(new { success = 1, message = "Users list", data = userlist });
            }
            else if (userlist == null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]

        [SwaggerOperation(
                            Summary = "Get User by ID",
                            Description = "Get User by ID",
                            OperationId = "GetUserByID",
                            Tags = new[] { "User" }
                        )]
        [SwaggerResponse(201, "User found", typeof(string))]
        [SwaggerResponse(204, "User not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> GetUserByID([FromQuery, SwaggerParameter("User's ID", Required = true)]Int32 UserID)
        {
            var user = await this.userControl.GetUserByID(UserID);

            if (user != null)
            {
                return Ok(new { success = 1, message = "User found", data = user });
            }
            else if (user == null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]

        [SwaggerOperation(
                             Summary = "Get allowed menus to user",
                             Description = "Get allowed menus to user",
                             OperationId = "GetAllowedMenusByUser",
                             Tags = new[] { "User" }
                         )]
        [SwaggerResponse(201, "User found", typeof(string))]
        [SwaggerResponse(204, "User not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> GetAllowedMenusByUser()
        {
            var applicationuser =await this.GetCurrentUser();
            
          var allowedmenus=  this.userControl.GetAllowedMenusByUser(applicationuser);

            return Ok(new { success = 1, message = "User's allowed menus", data = allowedmenus });
        }


        #endregion
    }
}