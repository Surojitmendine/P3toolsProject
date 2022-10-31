var ProductionPlanforecasting = {

    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
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
            //Product: $('#ddlProductName').val().join(),
        }

        clearDatatable('dtList_VolumeConversion')
        clearDatatable('dtList_VolumeCharge')
        clearDatatable('dtList_FinalChargeUnit')

        $.when(
            apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ProductionPlan_VolumeConversion'),
            apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ProductionPlan_VolumeCharge'),
            apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ProductionPlan_FinalChargeUnit')
        ).done((VolumeConversion, VolumeCharge, FinalChargeUnit) => {
            let hasdata = false
            if (typeof VolumeConversion[0] != typeof undefined) {
                
                hasdata = true
                ProductionPlanforecasting.onSuccess_ListVolumeConversion(VolumeConversion[0].data)
            }

            if (typeof VolumeCharge[0] != typeof undefined) {
                hasdata = true
                ProductionPlanforecasting.onSuccess_ListVolumeCharge(VolumeCharge[0].data)
            }

            if (typeof FinalChargeUnit[0] != typeof undefined) {
                hasdata = true
                ProductionPlanforecasting.onSuccess_ListFinalChargeUnit(FinalChargeUnit[0].data)
            }
            if (hasdata == true) {
                $('#btnExport').show()              
            }


        })

    },

    export: function () {
        let VolumeConversion = $('#dtList_VolumeConversion').dataTable().fnGetData();
        let VolumeCharge = $('#dtList_VolumeCharge').dataTable().fnGetData();
        let FinalChargeUnit = $('#dtList_FinalChargeUnit').dataTable().fnGetData();

        let dataArr=[]
        let sheetArr=[]
        if(VolumeConversion.length>0){
            dataArr.push(VolumeConversion)
            sheetArr.push('Volume Conversion')
        }
        if(VolumeCharge.length>0){
            dataArr.push(VolumeCharge)
            sheetArr.push('Volume Charge')
        }
        if(FinalChargeUnit.length>0){
            dataArr.push(FinalChargeUnit)
            sheetArr.push('Final ChargeUnit')
        }

        tablesToExcel.exportJson(dataArr,sheetArr, 'Production_Forecasting.xls', 'Excel')
    },


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
                    mData: "ProductCode", sTitle: "Product Code", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NextMonth_FinalForecastingQTY", sTitle: "Final Forecasting QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NoOfPCS", sTitle: "No Of PCS", sClass: "head1", bSortable: true,
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
            /*fnDrawCallback: function (oSettings) {
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
            },*/
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
                    mData: "ForMonth", sTitle: "Month", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },

                {
                    mData: "VolumeInLtrs", sTitle: "Volume(In Ltrs)", sClass: "head1", bSortable: true,
                },
                {
                    mData: "WIPInLtrs", sTitle: "WIP (In Ltrs)", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ChargeableVolumeInLtrs", sTitle: "Chargeable Volume (In Ltrs)", sClass: "head1", bSortable: true,
                },
                {
                    mData: "BatchSize", sTitle: "Batch Size", sClass: "head1", bSortable: true,
                },
                {
                    mData: "FinalChargeInLtrs", sTitle: "Final Charge (In Ltrs)", sClass: "head1", bSortable: true,
                }
            ],

            bUseRendered: true,
            sPaginationType: "simple_numbers",
            aaSorting: [],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[10, 20, 30, -1], [10, 20, 30, "All"]],
            /*fnDrawCallback: function (oSettings) {
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
            },*/
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
                    mData: "ForMonth", sTitle: "Month", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductType", sTitle: "Product Type", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Category", sTitle: "Category", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "Pack Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "FactorProjectionForecastQTY", sTitle: "Factor Projection QTY", sClass: "head1", bSortable: true,
                },
                {
                    mData: "FinalChargeInUnit", sTitle: "Final Charge In Unit", sClass: "head1", bSortable: true,
                }
            ],

            bUseRendered: true,
            sPaginationType: "simple_numbers",
            aaSorting: [],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[10, 20, 30, -1], [10, 20, 30, "All"]],
            /*fnDrawCallback: function (oSettings) {
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
            },*/
        });
    },
}