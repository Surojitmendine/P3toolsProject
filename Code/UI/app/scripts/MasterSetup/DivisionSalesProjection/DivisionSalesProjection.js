var DivisionSalesProjection = {
    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'SalesForecast/SalesTeamProjection_SearchFields').then(response => {
            if (typeof response != typeof undefined) {                
                DivisionSalesProjection.FillcomboDivision($('#ddlDivision'),response.data.divisions)
                DivisionSalesProjection.FillcomboddlDepot($('#ddlDepot'),response.data.depots)
                DivisionSalesProjection.FillcomboProduct($('#ddlProductName'),response.data.products)
                DivisionSalesProjection.FillcomboPackUnit($('#ddlPackUnit'),response.data.packunits)
            }
        })
    },

    FillcomboDivision: function (dropdown,data) {
        $(dropdown).empty();
        $(dropdown).select2({
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

    FillcomboddlDepot: function (dropdown,data) {
        $(dropdown).empty();
        $(dropdown).select2({
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

    FillcomboProduct: function (dropdown,data) {
        $(dropdown).empty();
        $(dropdown).select2({
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
    FillcomboPackUnit: function (dropdown,data) {
        $(dropdown).empty();
        $(dropdown).select2({
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
            showToastErrorMessage("Year can not be blank .Select Year and Month from DD")
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
            PackUnit: $('#ddlPackUnit').val().join(),
            IsManual: true               
        }
        clearDatatable('dtListDivisionSalesProjection')
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'SalesForecast/SalesTeamProjection_Show').then(response => {
           
            if (typeof response != typeof undefined) {
                clearDatatable('dtListDivisionSalesProjection')
                DivisionSalesProjection.onSuccess_ListDivisionSalesProjection(response.data)
            }
        })
    },

    export: function () {
        let Comparison_Details = $('#dtListDivisionSalesProjection').dataTable().fnGetData();
        let dataArr=[]
        let sheetArr=[]
        if(Comparison_Details.length>0){
            dataArr.push(Comparison_Details)
            sheetArr.push('Sales Forecasting Details')
        }
        tablesToExcel.exportJson(dataArr,sheetArr, 'DivisionSalesProjection.xls', 'Excel')
    },

  
    onSuccess_ListDivisionSalesProjection: function (data) {
        var dtList_DivisionSalesProjection = $('#dtListDivisionSalesProjection').DataTable({
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
                    mData: "ProjectedTotalSalesQTY", sTitle: "Projection QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PrimaryTotalSalesQTY", sTitle: "Primary Sales QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "TotalClosingStockQTY", sTitle: "Closing Stock", sClass: "head1", bSortable: true,
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
                    var tmptable = $('#dtListDivisionSalesProjection').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: ' Export Sales Division Projection',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "Sales Division Projection",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtListDivisionSalesProjection').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },

    reloadDatatable: function () {
        clearDatatable("dtListDivisionSalesProjection")
        this.Search();
    },

    FillcomboEntry_Division: function (dropdown,data) {
        $(dropdown).empty();
        $(dropdown).prepend('<option value="0" selected> Select Division </option>')        
        $(dropdown).select2({
            multiple: false,
            data: data,
            closeOnSelect: true,
            theme: 'bootstrap4',
           // $('#'+element).prepend('<option value="0" selected> Month</option>')
            placeholder: {
                id: '', // the value of the option
                text: 'Select Division'
            },
        });
    },

    FillcomboEntry_ddlDepot: function (dropdown,data) {
        $(dropdown).empty();
        $(dropdown).prepend('<option value="0" selected> Select Depot </option>') 
        $(dropdown).select2({
            multiple: false,
            data: data,
            closeOnSelect: true,
            theme: 'bootstrap4',
            placeholder: {
                id: '', // the value of the option
                text: 'Select Depot'
            },
        });
    },

    FillcomboEntry_Product: function (dropdown,data) {
        $(dropdown).empty();
        $(dropdown).prepend('<option value="0" selected> Select Product </option>') 
        $(dropdown).select2({
            multiple: false,
            data: data,
            closeOnSelect: true,
            theme: 'bootstrap4',
            placeholder: {
                id: '', // the value of the option
                text: 'Select Product'
            },
        });
    },
    FillcomboEntry_PackUnit: function (dropdown,data) {
        $(dropdown).empty();
        $(dropdown).prepend('<option value="0" selected> Select Unit </option>') 
        $(dropdown).select2({
            multiple: false,
            data: data,
            closeOnSelect: true,
            theme: 'bootstrap4',
            placeholder: {
                id: '', // the value of the option
                text: 'Select Packunit'
            },
        });
    },
    initializeControls: function () {       
        return new Promise(function (resolve, reject) {
            resolve(
               common.FillMonthList('ddlEntry_Month'),
               common.FillYearList('ddlEntry_Year'),
               apiCall.ajaxCallWithReturnData(undefined, 'GET', 'SalesForecast/SalesTeamProjection_SearchFields').then(response => {
                   if (typeof response != typeof undefined) {                
                    DivisionSalesProjection.FillcomboEntry_Division($('#ddlEntry_Division'),response.data.divisions)
                    DivisionSalesProjection.FillcomboEntry_ddlDepot($('#ddlEntry_Depot'),response.data.depots)
                    DivisionSalesProjection.FillcomboEntry_Product($('#ddlEntry_ProductName'),response.data.products)                    
                    DivisionSalesProjection.FillcomboEntry_PackUnit($('#ddlEntry_PackUnit'),response.data.packunits)
                   }
               })
            )      
            }
        )},

    OpenAddModal: function (OpenCallBack) {   
        CreateModal('modAddDivisionSalesProjection', 'pages/MasterSetup/DivisionSalesProjection/divisionsalesprojection-add.html', function () {
            //resetControls("formAddDivisionSalesProjection")
            
            DivisionSalesProjection.initializeControls()
                .then(() => {
                    if (typeof OpenCallBack != typeof undefined) {
                        OpenCallBack()
                        OpenCallBack = undefined
                    }
                })
        })
    },   

    CloseAddModal: function () {
        $("#modAddDivisionSalesProjection").modal('hide')
        onModalHidden('modAddDivisionSalesProjection', function () {
            resetControls("formAddDivisionSalesProjection")
        })
    },

    AddUpdateDivisionSalesProjection: function () {
        if (fieldValidation('formAddDivisionSalesProjection') == true) {
            if ($("#hdnID").val() <= 0) {
                apiCall.ajaxCall('formAddDivisionSalesProjection', 'POST', 'SalesForecast/AddNew_SalesTeamProjection')
                    .done(function (response) {
                        if (response.success == 1) {
                           // resetControls("formAddDivisionSalesProjection")
                            DivisionSalesProjection.reloadDatatable();
                            DivisionSalesProjection.CloseAddModal();
                            showToastSuccessMessage(response.message)
                        }
                    })
            }

            else if ($("#hdnID").val() > 0) {
                apiCall.ajaxCall('formAddDivisionSalesProjection', 'POST', 'SalesForecast/Update_SalesTeamProjection')
                    .done(function (response) {
                        if (response.success == 1) {
                            resetControls("formAddDivisionSalesProjection")
                            DivisionSalesProjection.reloadDatatable();
                            showToastSuccessMessage(response.message)
                        }
                    })
            }
        }
        else {
        }
    },
}