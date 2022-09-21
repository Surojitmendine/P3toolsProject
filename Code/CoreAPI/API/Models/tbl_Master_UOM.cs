using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_Master_UOM
    {
        public int PK_UomID { get; set; }
        public string UOMCode { get; set; }
        public string UOMName { get; set; }
    }
}
