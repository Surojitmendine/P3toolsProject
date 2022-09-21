using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_SYS_AspNet_UserTokens
    {
        public int UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual tbl_SYS_AspNet_UserInformation User { get; set; }
    }
}
