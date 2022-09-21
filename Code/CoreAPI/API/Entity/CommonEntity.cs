using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public class CommonEntity
    {
        public class MasterCurrencyDenomination
        {
            public int PK_MasterCurrencyDenomID { get; set; }
            /*public int? FK_UserID_CreatedBy { get; set; }
            public int? FK_DateKey_CreatedOn { get; set; }
            public int? FK_TimeKey_CreatedOn { get; set; }
            public bool? IsDeleted { get; set; }
            public bool? IsActive { get; set; }
            public int? FK_UserID_ModifiedBy { get; set; }
            public int? FK_DateKey_ModifiedOn { get; set; }
            public int? FK_TimeKey_ModifiedOn { get; set; }
            public int? FK_CompanyID { get; set; }
            public int? FK_BranchID { get; set; }*/
            public decimal? CurrencyDenomination { get; set; }
            public bool? IsNote { get; set; }

            //Extra Params
            public Int32 NoofCurrency { get; set; }

        }
    }
}
