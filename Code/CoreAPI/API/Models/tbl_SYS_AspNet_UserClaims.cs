using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_SYS_AspNet_UserClaims
    {
        public int Id { get; set; }
        public int FK_UserID { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public virtual tbl_SYS_AspNet_UserInformation FK_User { get; set; }
    }
}
