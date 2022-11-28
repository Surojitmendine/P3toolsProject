var ProductMaster = {
    // Initialize: function () {
    //     apiCall.ajaxCallWithReturnData(undefined, 'GET', 'MasterSetup/ProductMaster_SearchFields').then(response => {
    //         if (typeof response != typeof undefined) {                
    //             ProductMaster.FillcomboCategory(response.data.categories)
    //         }
    //     })
        
    // },
    // FillcomboCategory: function (data) {
    //     $('#ddlCategory').empty();
    //     $('#ddlCategory').select2({
    //         multiple: true,
    //         data: data,
    //         closeOnSelect: true,
    //         theme: 'bootstrap4',
    //         placeholder: {
    //             id: '', // the value of the option
    //             text: 'Select Category'
    //         },
    //     });
    // },

    GetProductTypeName: function(){
        apiCall.ajaxCall(undefined, 'GET', 'MasterSetup/Get_ProductTypeName',undefined)
        .then((res)=>{
            $('#ddlProductType').empty();
            $("#ddlProductType").append("<option value=0>Select Product Category</option>");
            $.each(res.data, function (index, value) {
                console.log(this.ProductTypeName);
                $("#ddlProductType").append("<option value=" + this.ProductTypeName + ">" + this.ProductTypeName + "</option>");

            });  
            
            $("#ddlProductName").select2({
                multiple: false,
                closeOnSelect: true,
                theme: 'bootstrap4'
            });
        })

    },

    GetProductTypeWiseCategory : function () {
        var obj = {
            ProductTypeName: $("#ddlProductType").val()
        };

        apiCall.ajaxCallWithReturnData(obj, "GET", 'MasterSetup/Get_ProductTypeWise_Category')
        .then((res)=>{
            $('#ddlCategory').empty();
            $("#ddlCategory").append("<option value=" + "" + ">Select/option>");
            $.each(res.data, function (index, value) {
                //console.log(this.BatchSize);
                $("#ddlCategory").append("<option value=" + this.CategoryName + ">" + this.CategoryName + "</option>");
            });
            
        })

        $("#ddlCategory").select2({
            multiple: true,
            closeOnSelect: true,
            theme: 'bootstrap4'
        });

    },


    ListProductMaster: function () {

        var catName = $('#ddlCategory :selected').map(function () {
            return $(this).text();
        }).get().join(',');

        var queryparams = {
            ProductTypeName: $("#ddlProductType").val(),
            CategoryName: catName,         
        }

        console.log(queryparams);

        clearDatatable('dtProductMasterlist')
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'MasterSetup/List_ProductMaster')
        .done(function (response) {
            clearDatatable('dtProductMasterlist')
            if (typeof response !== typeof undefined) {
                ProductMaster.onSuccess_ListProductMaster(response)
            }
    
        })
    },


    onSuccess_ListProductMaster: function (response) {        
        var dtProductMasterlist = $('#dtProductMasterlist').DataTable({
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
                    mData: "CategoryName", sTitle: "Category", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductCode", sTitle: "Code", sClass: "head1", bSortable: true,
                },

                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductUOM", sTitle: "UOM", sClass: "head1", bSortable: true,
                },                
                {
                    mData: "PackUnit", sTitle: "Unit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductCategory", sTitle: "Product Type", sClass: "head1", bSortable: true,
                },

                {
                    mData: "FactorValue", sTitle: "Factor Value", sClass: "head1", bSortable: true,
                },
                {
                    mData: "BatchSize", sTitle: "Batch Size", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NRVRate", sTitle: "NRV Rate", sClass: "head1", bSortable: true,
                },
                {
                    mData: "NRVEffectiveRateFrom", sTitle: "NRV Effective Date", sClass: "head1", bSortable: true, 
                },

                {
                    mData: null, defaultContent: "", sTitle: "Edit", sClass: "head1", bSortable: true,
                    mRender: function (data, type, full) {
                        console.log("hi")
                        var markup = '<a href="javascript:void(0)" onclick="ProductMaster.GetByID_ProductMaster(' + full.PK_ProductID + ',' +  '\'' + full.ProductCategory + '\'' + ')">Edit</a>'
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
                    var tmptable = $('#dtProductMasterlist').DataTable();
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
                                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                                },
                            },
                        ]
                    });
                    tmptable.buttons(0, null).container().appendTo('#export-area');
                }
                else if (info.recordsTotal <= 0) {
                    var tmptable = $('#dtProductMasterlist').DataTable();
                    tmptable.buttons().destroy();
                }

            }
        });

    },
    reloadDatatable: function () {
        clearDatatable("dtProductMasterlist")
        this.ListProductMaster();
    },

    initializeControls: function () {       
        return new Promise(function (resolve, reject) {
            resolve(

            )      
            }
        )},
        
    OpenAddModal: function (OpenCallBack) {   
        CreateModal('modAddProductMaster', 'pages/MasterSetup/ProductMaster/productmaster-add.html', function () {
                    if (typeof OpenCallBack != typeof undefined) {
                        OpenCallBack()
                        OpenCallBack = undefined
                    }
        })
    },

    CloseAddModal: function () {
        $("#modAddProductMaster").modal('hide')
        onModalHidden('modAddProductMaster', function () {
            resetControls("formAddProductMaster")
        })
    },

    OpenEditModal: function (OpenCallBack) {   
        CreateModal('modEditProductMaster', 'pages/MasterSetup/ProductMaster/productmaster-edit.html', function () {
                    if (typeof OpenCallBack != typeof undefined) {
                        OpenCallBack()
                        OpenCallBack = undefined
                    }
        })
    },

    CloseEditModal: function () {
        $("#modEditProductMaster").modal('hide')
        onModalHidden('modEditProductMaster', function () {
            resetControls("formEditProductMaster")
        })
    },

    AddProductMaster: function () {

        var ajaxdata = {
            ProductCategory: $('#txtProductCategory').val(),
            ProductCode: $('#txtProductCode').val(),
            ProductName: $('#txtProductName').val(),
            PackUnit: $('#txtPackUnit').val(),
            CategoryName: $('#txtProductType').val(),
            ProductUOM: $('#txtProductUOM').val(),
            FactorValue: $('#txtFactorValue').val(),
            BatchSize: $('#txtBatchSize').val(),
            NRVRate: $('#txtNRVRate').val(),
            NRVEffectiveRateFrom: $('#txtNRVEffectiveRateFrom').val()
            
        };
        //console.log(ajaxdata);

        apiCall.ajaxCall(undefined, 'POST', 'MasterSetup/AddNew_ProductMaster',ajaxdata)
        .then((res)=>{
            console.log(res.success + ',' + res.data);
            $("#modAddProductMaster").modal('hide');
            if(res.success == 1){
                showToastSuccessMessage(res.data);
            }
            else{
                showToastErrorMessage(res.data);
            }
            ProductMaster.reloadDatatable();
        })

        
    },

    EditProductMaster: function () {

        var ajaxdata = {
            PK_ProductID: $('#hdnPK_ProductID').val(),
            ProductCategory: $('#txtProductCategory').val(),
            ProductCode: $('#txtProductCode').val(),
            ProductName: $('#txtProductName').val(),
            PackUnit: $('#txtPackUnit').val(),
            CategoryName: $('#txtProductType').val(),
            ProductUOM: $('#txtProductUOM').val(),
            FactorValue: $('#txtFactorValue').val(),
            BatchSize: $('#txtBatchSize').val(),
            NRVRate: $('#txtNRVRate').val(),
            NRVEffectiveRateFrom: $('#txtNRVEffectiveRateFrom').val()
            
        };
        //console.log(ajaxdata);

        apiCall.ajaxCall(undefined, 'POST', 'MasterSetup/Update_ProductMaster',ajaxdata)
        .then((res)=>{
            $("#modEditProductMaster").modal('hide');
            showToastSuccessMessage(res.data);
            ProductMaster.reloadDatatable();
        })

        
    },


    GetByID_ProductMaster: function (ID, ProductCategory) {      
        var obj = { ProductID: ID, ProductCategory: ProductCategory }
        console.log(obj)
        ProductMaster.OpenEditModal(function () {
            apiCall.ajaxCallWithReturnData(obj, "GET", 'MasterSetup/GetByID_ProductMaster')
                .then((response) => {
                    //console.log(response);
                    //console.log(response.data[0].PK_ProductID)
                    $("#hdnPK_ProductID").val(response.data[0].PK_ProductID);
                    $("#txtProductCategory").val(response.data[0].CategoryName);
                    $("#txtProductCode").val(response.data[0].ProductCode);
                    $("#txtProductName").val(response.data[0].ProductName);
                    $("#txtPackUnit").val(response.data[0].PackUnit);
                    $('#txtProductType').val(response.data[0].ProductCategory);
                    $('#txtProductUOM').val(response.data[0].ProductUOM);
                    $('#txtFactorValue').val(response.data[0].FactorValue);
                    $('#txtBatchSize').val(response.data[0].BatchSize);
                    $('#txtNRVRate').val(response.data[0].NRVRate);
                    $('#txtNRVEffectiveRateFrom').val(response.data[0].NRVEffectiveRateFrom);

                })
        })
    }
  
}


