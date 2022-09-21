using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Helper;
using API.IdentityModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static API.Entity.CommonEntity;

namespace API.BusinessLogic
{
    public class CommonLogic
    {
        #region Declaration
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        readonly DBContext db;
        private readonly IMapper _mapper;
        private readonly Functions functions;
        readonly UserManager<ApplicationUser> userManager;

        public CommonLogic()
        {
            this.functions = new Functions();
            this.db = new DBContext();
        }
        public CommonLogic(DBContext db, IMapper mapper):this()
        {
            this.db = db;
            this._mapper = mapper;

        }

        public CommonLogic(DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager) : this(db, mapper)
        {

            this.userManager = userManager;
        }


        #endregion

        #region -- Fill Combo Functions--

        
        #endregion

        #region -- Common --
        public Int32 ModifiedOn_DateKey()
        {
            Int32 DateKey = 20191026;// DateTime.Today.Year.ToString()+ DateTime.Today.Month.ToString()+ DateTime.Today.Day.ToString();
            return DateKey;
        }

        public Int32 ModifiedOn_TimeKey()
        {
            Int32 TimeKey = 30261;// DateTime.Today.TimeOfDay.TotalSeconds.ToString();
            return TimeKey;
        }
        #endregion

     

        public List<dynamic> GetUsersByRole(Int32 RoleID)
        {
            
           // var users = this.userManager.Users.Where(x => x.IsActive == true && x.IsDeleted == false).Include(u => u.UserRoles).Select(s => s).ToList();

            List<dynamic> userlist = this.userManager.Users.Where(x => x.IsDeleted == false && x.IsActive == true && x.UserRoles.Any(x => x.RoleId == RoleID)).Select(s => new
            {
                Id = s.Id,
                text = s.FirstName + ' ' + s.MiddleName + ' ' + s.LastName,

            }).ToList<dynamic>();



            //var emps = (from emp in db.tbl_PS_Master_Employee.AsEnumerable()
            //            join user in userlist on emp.FK_UserID equals user.Id
            //            where emp.IsActive == true && emp.IsDeleted == false 
            //            select new
            //            {
            //                id = emp.PK_EmployeeID,
            //                text = user.text

            //            }).ToList<dynamic>();

            //return emps;
            return userlist;
        }
    }
}
