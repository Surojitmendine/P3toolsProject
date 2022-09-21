var salesforecasting = {
    Initialize: function () {       
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');

        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'SalesForecast/ForcastingDetailsSearchFields').then(response => {
           if (typeof response != typeof undefined) {
               salesforecasting.FillcomboProduct(response.data.products)
               salesforecasting.FillcomboDivision(response.data.divisions)
               salesforecasting.FillcomboDepotName(response.data.DepotNames)
            }
        })
    },

    FillcomboDivision:function(data){
        $('#ddlDivision').empty();
        $('#ddlDivision').select2({
            multiple: true,
            data: data,
            closeOnSelect: true,
            theme: 'bootstrap4',
            placeholder: {
                id: '', // the value of the option
                text: 'Select Division'
            },
        });
},

FillcomboDepotName:function(data){
    $('#ddlDepotName').empty();
    $('#ddlDepotName').select2({
        multiple: true,
        data: data,
        closeOnSelect: true,
        theme: 'bootstrap4',   
        placeholder: {
            id: '', // the value of the option
            text: 'Select Depot'
        },   
    });
},
FillcomboProduct:function(data){
    $('#ddlProductName').empty();
    //$('#ddlProductName').prepend('<option value="" selected> Select Product </option>')
    $('#ddlProductName').select2({
        multiple: true,
        data: data,
        closeOnSelect: true,
        theme: 'bootstrap4',
        placeholder: {
            id: '', // the value of the option
            text: 'Select Product'
        },
    });
},

    ListSalesForecasting: function () {       
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
            Division:$('#ddlDivision').val().join(),
            Depot:$('#ddlDepotName').val().join(),
            Product:$('#ddlProductName').val().join()
        }
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SalesForecast/SalesForecasting').then(response => {
           
            if (typeof response != typeof undefined) {
                clearDatatable('dtListSalesForecasting')
                salesforecasting.onSuccess_ListListSalesForecasting(response.data)
            }
        })
    },

    onSuccess_ListListSalesForecasting: function (data) {       
        var dtListSalesForecasting = $('#dtListSalesForecasting').DataTable({
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
             /*   {
                    mData: "HQ", sTitle: "HQ", sClass: "head1", bSortable: true,
                },*/
                {
                    mData: "DepotName", sTitle: "Depot", sClass: "head1", bSortable: true,
                },
                {
                    mData: "DivisionName", sTitle: "Division", sClass: "head1", bSortable: true,
                },
             /*   {
                    mData: "ProductCode", sTitle: "Product Code", sClass: "head1", bSortable: true,
                },*/
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProjectedQTY", sTitle: "Projected QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ForecastingType", sTitle: "Forecasting Type", sClass: "head1", bSortable: true,
                }
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
                    var tmptable = $('#dtListSalesForecasting').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: ' Export Sales Forecasting',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "SalesForecasting",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtListSalesForecasting').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },
}