var saleforecastsummary = {
    Initialize: function () {
        common.MonthList().then((res) => {
            saleforecastsummary.FillMonthList(res.data)
        })
        common.YearList().then((res) => {
            saleforecastsummary.FillYearList(res.data)
        })
    },

    FillMonthList: function (data) {
        $('#ddlMonth').empty();
        $('#ddlMonth').prepend('<option value="0" selected> Select Month </option>')
        $('#ddlMonth').select2({
            multiple: false,
            data: data,
            closeOnSelect: true,
            theme: 'bootstrap4'
        });
    },

    FillYearList: function (data) {
        $('#ddlYear').empty();
        $('#ddlYear').prepend('<option value="0" selected> Select Year </option>')
        $('#ddlYear').select2({
            multiple: false,
            data: data,
            closeOnSelect: true,
            theme: 'bootstrap4'
        });
    },

    ListSalesForecastSummary: function () {
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
       
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SalesForecast/SalesForecastSummary').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtSalesForecastSummary')
                saleforecastsummary.onSuccess_ListSalesForecastSummary(response.data)
            }
        })
    },

    onSuccess_ListSalesForecastSummary: function (data) {
        var dtSalesForecastSummary = $('#dtSalesForecastSummary').DataTable({
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
                    mData: "Month", sTitle: "Month", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Next_ProjectionSalesQTY", sTitle: "Projection QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NRVRate", sTitle: "NRV Rate", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProjectionValue", sTitle: "ProjectionValue", sClass: "head1", bSortable: true,
                },                      
            ],

            bUseRendered: true,
            sPaginationType: "simple_numbers",
            aaSorting: [],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[10, 20, 30, -1], [10, 20, 30, "All"]],

            fnDrawCallback: function (oSettings) {
                var info = this.api().page.info();
                if (info.recordsTotal > 0) {
                    var tmptable = $('#dtSalesForecastSummary').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Export Sales Forecast Summary',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "SalesForecastSummary",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtSalesForecastSummary').DataTable();
                    tmptable.buttons().destroy();
                }
            },

            footerCallback: function (row, data, start, end, display) {
                var api = this.api(), data;
                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i : 0;
                };
                // Total over all pages
              let  Next_ProjectionSalesQTY = api
                    .column(3)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                // Total over all pages
                let  Next_ProjectionValue = api
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

                $(this).find('tfoot').remove()
                $(this).append('<tfoot><tr></tr></tfoot>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th  class="text-left">Total Sales Projection QTY :</th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + Next_ProjectionSalesQTY.toFixed(2) + '</th>');                

                $(this).find("tfoot > tr:nth-child(1)").append('<th  class="text-left">Projection Value :</th>');                
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + Next_ProjectionValue.toFixed(2) + '</th>');                

            },            
        });
    },
}