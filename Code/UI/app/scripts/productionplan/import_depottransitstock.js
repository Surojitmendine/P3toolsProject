var importDepotTransitStock = {

    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');

    },
    UploadExcel_DepotTransitStock: function () {
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

        apiCall.ajaxFileUpload('FileUpload1', 'ProductionPlan/UploadExcel_DepotTransitStock', parameters)
            .then(res => {
                clearDatatable('dtList_DepotTransitStock')
                if (res.success == 1) {
                    importDepotTransitStock.onSuccess_List_DepotTransitStock(res.data)
                }
            })
    },

    List_DepotTransitStock: function () {
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
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_DepotTransitStock').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtList_DepotTransitStock')
                importDepotTransitStock.onSuccess_List_DepotTransitStock(response.data)
            }
        })
    },

    onSuccess_List_DepotTransitStock: function (data) {
        var dtList_DepotTransitStock = $('#dtList_DepotTransitStock').DataTable({
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
                    mData: "Stock_date", sTitle: "Stock Date", sClass: "head1", bSortable: false,
                },                
                {
                    mData: "St_group", sTitle: "Stock Group", sClass: "head1", bSortable: true,
                },
                {
                    mData: "St_category", sTitle: "Stock Category", sClass: "head1", bSortable: true,
                },
                {
                    mData: "product_name", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "quantity", sTitle: "Quantity", sClass: "head1", bSortable: true,
                },
                {
                    mData: "UOM", sTitle: "UOM", sClass: "head1", bSortable: true,
                },
                {
                    mData: "rate", sTitle: "Rate", sClass: "head1", bSortable: true,
                },
                {
                    mData: "amount", sTitle: "Amount", sClass: "head1", bSortable: true,
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
                    var tmptable = $('#dtList_DepotTransitStock').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Download Depot Transit Stock',
                                extend: 'excel',
                                className: 'btn btn-info float-right ',//fas fa-file-excel
                                title: "DepotTransitStock",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtList_DepotTransitStock').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },


    SaveExcel_DepotTransitStock: function () {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        let tabledata = $('#dtList_DepotTransitStock').dataTable().fnGetData();
        
        if (tabledata.length > 0) {
            apiCall.ajaxCall(undefined, 'POST', 'ProductionPlan/SaveExcel_DepotTransitStock', { DepotTransitStock: tabledata })
                .then(res => {
                   
                    if (res.success == true) {
                        $("#FileUpload1").val('')
                        $('#ddlYear').val('0').trigger('change')
                        $('#ddlMonth').val('0').trigger('change')
                        clearDatatable('dtList_DepotTransitStock')

                        showToastSuccessMessage("Depot Transit Stock Data Save Sucessfully !!")
                    }
                })
        }
    }
}