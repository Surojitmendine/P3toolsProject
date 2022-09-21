using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public class UserEntity
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DisplayUserName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public decimal? SalaryMonthly { get; set; }
    }
}
