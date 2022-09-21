using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class tbl_P3_Master_CalculationFormula
    {
        public int PK_ID { get; set; }
        public string FormulaType { get; set; }
        public string Condition1 { get; set; }
        public string Condition2 { get; set; }
        public decimal? FormulaValue { get; set; }
        public string Remarks { get; set; }
        public string ForecastingType { get; set; }
    }
}
