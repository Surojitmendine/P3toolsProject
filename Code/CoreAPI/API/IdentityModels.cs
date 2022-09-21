using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Configuration;
using System.IO;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using API.Helper;

namespace API.IdentityModels
{
    public class ApplicationUser : IdentityUser<Int32>
    {
        /*
        Asp.Net Identity Default Columns

        UserName	            nvarchar(256)	Checked
        NormalizedUserName	    nvarchar(256)	Checked
        Email	                nvarchar(256)	Checked
        NormalizedEmail	        nvarchar(256)	Checked
        EmailConfirmed	        bit	Unchecked
        PasswordHash	        nvarchar(MAX)	Checked
        SecurityStamp	        nvarchar(MAX)	Checked
        ConcurrencyStamp	    nvarchar(MAX)	Checked
        PhoneNumber	            nvarchar(MAX)	Checked
        PhoneNumberConfirmed	bit	Unchecked
        TwoFactorEnabled	    bit	Unchecked
        LockoutEnd	            datetimeoffset(7)	Checked
        LockoutEnabled	        bit	Unchecked
        AccessFailedCount	    int	Unchecked
        */
      
        public Nullable<Int32> FK_UserID_CreatedBy { get; set; }
        public Nullable<Int32> FK_DateKey_CreatedOn { get; set; }
        public Nullable<Int32> FK_TimeKey_CreatedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<Int32> FK_UserID_ModifiedBy { get; set; }
        public Nullable<Int32> FK_DateKey_ModifiedOn { get; set; }
        public Nullable<Int32> FK_TimeKey_ModifiedOn { get; set; }
        public Nullable<Int32> FK_ClientID { get; set; }
        
        //public Nullable<Int32> FK_FinancialYearID { get; set; }
   
        public string Password { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string DisplayUserName { get; set; }
        public string UserImagePath { get; set; }
        public Nullable<bool> IsSuperAdmin { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Device_UUID { get; set; }

        public virtual ICollection<tbl_SYS_AspNet_UserClaims> Claims { get; set; }
        public virtual ICollection<tbl_SYS_AspNet_UserLogins> Logins { get; set; }
        public virtual ICollection<tbl_SYS_AspNet_UserTokens> Tokens { get; set; }
        public virtual ICollection<tbl_SYS_AspNet_UserRoles> UserRoles { get; set; }

    }

    public class tbl_SYS_AspNet_UserRoles : IdentityUserRole<Int32>
    {

        public virtual ApplicationUser User { get; set; }
        public virtual tbl_SYS_AspNet_Roles Role { get; set; }

    }

    public class tbl_SYS_AspNet_Roles : IdentityRole<Int32>
    {
        public virtual ICollection<tbl_SYS_AspNet_UserRoles> UserRoles { get; set; }
        public virtual ICollection<tbl_SYS_AspNet_RoleClaims> RoleClaims { get; set; }
    }

    public class tbl_SYS_AspNet_UserClaims : IdentityUserClaim<Int32> 
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class tbl_SYS_AspNet_UserLogins : IdentityUserLogin<Int32> {
        public virtual ApplicationUser User { get; set; }
    }

    public class tbl_SYS_AspNet_RoleClaims : IdentityRoleClaim<Int32>
    {  

        //public int? CompanyID { get; set; }
        //public int? ModuleID { get; set; }
        //public int? FK_MenuID { get; set; }
        //public bool? IsShowMenuInGroup { get; set; }
        //public bool? Permission_New { get; set; }
        //public bool? Permission_Edit { get; set; }
        //public bool? Permission_Del { get; set; }
        //public bool? Permission_View { get; set; }
        //public bool? Permission_Print { get; set; }
        //public bool? Permission_Export { get; set; }
        //public bool? Permission_Import { get; set; }
        //public bool? Permission_Report { get; set; }
        //public bool? Permission_Others { get; set; }

        public virtual tbl_SYS_AspNet_Roles Role { get; set; }

    }

    public class tbl_SYS_AspNet_UserTokens : IdentityUserToken<Int32> 
    {
        public virtual ApplicationUser User { get; set; }
    }



    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, tbl_SYS_AspNet_Roles, Int32, tbl_SYS_AspNet_UserClaims, tbl_SYS_AspNet_UserRoles, tbl_SYS_AspNet_UserLogins, tbl_SYS_AspNet_RoleClaims, tbl_SYS_AspNet_UserTokens>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //https://stackoverflow.com/a/48484797
            if (!optionsBuilder.IsConfigured)
            {
                
                EnvDBConfig envConfig = new EnvDBConfig();
                envConfig.ConfigureDB(optionsBuilder);

            }
        }

        //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-2.2#default-model-configuration
        /**
        *[.NET Core 2.1 Identity get all users with their associated roles](https://stackoverflow.com/a/51005445)
        *
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {


                b.Property(p => p.Id).HasColumnName("PK_UserID").HasColumnType("int").UseIdentityColumn();
                b.Property(p => p.FK_UserID_CreatedBy).IsRequired(false);
                b.Property(p => p.FK_DateKey_CreatedOn).IsRequired(false);
                b.Property(p => p.FK_TimeKey_CreatedOn).IsRequired(false);
                b.Property(p => p.IsDeleted).IsRequired(false);
                b.Property(p => p.IsActive).IsRequired(false);
                b.Property(p => p.FK_UserID_ModifiedBy).IsRequired(false);
                b.Property(p => p.FK_DateKey_ModifiedOn).IsRequired(false);
                b.Property(p => p.FK_TimeKey_ModifiedOn).IsRequired(false);
                b.Property(p => p.FK_ClientID).IsRequired(false);               
                b.Property(p => p.Password).IsRequired(false);
                b.Property(p => p.Title).IsRequired(false);
                b.Property(p => p.FirstName).IsRequired(false);
                b.Property(p => p.MiddleName).IsRequired(false);
                b.Property(p => p.LastName).IsRequired(false);
                b.Property(p => p.Gender).IsRequired(false);
                b.Property(p => p.MobileNo).IsRequired(false);
                b.Property(p => p.DisplayUserName).IsRequired(false);
                b.Property(p => p.UserImagePath).IsRequired(false);
                b.Property(p => p.IsSuperAdmin).IsRequired(false);
                b.Property(p => p.Latitude).IsRequired(false);
                b.Property(p => p.Longitude).IsRequired(false);
                b.Property(p => p.Device_UUID).IsRequired(false);
                b.ToTable("tbl_SYS_AspNet_UserInformation");
            });

            modelBuilder.Entity<tbl_SYS_AspNet_UserRoles>(b =>
            {


                b.Property(p => p.UserId).HasColumnName("PK_UserID").HasColumnType("int");
                b.Property(p => p.RoleId).HasColumnName("PK_RoleID").HasColumnType("int");

                b.HasKey(ur => new { ur.UserId, ur.RoleId });

                b.HasOne(ur=>ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                b.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();


                b.ToTable("tbl_SYS_AspNet_UserRoles");
            });

            modelBuilder.Entity<tbl_SYS_AspNet_Roles>(b =>
            {

                b.Property(p => p.Id).HasColumnName("PK_RoleID").HasColumnType("int");
                b.ToTable("tbl_SYS_AspNet_Roles");
            });

            modelBuilder.Entity<tbl_SYS_AspNet_UserClaims>(b =>
            {
               
                b.Property(p => p.Id).HasColumnType("int");
                b.Property(p => p.UserId).HasColumnName("FK_UserId").HasColumnType("int");

                b.HasOne(ur => ur.User)
                   .WithMany(r => r.Claims)
                   .HasForeignKey(ur => ur.UserId)
                   .IsRequired();

                b.ToTable("tbl_SYS_AspNet_UserClaims");
            });

            modelBuilder.Entity<tbl_SYS_AspNet_RoleClaims>(b =>
            {
               
                b.Property(p => p.Id).HasColumnType("int");
                b.Property(p => p.RoleId).HasColumnName("FK_RoleID").HasColumnType("int");
                b.ToTable("tbl_SYS_AspNet_RoleClaims");
                //b.Property(p => p.CompanyID).IsRequired(false);
                //b.Property(p => p.ModuleID).IsRequired(false);
                //b.Property(p => p.FK_MenuID).IsRequired(false);
                //b.Property(p => p.IsShowMenuInGroup).IsRequired(false);
                //b.Property(p => p.Permission_New).IsRequired(false);
                //b.Property(p => p.Permission_Edit).IsRequired(false);
                //b.Property(p => p.Permission_Del).IsRequired(false);
                //b.Property(p => p.Permission_View).IsRequired(false);
                //b.Property(p => p.Permission_Print).IsRequired(false);
                //b.Property(p => p.Permission_Export).IsRequired(false);
                //b.Property(p => p.Permission_Import).IsRequired(false);
                //b.Property(p => p.Permission_Report).IsRequired(false);
                //b.Property(p => p.Permission_Others).IsRequired(false);


            });

            modelBuilder.Entity<tbl_SYS_AspNet_UserLogins>(b =>
            {


                b.Property(p => p.UserId).HasColumnName("FK_UserId").HasColumnType("Int");
                b.ToTable("tbl_SYS_AspNet_UserLogins");

            });

            modelBuilder.Entity<tbl_SYS_AspNet_UserTokens>(b =>
            {

                b.Property<Int32>(p => p.UserId);

                b.ToTable("tbl_SYS_AspNet_UserTokens");
            });





        }


    }
}
