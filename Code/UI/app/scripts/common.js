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
            apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Common/GetLoggedinUser')
                .then(response => {
                    $("#spanUserName").empty()
                    $("#pUserName").empty()
                    $("#spanUserName").text(response.data)
                    $("#pUserName").text(response.data)
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
            
            $('#'+element).empty();
            $('#'+element).prepend('<option value="0" selected> Month</option>')
            $('#'+element).select2({
                multiple: false,
                data: res.data,
                closeOnSelect: true,
                theme: 'bootstrap4'
            });
        })       
    },

    FillYearList: function (element) {
        this.YearList().then((res) => {
            $('#'+element).empty();
            $('#'+element).prepend('<option value="0" selected> Year </option>')
            $('#'+element).select2({
                multiple: false,
                data: res.data,
                closeOnSelect: true,
                theme: 'bootstrap4'
            });
        })     
    },
    GetAllowedMenusByUser:function(){       
        return apiCall.ajaxCallWithReturnData(undefined, 'GET', 'UserControl/GetAllowedMenusByUser')
        
    }

}