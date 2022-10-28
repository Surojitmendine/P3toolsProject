var chargeablebatch = {
    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
    },

    GetProductName: function(){
        apiCall.ajaxCall(undefined, 'GET', 'ProductionPlan/Get_ProductName',undefined)
        .then((res)=>{
            $('#ddlProductName').empty();
            $("#ddlProductName").append("<option value=0>Select Product</option>");
            $("#ddlBatchSize").append("<option value=" + "" + ">Select BatchSize</option>");
            $.each(res.data, function (index, value) {
                // console.log(this.ProductName);
                $("#ddlProductName").append("<option value=" + this.ProductName + ">" + this.ProductName + "</option>");

            });  
            
            $("#ddlProductName").select2({
                multiple: false,
                closeOnSelect: true,
                theme: 'bootstrap4'
            });
        })

    },

    GetProductWiseBatchSize : function () {
        var obj = {
            ProductName: $("#ddlProductName").val()
        };

        apiCall.ajaxCallWithReturnData(obj, "GET", 'ProductionPlan/Get_ProductWise_BatchSize')
        .then((res)=>{
            $('#ddlBatchSize').empty();
            $("#ddlBatchSize").append("<option value=" + "" + ">Select BatchSize</option>");
            $.each(res.data, function (index, value) {
                //console.log(this.BatchSize);
                $("#ddlBatchSize").append("<option value=" + this.BatchSize + ">" + this.BatchSize + "</option>");
            });
            
        })

        $("#ddlBatchSize").select2({
            multiple: true,
            closeOnSelect: true,
            theme: 'bootstrap4'
        });

    },

    ListChargeableBatch: function () {

        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
        if($("#ddlProductName").val() == "0"){
            showToastErrorMessage("Product Name can not be blank.Select Product Name")
            return false;
        }
        
        var Batch_Size = $('#ddlBatchSize :selected').map(function () {
            return $(this).text();
        }).get().join(',');

        var queryparams = {
            Month: $('#ddlMonth').val(),
            Year: $('#ddlYear').val(),
            ProductName: $("#ddlProductName").val(),
            BatchSize: Batch_Size
        };
        console.log(queryparams);
        clearDatatable('dtListChargeableBatch')
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'ProductionPlan/List_ChargeableBatchProduct')
            .done(function (response) {
                clearDatatable('dtListChargeableBatch')
                if (typeof response !== typeof undefined) {
                    chargeablebatch.onSuccess_ListChargeableBatch(response)
                }

            })
    },

    reloadDatatable: function () {
        clearDatatable("dtListChargeableBatch")
        this.ListChargeableBatch();
    },

    OpenEditModal: function (OpenCallBack) {
        CreateModal('modEditChargeableBatch', 'pages/productionplan/process/ChargeableBatch-edit.html', function () {
            if (typeof OpenCallBack != typeof undefined) {
                OpenCallBack()
                OpenCallBack = undefined
            }
        })
    },

    CloseEditModal: function () {
        $("#modEditChargeableBatch").modal('hide')
        onModalHidden('modEditChargeableBatch', function () {
            resetControls("formEditChargeableBatch")
        })
    },

    onSuccess_ListChargeableBatch: function (response) {
        var dtListChargeableBatchlist = $('#dtListChargeableBatch').DataTable({
            bServerSide: false,
            bDestroy: true,
            paging: true,
            autoWidth: false,
            bStateSave: false,
            searching: true,
            data: response.data,
            language: {
                paginate: {
                    previous: "<",
                    next: ">"
                },
                info: "Showing _START_ - _END_ of _TOTAL_ Chargeable Batch",
            },
            aoColumns: [
                {
                    mData: "SLNO", sTitle: "SLNO", sClass: "head1", bSortable: true, bVisible: false,
                },
                {
                    mData: "CompanyId", sTitle: "CompanyId", sClass: "head1", bSortable: true, bVisible: false,
                },
                {
                    mData: "ForMonth", sTitle: "Month", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ForYear", sTitle: "Year", sClass: "head1", bSortable: true,
                },

                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },

                {
                    mData: "ProductUOM", sTitle: "UOM", sClass: "head1", bSortable: true,
                },
                {
                    mData: "Batchsize", sTitle: "Batchsize", sClass: "head1", bSortable: true,
                },

                {
                    mData: "Final_Forecasted_Production_Volume_LT", sTitle: "Forecasted Production Volume", sClass: "head1", bSortable: true,
                },
                {
                    mData: "UnitFactor", sTitle: "Unit Factor", sClass: "head1", bSortable: true,
                },
                {
                    mData: "UserUnitFactor", sTitle: "User Unit Factor", sClass: "head1", bSortable: true,
                },
                {
                    mData: null, defaultContent: "", sTitle: "Edit", sClass: "head1", bSortable: true,
                    mRender: function (data, type, full) {
                        var markup = '<a href="javascript:void(0)" onclick="chargeablebatch.GetByID_BatchSize_UnitFactor(' + full.SLNO + ')" >Edit</a>'
                        // onclick="ProductMaster.GetByID_ProductMaster(' + full.PK_ProductID + ')"
                        return markup;
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
                    var tmptable = $('#dtListChargeableBatch').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Export',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "ChargeableBatchList",
                                extension: '.xls',
                                exportOptions: {
                                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                                },
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtListChargeableBatch').DataTable();
                    tmptable.buttons().destroy();
                }

            }
        });

    },

    GetByID_BatchSize_UnitFactor : function (ID) {
        var obj = { SLNO: ID }
        //console.log(obj)
        chargeablebatch.OpenEditModal(function () {
            apiCall.ajaxCallWithReturnData(obj, "GET", 'ProductionPlan/GetByID_BatchWiseUnitFactor')
                .then((response) => {
                    $("#hdnPK_SLNO").val(response.data[0].SLNO);
                    $("#txtForMonth").val(response.data[0].ForMonth);
                    $("#txtForYear").val(response.data[0].ForYear);
                    $("#txtProductName").val(response.data[0].ProductName);
                    $("#txtProductUOM").val(response.data[0].ProductUOM);
                    $("#txtForecastedProductionVolume").val(response.data[0].Final_Forecasted_Production_Volume_LT);
                    $("#txtBatchsize").val(response.data[0].Batchsize);
                    $("#txtUnitFactor").val(response.data[0].UnitFactor);
                    $("#txtUserUnitFactor").val(response.data[0].UserUnitFactor);

                })
        })
    },

    UpdateChargeableBatchUnit: function () {

        var ajaxdata = {
            SLNO: $('#hdnPK_SLNO').val(),
            UserUnitFactor: $('#txtUserUnitFactor').val(),
        };
        console.log(ajaxdata);

        apiCall.ajaxCall(undefined, 'POST', 'ProductionPlan/Update_BatchWiseUnitFactorBySLNO',ajaxdata)
        .then((res)=>{
            $("#modEditChargeableBatch").modal('hide');
            showToastSuccessMessage(res.data);
            chargeablebatch.reloadDatatable();
        })

        
    },

}