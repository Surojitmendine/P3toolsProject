var RoleMapping = {

    Initialize: function () {
        RoleMapping.FillRoleList();
        RoleMapping.FillActiveEmployeeList();
    },

    FillRoleList: function(){
        apiCall.ajaxCall(undefined, 'GET', 'Common/P3RoleList',undefined)
        .then((res)=>{
            $('#ddlRole').empty();
            $("#ddlRole").append("<option value=0>Select Role</option>");
           
            $.each(res.data, function (index, value) {
                 //console.log(this.Name);
                $("#ddlRole").append("<option value=" + this.IDRole + ">" + this.Name + "</option>");
            });  
            
            $("#ddlRole").select2({
                multiple: false,
                closeOnSelect: true,
                theme: 'bootstrap4'
            });
        })

    },

    FillActiveEmployeeList: function(){
        apiCall.ajaxCall(undefined, 'GET', 'Common/EmployeeList',undefined)
        .then((res)=>{
            $('#ddlEmployee').empty();
            $("#ddlEmployee").append("<option value=0>Select Employee</option>");
           
            $.each(res.data, function (index, value) {
                 //console.log(this.Empname);
                $("#ddlEmployee").append("<option value=" + this.Empno + ">" + this.Empname + "</option>");
            });  
            
            $("#ddlEmployee").select2({
                multiple: false,
                closeOnSelect: true,
                theme: 'bootstrap4'
            });
        })

    },
    
    AddEmployeeWiseRoleMapping: function () {

        var ajaxdata = {
            EmployeeNo: $('#ddlEmployee').val(),
            IDRole: $('#ddlRole').val()
        };
        console.log(ajaxdata);

        apiCall.ajaxCall(undefined, 'POST', 'Common/EmployeeRoleMappingSave',ajaxdata)
        .then((res)=>{
            console.log(res.success + ',' + res.message);
            if(res.success == 1){
                showToastSuccessMessage(res.message);
            }
            else{
                showToastErrorMessage(res.data);
            }
        })

        
    }

}