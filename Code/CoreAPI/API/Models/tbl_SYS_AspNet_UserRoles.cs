using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_SYS_AspNet_UserRoles
    {
        public int PK_UserID { get; set; }
        public int PK_RoleID { get; set; }

        public virtual tbl_SYS_AspNet_Roles PK_Role { get; set; }
        public virtual tbl_SYS_AspNet_UserInformation PK_User { get; set; }
    }
}
