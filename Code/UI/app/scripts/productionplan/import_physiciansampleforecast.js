var importPhysicianSampleForecast = {

    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');

    },
    UploadExcel_PhysicianSampleForecast: function () {
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

        apiCall.ajaxFileUpload('FileUpload1', 'ProductionPlan/UploadExcel_PhysicianSampleForecast', parameters)
            .then(res => {
                clearDatatable('dtList_PhysicianSampleForecast')
                if (res.success == 1) {
                    importPhysicianSampleForecast.onSuccess_List_PhysicianSampleForecast(res.data)
                }
            })
    },

    List_PhysicianSampleForecast: function () {
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
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_PhysicianSampleForecast').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtList_PhysicianSampleForecast')
                importPhysicianSampleForecast.onSuccess_List_PhysicianSampleForecast(response.data)
            }
        })
    },

    onSuccess_List_PhysicianSampleForecast: function (data) {
        var dtList_PhysicianSampleForecast = $('#dtList_PhysicianSampleForecast').DataTable({
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
                    mData: "ForecastForMonth", sTitle: "Month", sClass: "head1", bSortable: false,
                },                
                {
                    mData: "ForecastForYear", sTitle: "Year", sClass: "head1", bSortable: false,
                }, 
                {
                    mData: "St_group", sTitle: "Stock Group", sClass: "head1", bSortable: true,
                },
                {
                    mData: "St_category", sTitle: "Stock Category", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Product_Name", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Quantity", sTitle: "Quantity", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Pack", sTitle: "Pack", sClass: "head1", bSortable: true,
                },
                {
                    mData: "UOM", sTitle: "UOM", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Rate", sTitle: "Rate", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Amount", sTitle: "Amount", sClass: "head1", bSortable: true,
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
                    var tmptable = $('#dtList_PhysicianSampleForecast').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Download Physician Sample Forecast',
                                extend: 'excel',
                                className: 'btn btn-info float-right ',//fas fa-file-excel
                                title: "PhysicianSampleForecast",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtList_PhysicianSampleForecast').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },


    SaveExcel_PhysicianSampleForecast: function () {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        let tabledata = $('#dtList_PhysicianSampleForecast').dataTable().fnGetData();
        
        if (tabledata.length > 0) {
            apiCall.ajaxCall(undefined, 'POST', 'ProductionPlan/SaveExcel_PhysicianSampleForecasting', { PhysicianSampleForecasting: tabledata })
                .then(res => {
                   
                    if (res.success == true) {
                        $("#FileUpload1").val('')
                        $('#ddlYear').val('0').trigger('change')
                        $('#ddlMonth').val('0').trigger('change')
                        clearDatatable('dtList_PhysicianSampleForecast')

                        showToastSuccessMessage("Physician Sample Forecast Data Save Sucessfully !!")
                    }
                })
        }
    }
}