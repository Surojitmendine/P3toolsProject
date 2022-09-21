var importSecondarySales={
    Initialize:function(){
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
    },

    ListimportSecondarySales: function () {
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
            ForecastingType: $('#ddlForecastingType').val(),
        }

        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SalesForecast/List_SecondarySales').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtListimportSecondarySales')
                importSecondarySales.onSuccess_ListimportSecondarySales(response.data)
            }
        })
    },

    onSuccess_ListimportSecondarySales: function (data) {
    let month =$('#ddlMonth').select2('data')[0].text
    let year =$('#ddlYear').select2('data')[0].text
        var dtListimportSecondarySales = $('#dtListimportSecondarySales').DataTable({
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
                    mData: "ProductCode", sTitle: "Product Code", sClass: "head1", bSortable: true, bVisible:false,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true, 
                },
                {
                    mData: "UOM", sTitle: "UOM", sClass: "head1", bSortable: true, 
                },
                {
                    mData: "SalesQTY", sTitle: "Sales QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "FreeSampleQTY", sTitle: "Free QTY", sClass: "head1", bSortable: true, bVisible:false,
                },
                {
                    mData: "ClosingStockQTY", sTitle: "Closing Stock QTY", sClass: "head1", bSortable: true, bVisible:true,
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
                    var tmptable = $('#dtListimportSecondarySales').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Download Secondary Sales',
                                extend: 'excel',
                                className: 'btn btn-info float-right ',//fas fa-file-excel
                                title: "SecondarySales",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtListimportSecondarySales').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },

    uploadSecondarysales:function(){
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }

        apiCall.ajaxFileUpload('FileUpload1','SalesForecast/Upload_SecondarySales')
        .then(res=>{
            clearDatatable('dtListimportSecondarySales')
            if(res.success==1){               
               importSecondarySales.onSuccess_ListimportSecondarySales(res.data)               
            }
        })      

    },

    updateSecondarysales12:function(){
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        
        //let tabledata=JSON.stringify($('#dtListimportSecondarySales').dataTable().fnGetData());
        let tabledata=$('#dtListimportSecondarySales').dataTable().fnGetData();
        console.log(tabledata)
        var parameters = {
            Year: $('#ddlYear').val(),
            Month: $('#ddlMonth').val()
        }
        apiCall.ajaxCall(undefined,'POST','SalesForecast/Update_SecondarySales',{SecondarySales:tabledata}, parameters)

    },

    updateSecondarysales: function () {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        let tabledata = $('#dtListimportSecondarySales').dataTable().fnGetData();
        
        if (tabledata.length > 0) {
            apiCall.ajaxCall(undefined, 'POST', 'SalesForecast/SaveExcel_SecondarySales', { SecondarySales: tabledata,year:$('#ddlYear').val(),month:$('#ddlMonth').val() })
                .then(res => {                   
                    if (res.success == true) {
                        $("#FileUpload1").val('')
                        $('#ddlYear').val('0').trigger('change')
                        $('#ddlMonth').val('0').trigger('change')
                        clearDatatable('dtListimportSecondarySales')
                        showToastSuccessMessage(res.data)
                    }
                })
        }
        
    }
}