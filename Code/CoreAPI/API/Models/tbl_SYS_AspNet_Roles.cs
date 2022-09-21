using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_SYS_AspNet_Roles
    {
        public tbl_SYS_AspNet_Roles()
        {
            tbl_SYS_AspNet_RoleClaims = new HashSet<tbl_SYS_AspNet_RoleClaims>();
            tbl_SYS_AspNet_UserRoles = new HashSet<tbl_SYS_AspNet_UserRoles>();
        }

        public int PK_RoleID { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public virtual ICollection<tbl_SYS_AspNet_RoleClaims> tbl_SYS_AspNet_RoleClaims { get; set; }
        public virtual ICollection<tbl_SYS_AspNet_UserRoles> tbl_SYS_AspNet_UserRoles { get; set; }
    }
}
