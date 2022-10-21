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
                //console.log(this.ProductName);
                $("#ddlProductName").append("<option value=" + this.ProductName + ">" + this.ProductName + "</option>");
            });
        })

    },

    GetProductWiseBatchSize : function () {
        var obj = {
            ProductName: $("#ddlProductName").val()
        };
        if($("#ddlProductName").val() == "0"){
            showToastErrorMessage("Product Name can not be blank.Select Product Name")
            return false;
        }

        apiCall.ajaxCallWithReturnData(obj, "GET", 'ProductionPlan/Get_ProductWise_BatchSize')
        .then((res)=>{
            $('#ddlBatchSize').empty();
            $("#ddlBatchSize").append("<option value=" + "" + ">Select BatchSize</option>");
            $.each(res.data, function (index, value) {
                //console.log(this.BatchSize);
                $("#ddlBatchSize").append("<option value=" + this.BatchSize + ">" + this.BatchSize + "</option>");
            });
            
        })

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
        

        var queryparams = {
            Month: $('#ddlMonth').val(),
            Year: $('#ddlYear').val(),
            ProductName: $("#ddlProductName").val(),
            BatchSize: $("#ddlBatchSize").val()
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
                info: "Showing _Chargeable_EBatch",
            },
            aoColumns: [
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
                                    columns: [0, 1, 2]
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

    }

}