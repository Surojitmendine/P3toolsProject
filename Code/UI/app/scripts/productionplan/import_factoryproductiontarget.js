var importfactoryproductiontarget={

    Initialize:function(){
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
    },

    UploadExcel_FactoryProductionTarget:function(){
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

        apiCall.ajaxFileUpload('FileUpload1','ProductionPlan/UploadExcel_FactoryProductionTarget',parameters)
        .then(res=>{
            clearDatatable('dtListPPFactoryProductionTarget')
            if(res.success==1){               
                importfactoryproductiontarget.onSuccess_ListPPFactoryProductionTarget(res.data)               
            }
        })      
    },    

    ListPPFactoryProductionTarget: function () {
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
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_FactoryProductionTarget').then(response => {
            if (typeof response != typeof undefined) {
                clearDatatable('dtListPPFactoryProductionTarget')
                importfactoryproductiontarget.onSuccess_ListPPFactoryProductionTarget(response.data)
            }
        })
    },

    onSuccess_ListPPFactoryProductionTarget: function (data) {     
    let month =$('#ddlMonth').select2('data')[0].text
    let year =$('#ddlYear').select2('data')[0].text

        var dtListPPFactoryProductionTarget = $('#dtListPPFactoryProductionTarget').DataTable({
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
                    mData: "MonthName", sTitle: "For Month", sClass: "head1", bSortable: true,
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
                    mData: "FinalUnits_QTY", sTitle: "Units", sClass: "head1", bSortable: true,
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
                    var tmptable = $('#dtListPPFactoryProductionTarget').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Download Factory Production Target',
                                extend: 'excel',
                                className: 'btn btn-info float-right ',//fas fa-file-excel
                                title: "FactoryProductionTarget",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtListPPFactoryProductionTarget').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },

    SaveExcel_FactoryProductionTarget: function () {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        let tabledata = $('#dtListPPFactoryProductionTarget').dataTable().fnGetData();
        
        if (tabledata.length > 0) {
            apiCall.ajaxCall(undefined, 'POST', 'ProductionPlan/SaveExcel_FactoryProductionTarget', { FactoryProductionTarget_Data: tabledata,year:$('#ddlYear').val(),month:$('#ddlMonth').val() })
                .then(res => {
                   
                    if (res.success == true) {
                        $("#FileUpload1").val('')
                        $('#ddlYear').val('0').trigger('change')
                        $('#ddlMonth').val('0').trigger('change')
                        clearDatatable('dtListPPFactoryProductionTarget')

                        showToastSuccessMessage("Factory Production Target Data Save Sucessfully !!")
                    }
                })
        }
    }    
}