using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class salesdata
    {
        public string DIVISION { get; set; }
        public string StockLocation { get; set; }
        public string Billdate { get; set; }
        public string Customercode { get; set; }
        public string Customername { get; set; }
        public string HQ { get; set; }
        public string Productname { get; set; }
        public string Packsize { get; set; }
        public string productcode { get; set; }
        public long? Qtymade { get; set; }
        public long? FreeQty { get; set; }
        public decimal? Billamount { get; set; }
        public decimal? TaxableAmt { get; set; }
    }
}
