using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class clsRoleMappingInfo
    {
        public String EmployeeNo { get; set; } = "";
        public long IDRole { get; set; } = 0;
    }
    public class clsP3RoleInfo
    {
        public long IDRole { get; set; } = 0;
        public long IDApplication { get; set; } = 0;
        public String Name { get; set; } = "";
        public String Description { get; set; } = "";

    }
}
