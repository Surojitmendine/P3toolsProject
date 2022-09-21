using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class closingstock
    {
        public string DIVISION { get; set; }
        public string DEPOT { get; set; }
        public string STK { get; set; }
        public string PRODUCTNAME { get; set; }
        public string PACK { get; set; }
        public string BATCH { get; set; }
        public string MFGDATE { get; set; }
        public string EXPDATE { get; set; }
        public int? CLOSINGBALANCE { get; set; }
        public decimal? NRV { get; set; }
        public decimal? STOCKVALUE { get; set; }
        public int? DAYSEXP { get; set; }
        public string TYPE { get; set; }
        public long? DAYSFRMANUFAC { get; set; }
        public long? WEIGTDAYS { get; set; }
    }
}
