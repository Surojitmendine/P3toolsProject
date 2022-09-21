using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_SYS_AspNet_UserInformation
    {
        public tbl_SYS_AspNet_UserInformation()
        {
            tbl_SYS_AspNet_UserClaims = new HashSet<tbl_SYS_AspNet_UserClaims>();
            tbl_SYS_AspNet_UserLogins = new HashSet<tbl_SYS_AspNet_UserLogins>();
            tbl_SYS_AspNet_UserRoles = new HashSet<tbl_SYS_AspNet_UserRoles>();
            tbl_SYS_AspNet_UserTokens = new HashSet<tbl_SYS_AspNet_UserTokens>();
        }

        public int PK_UserID { get; set; }
        public int? FK_UserID_CreatedBy { get; set; }
        public int? FK_DateKey_CreatedOn { get; set; }
        public int? FK_TimeKey_CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public int? FK_UserID_ModifiedBy { get; set; }
        public int? FK_DateKey_ModifiedOn { get; set; }
        public int? FK_TimeKey_ModifiedOn { get; set; }
        public int? FK_ClientID { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string DisplayUserName { get; set; }
        public string UserImagePath { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Device_UUID { get; set; }

        public virtual ICollection<tbl_SYS_AspNet_UserClaims> tbl_SYS_AspNet_UserClaims { get; set; }
        public virtual ICollection<tbl_SYS_AspNet_UserLogins> tbl_SYS_AspNet_UserLogins { get; set; }
        public virtual ICollection<tbl_SYS_AspNet_UserRoles> tbl_SYS_AspNet_UserRoles { get; set; }
        public virtual ICollection<tbl_SYS_AspNet_UserTokens> tbl_SYS_AspNet_UserTokens { get; set; }
    }
}
