using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Entity;
using API.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.IdentityModels;


namespace API
{
    public class UserControlLogic
    {
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        readonly UserManager<ApplicationUser> userManager;
        readonly RoleManager<tbl_SYS_AspNet_Roles> roleManager;
        readonly DBContext db;
        private readonly IMapper _mapper;
        //private readonly BusinessLogic.PumpSetupLogic pumpSetup;

        public UserControlLogic()
        {

        }
        public UserControlLogic(UserManager<ApplicationUser> userManager,
        RoleManager<tbl_SYS_AspNet_Roles> roleManager, DBContext db, IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.db = db;
            this._mapper = mapper;
            //this.pumpSetup = new BusinessLogic.PumpSetupLogic(db, mapper);
        }


        #region User Control



        public async Task<dynamic[]> AddUser(UserEntity user)
        {
            bool buserCreated = false;
            string errormessage = string.Empty;
            dynamic[] d = new dynamic[2];
            var applicationuser = new ApplicationUser()
            {
                IsActive = true,
                IsDeleted = false,
                UserName = user.UserName,
                Email = user.Email,
                //PasswordHash=this.userManager.PasswordHasher.HashPassword()
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                Title = user.Title,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Gender = user.Gender,
                MobileNo = user.PhoneNumber,
                DisplayUserName = user.DisplayUserName
            };

            var createduser = this.userManager.CreateAsync(applicationuser, user.Password);

            if (createduser.Result == IdentityResult.Success)
            {
                var role = await this.roleManager.FindByIdAsync(user.UserRole);


                //var addasemployee = await this.pumpSetup.AddEmployee(employee);

                var addtorole = await this.userManager.AddToRoleAsync(applicationuser, role.Name);
                //if (addtorole.Succeeded && addasemployee == true)
                //{
                //    buserCreated = true;
                //}
            }
            else
            {
                errormessage = createduser.Result.Errors.Select(s => s.Description).First().ToString();


            }

            d[0] = buserCreated;
            d[1] = errormessage;
            return d;

        }

        public async Task<bool> UpdateUser(UserEntity user)
        {
            bool buserUpdated = false;

            // var applicationuser = await this.userManager.FindByIdAsync(user.UserID.ToString());
            var applicationuser = await this.userManager.Users.Where(x => x.Id == user.UserID).Include(u => u.UserRoles).Select(s => s).SingleOrDefaultAsync();

            applicationuser.UserName = user.UserName;
            applicationuser.Email = user.Email;
            applicationuser.PhoneNumber = user.PhoneNumber;
            applicationuser.Password = string.IsNullOrEmpty(user.Password) == true ? applicationuser.Password : user.Password;
            applicationuser.Title = user.Title;
            applicationuser.FirstName = user.FirstName;
            applicationuser.MiddleName = user.MiddleName;
            applicationuser.LastName = user.LastName;
            applicationuser.Gender = user.Gender;
            applicationuser.MobileNo = user.PhoneNumber;
            applicationuser.DisplayUserName = user.DisplayUserName;

            var updateduser = await this.userManager.UpdateAsync(applicationuser);

            if (updateduser.Succeeded)
            {

                buserUpdated = true;
                var role = await this.roleManager.FindByIdAsync(user.UserRole);

                if (applicationuser.UserRoles.Count > 0)
                {
                    var oldRole = applicationuser.UserRoles.SingleOrDefault();

                    if (Convert.ToInt32(user.UserRole) != oldRole.RoleId)
                    {
                        var oldRolename = await this.roleManager.FindByIdAsync(oldRole.RoleId.ToString());
                        if (Task.Run(async () => await this.userManager.RemoveFromRoleAsync(applicationuser, oldRolename.ToString())).Result.Succeeded)
                        {
                            var addtorole = await this.userManager.AddToRoleAsync(applicationuser, role.Name);
                        }
                    }
                }
                else
                {
                    var addtorole = await this.userManager.AddToRoleAsync(applicationuser, role.Name);
                }


                var userrole = await this.userManager.GetRolesAsync(applicationuser);
               

                //var updateemployee = await this.pumpSetup.UpdateEmployeeByUserID(employee);

                var resetpasstoken = await this.userManager.GeneratePasswordResetTokenAsync(applicationuser);

                if (string.IsNullOrEmpty(user.Password) == false)
                {
                    var resetpass = await this.userManager.ResetPasswordAsync(applicationuser, resetpasstoken.ToString(), user.Password);
                }


            }

            return buserUpdated;

        }

        public async Task<List<dynamic>> ListUser()
        {
            List<dynamic> userlist = await this.userManager.Users.Select(s => new
            {
                UserID = s.Id,
                Name = Convert.ToString(s.FirstName) + ' ' + Convert.ToString(s.MiddleName) + ' ' + Convert.ToString(s.LastName),
                Role = Convert.ToString(s.UserRoles.Select(s => s.Role.Name).SingleOrDefault())

            }).ToListAsync<dynamic>();

            return userlist;
        }

        public dynamic GetUserByID(Int32 UserID)
        {

            //var SalaryMonthly = db.tbl_PS_Master_Employee.Where(x => x.FK_UserID == UserID).Select(s => s.SalaryMonthly).SingleOrDefault();

            dynamic user = this.userManager.Users.Where(x => x.Id == UserID).Include(u => u.UserRoles)
                   .Select(s => new
                   {
                       UserID = s.Id,
                       s.Email,
                       s.PhoneNumber,
                       s.Title,
                       s.FirstName,
                       s.MiddleName,
                       s.LastName,
                       s.Gender,
                       s.DisplayUserName,
                       s.UserName,
                       s.PasswordHash,
                       UserRole = s.UserRoles.Select(s => s.RoleId).SingleOrDefault()
                   })
                   .SingleAsync();

            return user;
        }

        public dynamic GetAllowedMenusByUser(ApplicationUser user)
        {
            var userroleid =Convert.ToInt32(user.UserRoles.Select(x => x.RoleId).Single());

            var allaowedmenus = db.tbl_SYS_UserPermission.Where(x => x.FK_RoleID == userroleid && x.IsShowMenuInGroup==true)
                .Select(x => x.FK_MenuID).ToList();

            return allaowedmenus;

        }

        #endregion
    }
}
