var syncclosingstock = {
    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
    },

    SyncClosingStock: function (sptocall) {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        var ajaxdata = {
            FromDate: $('#dtFromdate').val(),
            Month: $('#ddlMonth').val(),
            Year: $('#ddlYear').val()
        };

        apiCall.ajaxCall(undefined, 'GET', 'SetupProcess/SyncClosingStockData',ajaxdata)
        .then((res)=>{
            showToastSuccessMessage(res.data)
        })
    },

    List_ClosingStock: function () {
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
        };

        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SetupProcess/List_ClosingStock').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtList_ClosingStock')
                syncclosingstock.onSuccess_List_ClosingStock(response.data)
            }
        })
    },

    onSuccess_List_ClosingStock: function (data) {
        var dtList_ClosingStock = $('#dtList_ClosingStock').DataTable({
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
                    mData: "PK_ID", sTitle: "ID", sClass: "head1", bSortable: false, bVisible:false,
                },   
                {
                    mData: "SyncDate", sTitle: "Sync Date", sClass: "head1", bSortable: true,
                }, 
                {
                    mData: "ForYear", sTitle: "Year", sClass: "head1", bSortable: true,
                }, 
                {
                    mData: "ForMonth", sTitle: "Month", sClass: "head1", bSortable: true,
                },   
                {
                    mData: "DivisionName", sTitle: "Division", sClass: "head1", bSortable: true,
                },           
                {
                    mData: "DepotName", sTitle: "Depot", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true, 
                },
                {
                    mData: "ClosingStockQTY", sTitle: "Closing QTY", sClass: "head1", bSortable: true,
                },                              
            ],

            bUseRendered: true,
            sPaginationType: "simple_numbers",
            aaSorting: [],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[100, 200, 300, -1], [100, 200, 300, "All"]],

            fnDrawCallback: function (oSettings) {
                var info = this.api().page.info();
                if (info.recordsTotal > 0) {
                    var tmptable = $('#dtList_ClosingStock').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Download Closing Stock',
                                extend: 'excel',
                                className: 'btn btn-info float-right ',//fas fa-file-excel
                                title: "ClosingStock",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtList_ClosingStock').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    }
}