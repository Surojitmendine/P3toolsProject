var datatransfer = {

    transferData: function (sptocall) {

        var ajaxdata = {
            FromDate: $('#dtFromdate').val(),
            ToDate: $('#dtTodate').val(),
            SpToCall:sptocall
        };

        apiCall.ajaxCall(undefined, 'GET', 'SetupProcess/DataTransfer',ajaxdata)
        .then((res)=>{
            showToastSuccessMessage(res.data)
        })
    },

    List_PrimarySales: function () {
        var queryparams = {
            FromDate: $('#dtFromdate').val(),
            ToDate: $('#dtTodate').val()
        };

        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SalesForecast/List_PrimarySales').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtList_PrimarySales')
                datatransfer.onSuccess_List_PrimarySales(response.data)
            }
        })
    },

    onSuccess_List_PrimarySales: function (data) {
   // let month =$('#ddlMonth').select2('data')[0].text
  //  let year =$('#ddlYear').select2('data')[0].text
        var dtList_PrimarySales = $('#dtList_PrimarySales').DataTable({
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
                    mData: "ID", sTitle: "ID", sClass: "head1", bSortable: false, bVisible:false,
                },   
                {
                    mData: "SaleDate", sTitle: "Sale Date", sClass: "head1", bSortable: true,
                }, 
                {
                    mData: "Month", sTitle: "Month", sClass: "head1", bSortable: true,
                },   
                {
                    mData: "DivisionName", sTitle: "Division", sClass: "head1", bSortable: true,
                },           
                {
                    mData: "DepotName", sTitle: "Depot", sClass: "head1", bSortable: true,
                },
                {
                    mData: "HQ", sTitle: "HQ", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductCode", sTitle: "Product Code", sClass: "head1", bSortable: true, bVisible:true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true, 
                },
                {
                    mData: "SalesQTY", sTitle: "Sales Qty", sClass: "head1", bSortable: true,
                },
                {
                    mData: "FreeSampleQTY", sTitle: "Free Qty", sClass: "head1", bSortable: true, 
                },
                {
                    mData: null, sTitle: "Total QTY", sClass: "text-center head1", bSortable: true, bVisible: true,
                    mRender: function (data, type, full) {
                        return full.FreeSampleQTY  + full.SalesQTY
                    }
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
                    var tmptable = $('#dtList_PrimarySales').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Download Primary Sales',
                                extend: 'excel',
                                className: 'btn btn-info float-right ',//fas fa-file-excel
                                title: "Primary Sales",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtList_PrimarySales').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    }
}