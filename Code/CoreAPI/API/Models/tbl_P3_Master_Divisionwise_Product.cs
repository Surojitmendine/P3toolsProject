using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Master_Divisionwise_Product
    {
        public int PK_ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string DivisionName { get; set; }
        public string DepotName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public bool? IsActive { get; set; }
    }
}
