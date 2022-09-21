using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public class SampleEntity
    {
        public int PK_AutoDemoSample { get; set; }
        public bool? IsActive { get; set; }
        public string Auto50_TextBoxVarchar { get; set; }
        public string EmailIDTextbox { get; set; }
        public string TextBoxVarchr150Auto { get; set; }
        public int? FK_CountryID { get; set; }
        public string TextAreaAuto { get; set; }
        public string TinyMceEditor { get; set; }
        public string ContactNo { get; set; }
        public bool? YesNo_Radiobutton { get; set; }
        public int? Textbox_Numeric { get; set; }
        public decimal? Textbox_Decimal { get; set; }
        public Nullable<DateTime> DateTimePicker { get; set; }
        public bool? CheckBox_Bit { get; set; }
        public string DD_DropdownHardcore { get; set; }
        public int? FK_UserGroupID_SingleDropdown { get; set; }
        public int? FK_StateID { get; set; }
        public string FK_UserGroupID_Dropdown_MultiSelect { get; set; }

    }
}
