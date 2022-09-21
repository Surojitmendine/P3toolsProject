using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Master_DepotNameMapping
    {
        public int PK_DepotMapID { get; set; }
        public string P3DepotName { get; set; }
        public string SwillDepotName { get; set; }
    }
}
