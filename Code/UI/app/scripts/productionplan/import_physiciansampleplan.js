var importPhysicianSamplePlan = {

    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');

    },
    UploadExcel_PhysicianSamplePlan: function () {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        var parameters = {
            Year: $('#ddlYear').val(),
            Month: $('#ddlMonth').val()
        }

        apiCall.ajaxFileUpload('FileUpload1', 'ProductionPlan/UploadExcel_PhysicianSamplePlan', parameters)
            .then(res => {
                clearDatatable('dtList_PhysicianSamplePlan')
                if (res.success == 1) {
                    importPhysicianSamplePlan.onSuccess_List_PhysicianSamplePlan(res.data)
                }
            })
    },

    List_PhysicianSamplePlan: function () {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        var queryparams = {
            Month: $('#ddlMonth').val(),
            Year: $('#ddlYear').val(),
        }
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_PhysicianSamplePlan').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtList_PhysicianSamplePlan')
                importPhysicianSamplePlan.onSuccess_List_PhysicianSamplePlan(response.data)
            }
        })
    },

    onSuccess_List_PhysicianSamplePlan: function (data) {
        var dtList_PhysicianSamplePlan = $('#dtList_PhysicianSamplePlan').DataTable({
            bServerSide: false,
            bDestroy: true,
            paging: true,
            autoWidth: false,
            bStateSave: false,
            searching: true,
            data: data,

            language: {
                paginate: {
                    previous: "<",
                    next: ">"
                },
                info: "Showing _START_ - _END_ of _TOTAL_ readings",
            },
            aoColumns: [

                {
                    mData: "ID", sTitle: "ID", sClass: "head1", bSortable: false, bVisible: false,
                },
                {
                    mData: "MonthName", sTitle: "For Month", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PhysicianSampleQTY", sTitle: "Units", sClass: "head1", bSortable: true,
                },
            ],

            bUseRendered: true,
            sPaginationType: "simple_numbers",
            aaSorting: [],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, "All"]],

            fnDrawCallback: function (oSettings) {
                var info = this.api().page.info();
                if (info.recordsTotal > 0) {
                    var tmptable = $('#dtList_PhysicianSamplePlan').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Download Physician Sample Plan',
                                extend: 'excel',
                                className: 'btn btn-info float-right ',//fas fa-file-excel
                                title: "PhysicianSamplePlan",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtList_PhysicianSamplePlan').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },


    SaveExcel_PhysicianSamplePlan: function () {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        let tabledata = $('#dtList_PhysicianSamplePlan').dataTable().fnGetData();
        
        if (tabledata.length > 0) {
            apiCall.ajaxCall(undefined, 'POST', 'ProductionPlan/SaveExcel_PhysicianSamplePlan', { sampledata: tabledata,year:$('#ddlYear').val(),month:$('#ddlMonth').val() })
                .then(res => {
                   
                    if (res.success == true) {
                        $("#FileUpload1").val('')
                        $('#ddlYear').val('0').trigger('change')
                        $('#ddlMonth').val('0').trigger('change')
                        clearDatatable('dtList_PhysicianSamplePlan')

                        showToastSuccessMessage("Physician SamplePlan Data Save Sucessfully !!")
                    }
                })
        }
    }
}