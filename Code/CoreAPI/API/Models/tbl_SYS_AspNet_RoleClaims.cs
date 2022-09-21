using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_SYS_AspNet_RoleClaims
    {
        public int Id { get; set; }
        public int FK_RoleID { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public virtual tbl_SYS_AspNet_Roles FK_Role { get; set; }
    }
}
