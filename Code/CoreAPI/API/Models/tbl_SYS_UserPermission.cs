using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_SYS_UserPermission
    {
        public int PK_RoleAccessID { get; set; }
        public int? Createdby { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CompanyID { get; set; }
        public int? FK_RoleID { get; set; }
        public int? ModuleID { get; set; }
        public int? FK_MenuID { get; set; }
        public bool? IsShowMenuInGroup { get; set; }
        public bool? Permission_New { get; set; }
        public bool? Permission_Edit { get; set; }
        public bool? Permission_Del { get; set; }
        public bool? Permission_View { get; set; }
        public bool? Permission_Print { get; set; }
        public bool? Permission_Export { get; set; }
        public bool? Permission_Import { get; set; }
        public bool? Permission_Report { get; set; }
        public bool? Permission_Others { get; set; }

        public virtual tbl_SYS_Master_Menu FK_Menu { get; set; }
    }
}
