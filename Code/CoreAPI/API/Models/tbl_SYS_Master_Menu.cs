using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_SYS_Master_Menu
    {
        public tbl_SYS_Master_Menu()
        {
            InverseFK_MenuID_ParentNavigation = new HashSet<tbl_SYS_Master_Menu>();
            tbl_SYS_UserPermission = new HashSet<tbl_SYS_UserPermission>();
        }

        public int PK_MenuID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModuleName { get; set; }
        public string MenuName { get; set; }
        public int? ViewOrder { get; set; }
        public int? FK_MenuID_Parent { get; set; }
        public string MenuType { get; set; }
        public string PageRedirect { get; set; }
        public string TablesName { get; set; }
        public string RelatedTableName { get; set; }
        public string StoreProcedureName { get; set; }
        public string RelatedSPName { get; set; }

        public virtual tbl_SYS_Master_Menu FK_MenuID_ParentNavigation { get; set; }
        public virtual ICollection<tbl_SYS_Master_Menu> InverseFK_MenuID_ParentNavigation { get; set; }
        public virtual ICollection<tbl_SYS_UserPermission> tbl_SYS_UserPermission { get; set; }
    }
}
