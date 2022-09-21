var divisionsale_projection = {
    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'SalesForecast/SalesTeamProjection_SearchFields').then(response => {
            if (typeof response != typeof undefined) {                
                divisionsale_projection.FillcomboDivision(response.data.divisions)
                divisionsale_projection.FillcomboddlDepot(response.data.depots)
                divisionsale_projection.FillcomboProduct(response.data.products)
                divisionsale_projection.FillcomboPackUnit(response.data.packunits)
            }
        })
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

    FillcomboddlDepot: function (data) {
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
            Month: $('#ddlMonth').val(),
            Year: $('#ddlYear').val(),
            Division: $('#ddlDivision').val().join(),
            DepotName: $('#ddlDepot').val().join(),
            Product: $('#ddlProductName').val().join(),
            PackUnit: $('#ddlPackUnit').val().join()                   
        }
        clearDatatable('dtListdivisionsale_projection')
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SalesForecast/SalesTeamProjection_Show').then(response => {
           
            if (typeof response != typeof undefined) {
                clearDatatable('dtListdivisionsale_projection')
                divisionsale_projection.onSuccess_Listdivisionsale_projection(response.data)
            }
        })
    },

    export: function () {
        let Comparison_Details = $('#dtListdivisionsale_projection').dataTable().fnGetData();
        let dataArr=[]
        let sheetArr=[]
        if(Comparison_Details.length>0){
            dataArr.push(Comparison_Details)
            sheetArr.push('Sales Forecasting Details')
        }
        tablesToExcel.exportJson(dataArr,sheetArr, 'divisionsale_projection.xls', 'Excel')
    },

  
    onSuccess_Listdivisionsale_projection: function (data) {
        var dtList_divisionsale_projection = $('#dtListdivisionsale_projection').DataTable({
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
               /* {
                    mData: "ProductCode", sTitle: "Product Code", sClass: "head1", bSortable: true,
                },*/
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PrimaryTotalSalesQTY", sTitle: "Primary Sales QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "TotalClosingStockQTY", sTitle: "Closing Stock", sClass: "head1", bSortable: true,
                },

                {
                    mData: "ProjectedTotalSalesQTY", sTitle: "Projection Quantity", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NRVRate", sTitle: "NRV Rate", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProjectionValue", sTitle: "ProjectionValue", sClass: "head1", bSortable: true,
                },                
                {
                    mData: "IsProcessed", defaultContent: '', sTitle: "", sClass: "head1", bSortable: false,
                    mRender: function (data, type, row, meta) {
                        //if (data == false) {
                            var markup = '<a  href="javascript:void(0)" onclick="divisionsale_projection.onRowEditClick(this)">'
                            markup += '      <i class="fas fa-edit" aria-hidden="true"></i>'
                            markup += '   </a>'
                            markup += '<a  href="javascript:void(0)" onclick="divisionsale_projection.onRowSaveClick(this,' + row.ID + ')" style="display:none">'
                            markup += '      <i class="fas fa-save" aria-hidden="true"></i>'
                            markup += '   </a>'
                            markup += '<a  href="javascript:void(0)" onclick="divisionsale_projection.onRowUndoClick(this)" style="display:none">'
                            markup += '      <i class="fas fa-undo-alt" aria-hidden="true"></i>'
                            markup += '   </a>'
                            return markup;
                        //}
                    }
                },
            ],
            bUseRendered: true,
            sPaginationType: "simple_numbers",
            aaSorting: [],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "All"]],   

            fnDrawCallback: function (oSettings) {
                var info = this.api().page.info();
                if (info.recordsTotal > 0) {
                    var tmptable = $('#dtListdivisionsale_projection').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: ' Export Sales Division',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "Sales Division",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtListdivisionsale_projection').DataTable();
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
              let  ProjectedTotalSalesQTY = api
                    .column(7)
                    .data()
                    .reduce(function (a, b) {
                        return intVal(a) + intVal(b);
                    }, 0);

                $(this).find('tfoot').remove()
                $(this).append('<tfoot><tr></tr></tfoot>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th  class="text-left">Total Division Sale Projection QTY - </th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');              
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + ProjectedTotalSalesQTY.toFixed(2) + '</th>'); 
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
            },
              
            createdRow: function( row, data, dataIndex ) {
                if ( parseFloat(data.ProjectedTotalSalesQTY) <=0 ) {
                  $(row).css({"background-color": '#eff2eb' });
                }
              }
        });
    },

    onRowEditClick(element, row) {
        $(element).hide()
        $(element).nextAll('a').show()
        let $row = $(element).closest("tr");
        let $td = $row.find("td:eq(7)");
        let txt = $td.text();
        $td.html("").append("<input type='text' value=\"" + txt + "\">");
    },

    onRowSaveClick: function (element, ID) {
        let $row = $(element).closest("tr");
        let $td = $row.find("td:eq(7)");
        let txt = $td.find("input").val()

        let $td1 = $row.find("td:eq(8)");
        let txtNRV = $row.find("td:eq(8)").text();
         
        apiCall.ajaxCall(undefined, 'POST', 'SalesForecast/SalesTeamProjection_Save',
            { ID: ID, ProjectedTotalSalesQTY: txt, NRVRate: txtNRV }
        ).then(res => {
            if (res.success == 1) {
                
                divisionsale_projection.Listdivisionsale_projection(true)
            }
        })
            .done(() => {
                $(element).hide()
                $(element).prev('a').show()
                bootbox.alert("Sales Projection added.");
                // $td.html(txt)
            })
    },

    onRowUndoClick: function (element) {
        $(element).hide()
        $(element).prev('a').hide()
        let firsta = $(element).parent().find('a')[0]
        $(firsta).show()
        let $row = $(element).closest("tr");
        let $td = $row.find("td:eq(8)");
        let txt = $td.find("input").val()
        $td.html(txt)
    }
}