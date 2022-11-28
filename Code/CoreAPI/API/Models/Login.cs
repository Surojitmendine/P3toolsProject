using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Login
    {
        public String uname { get; set; }
        public String pwd { get; set; }
    }

    public class clsEmployeeInfo
    {
        public String Empno { get; set; } = "";
        public String Empname { get; set; } = "";
        public String code { get; set; }
        public String IDDept { get; set; }
        public String Department { get; set; } = "";
        //public String Post { get; set; } = "";
        public String Empemail { get; set; } = "";
        public String IDRole { get; set; } = "";
        public String RoleName { get; set; } = "";
        public Boolean Truefalse { get; set; } = true;
    }

    public class clsUserMainMenuInfo
    {
        public long MenuSRL { get; set; } = 0;
        public String MainMenu { get; set; } = "";


    }
    public class clsUserSubMenuInfo
    {
        public long MenuSRL { get; set; } = 0;
        public String MainMenu { get; set; } = "";
        public String SubMenu { get; set; } = "";
        public String MenuURL { get; set; } = "";
        public String MenuIcon { get; set; } = "";
    }
    public class clsUserMenuInfo
    {
        public List<clsUserMainMenuInfo> MainMenu { get; set; } = new List<clsUserMainMenuInfo>();
        public List<clsUserSubMenuInfo> SubMenu { get; set; } = new List<clsUserSubMenuInfo>();
    }
}
