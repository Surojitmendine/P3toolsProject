using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_SYS_AspNet_UserLogins
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public int FK_UserID { get; set; }

        public virtual tbl_SYS_AspNet_UserInformation FK_User { get; set; }
    }
}
