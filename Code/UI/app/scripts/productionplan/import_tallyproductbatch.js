var importTallyProductBatch = {

    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');

    },
    UploadExcel_TallyProductBatch: function () {
        // if ($('#ddlYear').val() == "0") {
        //     showToastErrorMessage("Year can not be blank.Select Year and Month")
        //     return false;
        // }
        // if ($('#ddlMonth').val() == "0") {
        //     showToastErrorMessage("Month can not be blank.Select Year and Month")
        //     return false;
        // }
        // var parameters = {
        //     Year: $('#ddlYear').val(),
        //     Month: $('#ddlMonth').val()
        // }

        apiCall.ajaxFileUpload('FileUpload1', 'ProductionPlan/UploadExcel_TallyProductBatch', undefined)
            .then(res => {
                clearDatatable('dtList_TallyProductBatch')
                if (res.success == 1) {
                    importTallyProductBatch.onSuccess_List_TallyProductBatch(res.data)
                }
            })
    },

    List_TallyProductBatch: function () {
        // if ($('#ddlYear').val() == "0") {
        //     showToastErrorMessage("Year can not be blank.Select Year and Month")
        //     return false;
        // }
        // if ($('#ddlMonth').val() == "0") {
        //     showToastErrorMessage("Month can not be blank.Select Year and Month")
        //     return false;
        // }
        // var queryparams = {
        //     Month: $('#ddlMonth').val(),
        //     Year: $('#ddlYear').val(),
        // }
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'ProductionPlan/List_TallyProductBatch').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtList_TallyProductBatch')
                importTallyProductBatch.onSuccess_List_TallyProductBatch(response.data)
            }
        })
    },

    onSuccess_List_TallyProductBatch: function (data) {
        var dtList_TallyProductBatch = $('#dtList_TallyProductBatch').DataTable({
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

                // {
                //     mData: "ID", sTitle: "ID", sClass: "head1", bSortable: false, bVisible: false,
                // },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "UOM", sTitle: "UOM", sClass: "head1", bSortable: true,
                },
                {
                    mData: "BatchSize", sTitle: "Batch Size", sClass: "head1", bSortable: true,
                },
                {
                    mData: "BOMName", sTitle: "BOM Name", sClass: "head1", bSortable: true,
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
                    var tmptable = $('#dtList_TallyProductBatch').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Download Tally Product Batch',
                                extend: 'excel',
                                className: 'btn btn-info float-right ',//fas fa-file-excel
                                title: "TallyProductBatch",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtList_TallyProductBatch').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },


    SaveExcel_TallyProductBatch: function () {
        // if ($('#ddlYear').val() == "0") {
        //     showToastErrorMessage("Year can not be blank.Select Year and Month")
        //     return false;
        // }
        // if ($('#ddlMonth').val() == "0") {
        //     showToastErrorMessage("Month can not be blank.Select Year and Month")
        //     return false;
        // }
        let tabledata = $('#dtList_TallyProductBatch').dataTable().fnGetData();
        
        if (tabledata.length > 0) {
            apiCall.ajaxCall(undefined, 'POST', 'ProductionPlan/SaveExcel_TallyProductBatch', { TallyProductBatch: tabledata })
                .then(res => {
                   
                    if (res.success == true) {
                        $("#FileUpload1").val('')
                        // $('#ddlYear').val('0').trigger('change')
                        // $('#ddlMonth').val('0').trigger('change')
                        //clearDatatable('dtList_TallyProductBatch')

                        showToastSuccessMessage("Tally Product Batch Data Save Sucessfully !!")
                    }
                })
        }
    }
}