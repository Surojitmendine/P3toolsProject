var salesprojection={

    Initialize:function(){
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');

    },

    Listsalesprojection: function () {

        var queryparams = {
            Month: $('#ddlMonth').val(),
            Year: $('#ddlYear').val(),
            ForecastingType: $('#ddlForecastingType').val(),
        }

        clearDatatable('dtListsalesprojection')
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SalesForecast/ListImportProjection').then(response => {
            if (typeof response != typeof undefined) {
                
                salesprojection.onSuccess_Listsalesprojection(response.data)
            }
        })
    },
    onSuccess_Listsalesprojection: function (data) {

     
    let month =$('#ddlMonth').select2('data')[0].text
    let year =$('#ddlYear').select2('data')[0].text


        var dtListsalesprojection = $('#dtListimportsalesprojection').DataTable({
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
                    mData: "ID", sTitle: "ID", sClass: "head1", bSortable: false,bVisible:false,
                }, 
                {
                    mData: "Month", sTitle: "Month56", sClass: "head1", bSortable: true,
                }, 
                {
                    mData: "Division", sTitle: "Division", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Depot", sTitle: "Depot", sClass: "head1", bSortable: true,
                },
                {
                    mData: "HQ", sTitle: "HQ", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProjectedTotalSalesQTY", sTitle: "Projected QTY For", sClass: "head1", bSortable: true,
                },                
                {
                    mData: "ForecastingType", sTitle: "Type", sClass: "head1", bSortable: true,
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
                    var tmptable = $('#dtListimportsalesprojection').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: ' Download Sales Projection',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',//fas fa-file-excel
                                title: "Sales Projection",
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
                    var tmptable = $('#dtListimportsalesprojection').DataTable();
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
                $(this).find("tfoot > tr:nth-child(1)").append('<th  class="text-left">Total Sale Projection QTY - </th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-left">' + ProjectedTotalSalesQTY + '</th>');   
                $(this).find("tfoot > tr:nth-child(1)").append('<th class="text-right"></th>');             
            },

        });
    },

    uploadProjection:function(){

        apiCall.ajaxFileUpload('FileUpload1','SalesForecast/UploadProjection')
        .then(res=>{
            clearDatatable('dtListimportsalesprojection')
            if(res.success==1){               
               salesprojection.onSuccess_Listsalesprojection(res.data)               
            }
        })      

    },

    updateProjection:function(){
        //let tabledata=JSON.stringify($('#dtListsalesprojection').dataTable().fnGetData());
        let tabledata=$('#dtListimportsalesprojection').dataTable().fnGetData();
        console.log(tabledata)
        
        apiCall.ajaxCall(undefined,'POST','SalesForecast/UpdateProjection',{ProjectionData:tabledata})                
            .then(res => {                   
                if (res.success == true) {
                    alert("Hi")
                    $("#FileUpload1").val('')
                    $('#ddlYear').val('0').trigger('change')
                    $('#ddlMonth').val('0').trigger('change')
                    clearDatatable('dtListimportsalesprojection')
                    showToastSuccessMessage(res.data)
                }
            })
    }

}