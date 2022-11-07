var ProductMappingMaster = {

    ListProductMappingMaster: function () {
        var queryparams = {
            Type: $('#ddlType').val(),
            ProductCategory: $('#ddlProductCategory').val()
        }
        clearDatatable('dtProductMappingMasterlist')
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'MasterSetup/List_ProductMappingMaster')
            .done(function (response) {
                clearDatatable('dtProductMappingMasterlist')
                if (typeof response !== typeof undefined) {
                    ProductMappingMaster.onSuccess_ListProductMappingMaster(response)
                }

            })
    },

    onSuccess_ListProductMappingMaster: function (response) {
        var dtProductMappingMasterlist = $('#dtProductMappingMasterlist').DataTable({
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
                info: "Showing _START_ - _END_ of _TOTAL_ ProductMasters",
            },
            aoColumns: [
                {
                    mData: "PK_ProductID", sTitle: "ID", sClass: "head1", bSortable: true, bVisible: false,
                },
                {
                    mData: "ProductCategory", sTitle: "ProductCategory", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductCode", sTitle: "Code", sClass: "head1", bSortable: true,
                },

                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },

                {
                    mData: "PackUnit", sTitle: "Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "TallyProductName", sTitle: "TallyProductName", sClass: "head1", bSortable: true,
                },

                {
                    mData: "TallyUOM", sTitle: "TallyUOM", sClass: "head1", bSortable: true,
                },

                {
                    mData: null, defaultContent: "", sTitle: "Edit", sClass: "head1", bSortable: true,
                    mRender: function (data, type, full) {
                        console.log("hi")
                        var markup = '<a href="javascript:void(0)" onclick="ProductMappingMaster.GetByID_ProductMappingMaster(' + full.PK_ProductID + ',' +  '\'' + full.ProductCategory + '\'' + ')" >Edit</a>'
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
                    var tmptable = $('#dtProductMappingMasterlist').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Export',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "ProductMasterList",
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
                    var tmptable = $('#dtProductMappingMasterlist').DataTable();
                    tmptable.buttons().destroy();
                }

            }
        });

    },

    OpenEditModal: function (OpenCallBack) {
        CreateModal('modEditProductMappingMaster', 'pages/MasterSetup/ProductMappingMaster/productmappingmaster-edit.html', function () {
            if (typeof OpenCallBack != typeof undefined) {
                OpenCallBack()
                OpenCallBack = undefined
            }
        })
    },

    CloseEditModal: function () {
        $("#modEditProductMappingMaster").modal('hide')
        onModalHidden('modEditProductMappingMaster', function () {
            resetControls("formEditProductMappingMaster")
        })
    },

    reloadDatatable: function () {
        clearDatatable("dtProductMappingMasterlist")
        this.ListProductMappingMaster();
    },

    UpdateProductMappingMaster: function (sptocall) {

        var ajaxdata = {
            PK_ProductID: $('#hdnPK_ProductID').val(),
            // ProductCode: $('#txtProductCode').val(),
            // ProductName: $('#txtProductName').val(),
            // PackUnit: $('#txtPackUnit').val(),
            TallyProductName: $('#txtTallyProductName').val(),
            TallyUOM: $('#txtTallyUOM').val(),
            ProductCategory: $('#txtProductCategory').val()
        };
        console.log(ajaxdata);

        apiCall.ajaxCall(undefined, 'POST', 'MasterSetup/Update_ProductMappingMaster',ajaxdata)
        .then((res)=>{
            $("#modEditProductMappingMaster").modal('hide');
            showToastSuccessMessage(res.data);
            ProductMappingMaster.reloadDatatable();
        })

        
    },

    GetByID_ProductMappingMaster: function (ID,ProductCategory) {
        var obj = { ProductID: ID, ProductCategory: ProductCategory }
        //console.log(obj)
        ProductMappingMaster.OpenEditModal(function () {
            apiCall.ajaxCallWithReturnData(obj, "GET", 'MasterSetup/GetByID_ProductMappingMaster')
                .then((response) => {
                    //console.log(response.data[0].PK_ProductID)
                    //apiCall.bindModel('formEditProductMappingMaster', response.data)
                    $("#hdnPK_ProductID").val(response.data[0].PK_ProductID);
                    $("#txtProductCategory").val(response.data[0].ProductCategory);
                    $("#txtProductCode").val(response.data[0].ProductCode);
                    $("#txtProductName").val(response.data[0].ProductName);
                    $("#txtPackUnit").val(response.data[0].PackUnit);
                    $("#txtTallyProductName").val(response.data[0].TallyProductName);
                    $("#txtTallyUOM").val(response.data[0].TallyUOM);
                    $("#chkIsActive").bootstrapSwitch('state', response.data.IsActive);

                })
        })
    },

}