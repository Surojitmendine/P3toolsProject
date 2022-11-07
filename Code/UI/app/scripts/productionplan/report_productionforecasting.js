var ProductionPlanforecasting = {

    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');

        $("#ddlProductionForecasting").select2({
            multiple: false,
            closeOnSelect: true,
            theme: 'bootstrap4'
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
        if ($('#ddlProductionForecasting').val() == "0") {
            showToastErrorMessage("Production Forecasting can not be blank.Select Production Forecasting")
            return false;
        }

        var queryparams = {
            Month: $('#ddlMonth').val(),
            Year: $('#ddlYear').val(),
            //Product: $('#ddlProductName').val().join(),
        }

        // clearDatatable('dtList_VolumeConversion')
        // clearDatatable('dtList_VolumeCharge')
        // clearDatatable('dtList_FinalChargeUnit')

        if($('#ddlProductionForecasting').val() == 'Volume Conversion'){
            clearDatatable('dtList_VolumeConversion')
            apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ProductionPlan_VolumeConversion')
            .done(function (response) {
                console.log(response);
                clearDatatable('dtList_VolumeConversion')
                if (typeof response !== typeof undefined || response.success !== 0 ) {
                    ProductionPlanforecasting.onSuccess_ListVolumeConversion(response.data)

                    clearDatatable('dtList_VolumeCharge')
                    clearDatatable('dtList_FinalChargeUnit')
                }
                else{
                    ProductionPlanforecasting.onSuccess_ListVolumeConversion(response.data)
                    clearDatatable('dtList_VolumeCharge')
                    clearDatatable('dtList_FinalChargeUnit')
                }
        
            })
        }

        else if($('#ddlProductionForecasting').val() == 'Volume Charge'){
            clearDatatable('dtList_VolumeCharge')
            apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ProductionPlan_VolumeCharge')
            .done(function (response) {
                console.log(response)
                //clearDatatable('dtList_VolumeCharge')
                if (typeof response !== typeof undefined || response.success !== 0) {
                    ProductionPlanforecasting.onSuccess_ListVolumeCharge(response.data)
                    
                    clearDatatable('dtList_VolumeConversion')
                    clearDatatable('dtList_FinalChargeUnit')
                }
                else{
                    ProductionPlanforecasting.onSuccess_ListVolumeCharge(response.data)
                    clearDatatable('dtList_VolumeConversion')
                    clearDatatable('dtList_FinalChargeUnit')
                }
        
            })
        }

        else if($('#ddlProductionForecasting').val() == 'Final Charge Unit'){
            clearDatatable('dtList_FinalChargeUnit')
            apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ProductionPlan_FinalChargeUnit')
            .done(function (response) {
                console.log(response)
                //clearDatatable('dtList_VolumeCharge')
                if (typeof response !== typeof undefined || response.success !== 0) {
                    ProductionPlanforecasting.onSuccess_ListFinalChargeUnit(response.data)
                    
                    clearDatatable('dtList_VolumeConversion')
                    clearDatatable('dtList_VolumeCharge')
                }
                else{
                    ProductionPlanforecasting.onSuccess_ListFinalChargeUnit(response.data)
                    clearDatatable('dtList_VolumeConversion')
                    clearDatatable('dtList_VolumeCharge')
                }
        
            })
        }

        // $.when(
        //     apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ProductionPlan_VolumeConversion'),
        //     apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ProductionPlan_VolumeCharge'),
        //     apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ProductionPlan_FinalChargeUnit')
        // ).done((VolumeConversion, VolumeCharge, FinalChargeUnit) => {
        //     let hasdata = false
        //     console.log(VolumeCharge);
        //     if (typeof VolumeConversion[0] != typeof undefined) {
                
        //         hasdata = true
        //         ProductionPlanforecasting.onSuccess_ListVolumeConversion(VolumeConversion[0].data)
        //     }

        //     if (typeof VolumeCharge[0] != typeof undefined) {
        //         hasdata = true
        //         ProductionPlanforecasting.onSuccess_ListVolumeCharge(VolumeCharge[0].data)
        //     }

        //     if (typeof FinalChargeUnit[0] != typeof undefined) {
        //         hasdata = true
        //         ProductionPlanforecasting.onSuccess_ListFinalChargeUnit(FinalChargeUnit[0].data)
        //     }
        //     if (hasdata == true) {
        //         $('#btnExport').show()            
        //     }

        //     //ProductionPlanforecasting.Initialize();  

        // })

    },

    // export: function () {
    //     let VolumeConversion = $('#dtList_VolumeConversion').dataTable().fnGetData();
    //     let VolumeCharge = $('#dtList_VolumeCharge').dataTable().fnGetData();
    //     let FinalChargeUnit = $('#dtList_FinalChargeUnit').dataTable().fnGetData();

    //     console.log(VolumeConversion);

    //     let dataArr=[]
    //     let sheetArr=[]
    //     if(VolumeConversion.length>0){
    //         dataArr.push(VolumeConversion)
    //         sheetArr.push('Volume Conversion')
    //     }
    //     else if(VolumeCharge.length>0){
    //         dataArr.push(VolumeCharge)
    //         sheetArr.push('Volume Charge')
    //     }
    //     else if(FinalChargeUnit.length>0){
    //         dataArr.push(FinalChargeUnit)
    //         sheetArr.push('Final ChargeUnit')
    //     }

    //     tablesToExcel.exportJson(dataArr,sheetArr, 'Production_Forecasting.xls', 'Excel')
    // },


    onSuccess_ListVolumeConversion: function (data) {
        var dtList_VolumeConversion = $('#dtList_VolumeConversion').DataTable({
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
                    mData: "ForecastingForMonth", sTitle: "Month", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ForecastingForYear", sTitle: "Year", sClass: "head1", bSortable: true,
                },

                {
                    mData: "ProductType", sTitle: "Product Type", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductCategory", sTitle: "Product Category", sClass: "head1", bSortable: true,
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
                    mData: "FactorValue", sTitle: "Factor", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NextMonth_FinalForecastingQTY", sTitle: "Final Forecasting QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NoOfPCS", sTitle: "No Of PCS", sClass: "head1", bSortable: true,
                },
                {
                    mData: "DepotClosingStock", sTitle: "Depot Closing Stock", sClass: "head1", bSortable: true,
                },
                {
                    mData: "StockTransit", sTitle: "Stock Transit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "FactoryClosingStock", sTitle: "Factory Closing Stock", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductionForecastQTY", sTitle: "Production Forecast QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "LTR", sTitle: "Volume (In Ltrs)", sClass: "head1", bSortable: true,
                }
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
                    var tmptable = $('#dtList_VolumeConversion').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Volume Conversion',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "Volume Conversion",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtList_VolumeConversion').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },

    onSuccess_ListVolumeCharge: function (data) {
        var dtList_VolumeCharge = $('#dtList_VolumeCharge').DataTable({
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
                    mData: "ForecastingForMonth", sTitle: "Month", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ForecastingForYear", sTitle: "Year", sClass: "head1", bSortable: true,
                },

                {
                    mData: "ProductType", sTitle: "Product Type", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "BatchSize", sTitle: "Batch Size", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductionForecastVol", sTitle: "Production Forecast Vol", sClass: "head1", bSortable: true,
                },
                {
                    mData: "WIP", sTitle: "WIP", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ChargeableVolume", sTitle: "Chargeable Volume", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ChargeableBatchCount", sTitle: "Chargeable Batch Count", sClass: "head1", bSortable: true,
                },
                {
                    mData: "FinalCharge", sTitle: "Final Charge", sClass: "head1", bSortable: true,
                }
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
                    var tmptable = $('#dtList_VolumeCharge').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Volume Charge',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "Volume Charge",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtList_VolumeCharge').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },

    onSuccess_ListFinalChargeUnit: function (data) {
        var dtList_FinalChargeUnit = $('#dtList_FinalChargeUnit').DataTable({
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
                    mData: "ForecastingForMonth", sTitle: "Month", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ForecastingForYear", sTitle: "Year", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductType", sTitle: "Product Type", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductCategory", sTitle: "Category", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NoOfPCS", sTitle: "No Of Pcs", sClass: "head1", bSortable: true,
                },
                {
                    mData: "FinalChargeUnit", sTitle: "Final Charge In Unit", sClass: "head1", bSortable: true,
                }
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
                    var tmptable = $('#dtList_FinalChargeUnit').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'FinalChargeUnit',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "Final Charge Unit",
                                extension: '.xls',
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtList_FinalChargeUnit').DataTable();
                    tmptable.buttons().destroy();
                }
            },
        });
    },
}