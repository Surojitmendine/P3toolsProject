using API.Models;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.BusinessLogic
{
    public class clsLogin
    {
        public static DataTable Login_User_Information(String Connection, String UserEmail)
        {
            return clsDatabase.fnDataTable(Connection, "PRC_Login_User_Information", UserEmail);
        }

        public static List<clsUserMenuInfo> Employee_Wise_Menu_List(String Con, String EmployeeNo)
        {
            List<clsUserMainMenuInfo> mainmenu = new List<clsUserMainMenuInfo>();
            List<clsUserSubMenuInfo> submenu = new List<clsUserSubMenuInfo>();
            List<clsUserMenuInfo> menus = new List<clsUserMenuInfo>();
            DataSet DS = clsDatabase.fnDataSet(Con, "P3TOOLS_PRC_Employee_Wise_Menu", EmployeeNo);

            foreach (DataRow Dr in DS.Tables[0].Rows)
            {
                clsUserMainMenuInfo mobj = new clsUserMainMenuInfo();
                mobj.MenuSRL = clsHelper.fnConvert2Long(Dr["MenuSRL"]);
                mobj.MainMenu = Dr["MainMenu"].ToString();
                mainmenu.Add(mobj);
            }
            foreach (DataRow Dr in DS.Tables[1].Rows)
            {
                clsUserSubMenuInfo sobj = new clsUserSubMenuInfo();
                sobj.MenuSRL = clsHelper.fnConvert2Long(Dr["MenuSRL"]);
                sobj.MainMenu = Dr["MainMenu"].ToString();
                sobj.SubMenu = Dr["SubMenu"].ToString();
                sobj.MenuURL = Dr["MenuURL"].ToString();
                sobj.MenuIcon = Dr["MenuIcon"].ToString();
                submenu.Add(sobj);
            }
            menus.Add(new clsUserMenuInfo { MainMenu = mainmenu, SubMenu = submenu });
            return menus;
        }

        public static List<clsP3RoleInfo> P3RoleList(String Connection)
        {
            List<clsP3RoleInfo> mlist = new List<clsP3RoleInfo>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "P3_PRC_Role_List");
            foreach (DataRow DR in DT.Rows)
            {
                mlist.Add(new clsP3RoleInfo()
                {
                    IDRole = clsHelper.fnConvert2Long(DR["IDRole"]),
                    IDApplication = clsHelper.fnConvert2Long(DR["IDApplication"]),
                    Name = DR["Name"].ToString(),
                    Description = DR["Remarks"].ToString()
                });
            }
            return mlist;
        }

        public static List<clsEmployeeInfo> EmployeeList(String Connection)
        {
            List<clsEmployeeInfo> mlist = new List<clsEmployeeInfo>();
            DataTable DT = clsDatabase.fnDataTable(Connection, "P3_PRC_Active_Employee_List");
            foreach (DataRow DR in DT.Rows)
            {
                mlist.Add(new clsEmployeeInfo()
                {
                    Empno = DR["Empno"].ToString(),
                    Empname = DR["DisplayName"].ToString(),
                });
            }
            return mlist;
        }

        public static String EmployeeRoleMappingSave(String Connection, String EmployeeNo, long IDRole)
        {
            return clsDatabase.fnDBOperation(Connection, "P3_PRC_Employee_Role_Mapping_Save", EmployeeNo, IDRole);
        }

    }
}
