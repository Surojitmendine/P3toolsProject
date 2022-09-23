using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class factory_closing_stock
    {
        public string CompanyId { get; set; }
        public string Stock_date { get; set; }
        public string St_group { get; set; }
        public string St_category { get; set; }

        public string product_name { get; set; }
        public Decimal quantity { get; set; }
        public string UOM { get; set; }
        public Decimal rate { get; set; }
        public Decimal amount { get; set; }
    }
}
