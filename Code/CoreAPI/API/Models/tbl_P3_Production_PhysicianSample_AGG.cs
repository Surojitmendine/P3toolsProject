using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Production_PhysicianSample_AGG
    {
        public int PK_PhysicianSampleID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? FK_CreatedByID { get; set; }
        public int? ForMonth { get; set; }
        public int? ForYear { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackUnit { get; set; }
        public decimal? PhysicianSampleQTY { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
