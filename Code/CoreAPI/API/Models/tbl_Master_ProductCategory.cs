using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_Master_ProductCategory
    {
        public tbl_Master_ProductCategory()
        {
            tbl_Master_Product = new HashSet<tbl_Master_Product>();
        }

        public int PK_ProductCategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<tbl_Master_Product> tbl_Master_Product { get; set; }
    }
}
