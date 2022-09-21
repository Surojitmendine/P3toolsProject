var salesforecastingComparison = {
    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'SalesForecast/ForcastingComparison_SearchFields').then(response => {
            if (typeof response != typeof undefined) {
                salesforecastingComparison.FillcomboProduct(response.data.products)
                salesforecastingComparison.FillcomboDivision(response.data.divisions)
                salesforecastingComparison.FillcomboStockLocation(response.data.stocklocations)
                salesforecastingComparison.FillcomboPackUnit(response.data.packunits)
            }
        })
    },

    FillcomboProduct: function (data) {
        $('#ddlProductName').empty();
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

    FillcomboDivision: function (data) {
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

    FillcomboStockLocation: function (data) {
        $('#ddlStockLocation').empty();
        $('#ddlStockLocation').select2({
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
    FillcomboPackUnit: function (data) {
        $('#ddlPackUnit').empty();
        $('#ddlPackUnit').select2({
            multiple: true,
            data: data,
            closeOnSelect: true,
            theme: 'bootstrap4',
            placeholder: {
                id: '', // the value of the option
                text: 'Select Packunit'
            },
        });
    },

    Search: function () {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank. PLease Select Year and Month.")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank. Please Select Year and Month.")
            return false;
        }
        var queryparams = {
            Month: $('#ddlMonth').val(),
            Year: $('#ddlYear').val(),
            Product: $('#ddlProductName').val().join(),
            Division: $('#ddlDivision').val().join(),
            Location: $('#ddlStockLocation').val().join(),
            PackUnit: $('#ddlPackUnit').val().join()
        }
        clearDatatable('dtListSalesForecastingComparison_Summary')
        clearDatatable('dtListSalesForecastingComparison')

        $.when(
            apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SalesForecast/SalesForecastingComparison_Summary'),
            apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SalesForecast/SalesForecastingComparison'),
        ).done((Comparison_Summary, Comparison_Details) => {
            let hasdata = false
            if (typeof Comparison_Summary[0] != typeof undefined) {
                hasdata = true
                salesforecastingComparison.onSuccess_ListSalesForecastingComparison_Summary(Comparison_Summary[0].data)
            }

            if (typeof Comparison_Details[0] != typeof undefined) {
                hasdata = true
                salesforecastingComparison.onSuccess_ListSalesForecastingComparison(Comparison_Details[0].data)
            }
            if (hasdata == true) {
                $('#btnExport').show()              
            }
        })

    },

    export: function () {
        let Comparison_Summary = $('#dtListSalesForecastingComparison_Summary').dataTable().fnGetData();
        let Comparison_Details = $('#dtListSalesForecastingComparison').dataTable().fnGetData();

        let dataArr=[]
        let sheetArr=[]
        if(Comparison_Summary.length>0){
            dataArr.push(Comparison_Summary)
            sheetArr.push('SalesForecasting Comparison Summary')
        }
        if(Comparison_Details.length>0){
            dataArr.push(Comparison_Details)
            sheetArr.push('Sales Forecasting Details')
        }
        tablesToExcel.exportJson(dataArr,sheetArr, 'SalesForecastingComparison.xls', 'Excel')
    },

    onSuccess_ListSalesForecastingComparison_Summary: function (data) {
        var dtList_SalesForecastingComparison_Summary = $('#dtListSalesForecastingComparison_Summary').DataTable({
            bServerSide: false,
            bDestroy: true,
            paging: true,
            autoWidth: true,
            bStateSave: false,
            searching: false,
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
                    mData: "DepotName", sTitle: "Depot", sClass: "head1", bSortable: true,
                },
                {
                    mData: "DivisionName", sTitle: "Division", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductCode", sTitle: "Product Code", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Logistics_ProjectionSalesQTY", sTitle: "Logistics", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Marketing_ProjectedSaleQTY", sTitle: "Marketing", sClass: "head1", bSortable: true,
                },
                {
                    mData: "DifferencePersentage", sTitle: "Difference(%)", sClass: "head1", bSortable: true,
                    mRender:function(data){
                        return data+"%"
                    }
                },
                {
                    mData: "NextMonth_ForecastingQTY", sTitle: "Forecasting", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NextMonth_FinialForecastingQTY", sTitle: "Final Forecasting", sClass: "head1", bSortable: true,
                },
            ],

            bUseRendered: true,
            sPaginationType: "simple_numbers",
            aaSorting: [],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],           
        });
    },

    onSuccess_ListSalesForecastingComparison: function (data) {
        var dtList_SalesForecastingComparison = $('#dtListSalesForecastingComparison').DataTable({
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
                    mData: "Logistics_ProjectionSalesQTY", sTitle: "Logistics", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Marketing_ProjectedSaleQTY", sTitle: "Marketing", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Sales_ProjectedSaleQTY", sTitle: "Sales", sClass: "head1", bSortable: true,
                },
                {
                    mData: "DifferencePersentage", sTitle: "Difference(%)", sClass: "head1", bSortable: true,
                    mRender:function(data){
                        return data+"%"
                    }
                },
                {
                    mData: "NextMonth_FinialForecastingQTY", sTitle: "Final Forecasting", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NRVRate", sTitle: "NRV Rate", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProjectionValue", sTitle: "ProjectionValue", sClass: "head1", bSortable: true,
                },                   

                {
                    mData: "IsAutoCalculate", defaultContent: '', sTitle: "", sClass: "head1", bSortable: false,
                    mRender: function (data, type, row, meta) {
                        if (data == false) {

                            var markup = '<a  href="javascript:void(0)" onclick="salesforecastingComparison.onRowEditClick(this)">'
                            markup += '      <i class="fas fa-edit" aria-hidden="true"></i>'
                            markup += '   </a>'
                            markup += '<a  href="javascript:void(0)" onclick="salesforecastingComparison.onRowSaveClick(this,' + row.SaleComparisonID + ')" style="display:none">'
                            markup += '      <i class="fas fa-save" aria-hidden="true"></i>'
                            markup += '   </a>'
                            markup += '<a  href="javascript:void(0)" onclick="salesforecastingComparison.onRowUndoClick(this)" style="display:none">'
                            markup += '      <i class="fas fa-undo-alt" aria-hidden="true"></i>'
                            markup += '   </a>'
                            return markup;
                        }
                    }
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
                    var tmptable = $('#dtListSalesForecastingComparison').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: ' Export Sales Forecasting Comparison',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "Sales Forecasting Comparison",
                                extension: '.xls',
                                /*exportOptions: {
                                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
                                },*/
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtListSalesForecastingComparison').DataTable();
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
              let  Logistics_ProjectionSalesQTY = api
                    .column(5)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

             let  Marketing_ProjectedSaleQTY = api
                    .column(6)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

            let  Sales_ProjectedSaleQTY = api
                    .column(7)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);
                    
            let  NextMonth_FinialForecastingQTY = api
                    .column(9)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

            let  ProjectionValue = api
                    .column(11)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);        

                $(this).find('tfoot').remove()
                $(this).append('<tfoot><tr></tr></tfoot>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th  class="text-left">Total Sale Projection QTY - </th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + Logistics_ProjectionSalesQTY.toFixed(2) + '</th>');                
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">'+ Marketing_ProjectedSaleQTY.toFixed(2) +'</th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + Sales_ProjectedSaleQTY.toFixed(2) + '</th>'); 
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + NextMonth_FinialForecastingQTY.toFixed(2) + '</th>'); 
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + ProjectionValue.toFixed(2) + '</th>'); 
            },
              

            createdRow: function( row, data, dataIndex ) {
                if ( parseFloat(data.DifferencePersentage) >20 ) {
                  $(row).css({"background-color": '#ffcccb' });
                }
              }
        });
    },


    onRowEditClick(element, row) {
        $(element).hide()
        $(element).nextAll('a').show()
        let $row = $(element).closest("tr");
        let $td = $row.find("td:eq(9)");
        let txt = $td.text();
        $td.html("").append("<input type='text' value=\"" + txt + "\">");
    },

    onRowSaveClick: function (element, SaleComparisonID) {
        let $row = $(element).closest("tr");
        let $td = $row.find("td:eq(9)");
        let txt = $td.find("input").val()

        let $td1 = $row.find("td:eq(11)");
        let txtNRV = $row.find("td:eq(10)").text();        
        console.log(txt)

        apiCall.ajaxCall(undefined, 'POST', 'SalesForecast/SalesForecastingComparisonSave',
            { SaleComparisonID: SaleComparisonID, NextMonth_FinialForecastingQTY: txt, NRVRate:txtNRV  }
        ).then(res => {
            if (res.success == 1) {
                console.log(res)
                salesforecastingComparison.ListSalesForecastingComparison(true)
            }
        })
            .done(() => {
                alert(1)
                $(element).hide()
                $(element).prev('a').show()
                bootbox.alert("Sales Forecasting Comparison added.");
                // $td.html(txt)
            })
    },

    onRowUndoClick: function (element) {
        $(element).hide()
        $(element).prev('a').hide()
        let firsta = $(element).parent().find('a')[0]
        $(firsta).show()
        let $row = $(element).closest("tr");
        let $td = $row.find("td:eq(9)");
        let txt = $td.find("input").val()
        $td.html(txt)
    }
}