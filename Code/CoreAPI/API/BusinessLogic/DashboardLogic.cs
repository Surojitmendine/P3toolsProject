using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Helper;
using API.Helper.ReqObject;
using API.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.IdentityModels;

namespace API.BusinessLogic
{
    public class DashboardLogic
    {
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private DBContext db;
        private readonly IMapper _mapper;
        private readonly Functions functions;
        private CommonLogic common;
        //private PumpSetup pumpSetup;
        readonly UserManager<ApplicationUser> userManager;

        public DashboardLogic()
        {

            this.functions = new Functions();

        }
        public DashboardLogic(DBContext db, IMapper mapper):this()
        {
            this.db = db;
            this._mapper = mapper;
            this.common = new CommonLogic(db, mapper);
        }

        public DashboardLogic(DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager) : this(db, mapper)
        {
            this.userManager = userManager;
        }

       

        
    }
}
