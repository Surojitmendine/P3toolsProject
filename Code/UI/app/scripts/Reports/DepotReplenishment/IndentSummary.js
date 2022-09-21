var IndentSummary = {
    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'AnallyticalReports/Reports_SearchFields').then(response => {
            if (typeof response != typeof undefined) {
                IndentSummary.FillcomboDivision(response.data.division)
                IndentSummary.FillcomboDepot(response.data.depot)
                IndentSummary.FillcomboProduct(response.data.product)                            
                IndentSummary.FillcomboPackUnit(response.data.packunit)
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

    FillcomboDepot: function (data) {
        $('#ddlDepot').empty();
        $('#ddlDepot').select2({
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
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        var queryparams = {
            ForMonth: $('#ddlMonth').val(),
            ForYear: $('#ddlYear').val(),
            Divisions: $('#ddlDivision').val().join(),
            Depots: $('#ddlDepot').val().join(),
            Products: $('#ddlProductName').val().join(),                    
            PackUints: $('#ddlPackUnit').val().join()
        }
        

        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'AnallyticalReports/DepotReplenishment_IndentSummary').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtListIndentSummary')
                IndentSummary.onSuccess_ListIndentSummary(response.data)
            }
        })
    },

    onSuccess_ListIndentSummary: function (data) {
        var dtSalesForecastSummary = $('#dtListIndentSummary').DataTable({
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
               //     mData: "ForYear", sTitle: "Year", sClass: "head1", bSortable: true, 
               // },                
               // {
               //     mData: "ForMonth", sTitle: "Month", sClass: "head1", bSortable: true,
               // },   
               {
                    mData: null, sTitle: "Month", sClass: "text-center head1", bSortable: true, bVisible: true,
                    mRender: function (data, type, full) {
                        return full.ForMonth + '-' + full.ForYear
                    }
                },            
                {
                    mData: "DivisionName", sTitle: "Division", sClass: "head1", bSortable: true,
                },
                {
                    mData: "DepotName", sTitle: "Depot", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "FinalProjection", sTitle: "Final Projection", sClass: "head1", bSortable: true,
                },                
                {
                    mData: "ClosingStock", sTitle: "Closing Stock", sClass: "head1", bSortable: true,
                },
                {
                    mData: "LTFactor", sTitle: "LT Factor", sClass: "head1", bSortable: true,
                },                
                {
                    mData: "DepotReplenishmentIndent", sTitle: "Depot Replenishment Indent", sClass: "head1", bSortable: true,
                },  
                {
                    mData: "CumulativeDepotSentQTY", sTitle: "Cumulative Depot Sent QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PendingQTY", sTitle: "Pending", sClass: "head1", bSortable: true,
                },                               
            ],

            bUseRendered: true,
            sPaginationType: "simple_numbers",
            aaSorting: [],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[50, 100, 500, -1], [50, 100, 500, "All"]],

            fnDrawCallback: function (oSettings) {
                var info = this.api().page.info();
                if (info.recordsTotal > 0) {
                    var tmptable = $('#dtListIndentSummary').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Export Indent Summary',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "Depot Replenishment Indent Summary",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtListIndentSummary').DataTable();
                    tmptable.buttons().destroy();
                }
            },
            createdRow: function (row, data, dataIndex) {                
                if ( data.PendingQTY <0) {
                    $(row).css({ 'background-color': '#fae8e8' })
                }
                else {
                    $(row).css({ 'background-color': '#e5e5ab' })
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
              let  TotalFinalProjection = api
                    .column(5)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                let  TotalClosingStock = api
                    .column(6)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);                    

                let  TotalDepotReplenishmentIndent = api
                    .column(8)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0); 

                let  TotalCumulativeDepotSentQTY = api
                    .column(9)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0); 

                // Pending Only Positive Value Sumation
                let v=api
                .column(10)
                .data().filter(x=>x>0)      
                let  TotalPendingQTYP = v                 
                    .reduce(function (a, b) {
                        if (parseInt(intVal(b)) > 0) {                            
                            console.log (a,intVal(b))
                            return parseInt(intVal(a)) +parseInt(intVal(b));
                        }
                    }, 0); 

                $(this).find('tfoot').remove()
                $(this).append('<tfoot><tr></tr></tfoot>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th  class="text-left">Total Final Projection :</th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + TotalFinalProjection + '</th>');   
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + TotalClosingStock + '</th>');   
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');    
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + TotalDepotReplenishmentIndent + '</th>');    
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + TotalCumulativeDepotSentQTY + '</th>');  
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + TotalPendingQTYP + '</th>'); 
            },            
        });
    },
}