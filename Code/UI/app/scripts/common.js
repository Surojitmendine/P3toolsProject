var common = {

    CurrentDatetime: function () {
        return apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Common/CurrentDatetime')
    },
    CurrentDate: function () {
        //console.log(localStorage.getItem('sysdate'))
        if (sessionStorage.getItem('sysdate') == null) {
            return apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Common/CurrentDate')
        }
        else {
            return new Promise((resolve, reject) => {
                resolve({ data: sessionStorage.getItem('sysdate') })
            })
        }

    },

    CurrentTime: function () {
        return apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Common/CurrentTime')
    },

    GetLoggedinUser: function () {


        return new Promise((resolve, reject) => {
            apiCall.ajaxCallWithReturnData({ UserEmail: sessionStorage.getItem("username") }, 'GET', 'Common/GetLoggedinUser')
                .then(response => {
                    $("#spanUserName").empty()
                    $("#pUserName").empty()
                    $("#spanUserName").text(response.data)
                    $("#pUserName").text(response.data)
                    common.EmployeeWiseMenuList(response.empno)
                    return response;
                })
                .then(response => {
                    resolve(response)
                })

        })
    },

    GetUserRoles: function () {
        return apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Common/GetUserRoles')
    },

    /**
     * 
     * @param {number} RoleID 
     *   
     * | RoleID| Name |
        |--|--|
        |1|System Admin|
        |2|Administrator|
        |3|Users|
        |4|Manager|
        |5|Staff|  
     *
     */
    GetUsersByRole: function (RoleID) {
        return apiCall.ajaxCallWithReturnData({ RoleID: RoleID }, 'GET', 'Common/GetUsersByRole')
    },

    MonthList: function () {
        return apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Common/MonthList')
    },

    YearList: function () {
        return apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Common/YearList')
    },

    FillMonthList: function (element) {

        this.MonthList().then((res) => {

            $('#' + element).empty();
            $('#' + element).prepend('<option value="0" selected> Month</option>')
            $('#' + element).select2({
                multiple: false,
                data: res.data,
                closeOnSelect: true,
                theme: 'bootstrap4'
            });
        })
    },

    FillYearList: function (element) {
        this.YearList().then((res) => {
            $('#' + element).empty();
            $('#' + element).prepend('<option value="0" selected> Year </option>')
            $('#' + element).select2({
                multiple: false,
                data: res.data,
                closeOnSelect: true,
                theme: 'bootstrap4'
            });
        })
    },
    // GetAllowedMenusByUser: function () {
    //     return apiCall.ajaxCallWithReturnData(undefined, 'GET', 'UserControl/GetAllowedMenusByUser')

    // },
    // Menu And SubMenu
    EmployeeWiseMenuList: function (EmployeeNo) {
        return new Promise((resolve, reject) => {
            return apiCall.ajaxCallWithReturnData({ EmployeeNo: EmployeeNo }, 'GET', 'common/EmployeeWiseMenuList')
                .then(data => {
                    //console.log(data)
                    let main = data[0].MainMenu;
                    let sub = data[0].SubMenu;
                    let mainID = 0;
                    let divstring = '';
                    $.each(main, function () {
                        mainID = this.MenuSRL;
                        divstring += '<li class="nav-item has-treeview">';
                        divstring += '<a href="#" class="nav-link">';
                        divstring += '<i class="nav-icon fas fa-chart-pie"></i>';
                        divstring += '<p>';
                        divstring += this.MainMenu;
                        divstring += '<i class="right fas fa-angle-left"></i>';
                        divstring += '</p> </a>';
                        divstring += '<ul class="nav nav-treeview">';
                        let submenu = sub.filter(item => item.MenuSRL == mainID);

                        $.each(submenu, function () {
                            //console.log(this.MenuURL)
                            // divstring += '<ul class="nav nav-treeview">';
                            divstring += '<li class="nav-item" data-permissionmenuid="1002">';
                            divstring += '<a href="javascript:void(0)" class="nav-link" data-filepath="' + this.MenuURL + '">';
                            divstring += '<i class="nav-icon fas fa-upload"></i>';
                            divstring += '<p>';
                            divstring += '<span class="badge badge-info left"></span> ' + this.SubMenu + '</p>';
                            divstring += '</a>';
                            divstring += '</li>';

                            // divstring += '</li></ul>';
                        });
                        divstring += '</ul>';
                        
                        divstring += '</li>';

                    });
                    //console.log(divstring)
                    $("#Submenu").append(divstring);
                    divstring = "";
                    // Menu URL Add Dynamically
                    addnavigation();
                });
                
        })
    }

}