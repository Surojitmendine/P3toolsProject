var RoleMapping = {

    Initialize: function () {
        RoleMapping.FillRoleList();
        RoleMapping.FillActiveEmployeeList();
        RoleMapping.ListEmployeeRoleMapping();
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
            RoleMapping.Initialize()
        })

    },
    

    ListEmployeeRoleMapping: function () {
        clearDatatable('dtEmployeeRoleMappinglist')
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Common/EmployeeRoleMappingList')
        .done(function (response) {
            clearDatatable('dtEmployeeRoleMappinglist')
            if (typeof response !== typeof undefined) {
                RoleMapping.onSuccess_ListEmployeeRoleMapping(response)
            }
    
        })
    },


    onSuccess_ListEmployeeRoleMapping: function (response) {        
        var dtEmployeeRoleMappinglist = $('#dtEmployeeRoleMappinglist').DataTable({
            bServerSide: false,
            bDestroy: true,
            paging: true,
            autoWidth: false,
            bStateSave: false,
            searching: true,
            data: response.data,
            language: {
                paginate: {
                    previous: "<",
                    next: ">"
                },
                info: "Showing _START_ - _END_ of _TOTAL_ Employee Role Mapping",
            },
            aoColumns: [
                {
                    mData: "EmpNo", sTitle: "Employee No", sClass: "head1", bSortable: true, bVisible: true,
                }, 
                {
                    mData: "EmployeeName", sTitle: "Employee Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PostName", sTitle: "Post", sClass: "head1", bSortable: true,
                },

                {
                    mData: "DepartmentName", sTitle: "Department", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Role", sTitle: "Role", sClass: "head1", bSortable: true,
                },                
                {
                    mData: "Remarks", sTitle: "Remarks", sClass: "head1", bSortable: true,
                }
            ],

            bUseRendered: true,
            sPaginationType: "simple_numbers",
            aaSorting: [],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],

            fnDrawCallback: function (oSettings) {
                var info = this.api().page.info();
                if (info.recordsTotal > 0) {
                    var tmptable = $('#dtEmployeeRoleMappinglist').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Export',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "EmployeeRoleMappingList",
                                extension: '.xls|.xlsx',
                                exportOptions: {
                                    columns: [0, 1, 2, 3, 4, 5]
                                },
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtEmployeeRoleMappinglist').DataTable();
                    tmptable.buttons().destroy();
                }

            }
        });

    },

}