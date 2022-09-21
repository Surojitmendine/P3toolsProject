var ProductMaster = {
    Initialize: function () {
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'MasterSetup/ProductMaster_SearchFields').then(response => {
            if (typeof response != typeof undefined) {                
                ProductMaster.FillcomboCategory(response.data.categories)
            }
        })
    },
    FillcomboCategory: function (data) {
        $('#ddlCategory').empty();
        $('#ddlCategory').select2({
            multiple: true,
            data: data,
            closeOnSelect: true,
            theme: 'bootstrap4',
            placeholder: {
                id: '', // the value of the option
                text: 'Select Category'
            },
        });
    },


    ListProductMaster: function () {
        var queryparams = {
            Division: $('#ddlCategory').val().join(),                 
        }
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
                    mData: "Category", sTitle: "Category", sClass: "head1", bSortable: true,
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
                    mData: "ProductCategory", sTitle: "Group Name", sClass: "head1", bSortable: true,
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
                        var markup = '<a href="javascript:void(0)" onclick="ProductMaster.GetByID_ProductMaster(' + full.PK_ProductID + ')">Edit</a>'
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
                                    columns: [0, 1, 2]
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

    AddUpdateProductMaster: function () {

        if (fieldValidation('formAddProductMaster') == true) {
            console.log('hi')
            if ($("#hdnPK_ID").val() <= 0) {
                apiCall.ajaxCall('formAddProductMaster', 'POST', 'MasterSetup/AddNew_ProductMaster')
                    .done(function (response) {
                        if (response.success == 1) {
                           // resetControls("formAddProductMaster")
                            ProductMaster.reloadDatatable();
                            showToastSuccessMessage(response.message)
                        }
                    })
            }

            else if ($("#hdnPK_ProductID").val() > 0) {
                apiCall.ajaxCall('formAddProductMaster', 'POST', 'MasterSetup/Update_ProductMaster')
                    .done(function (response) {
                        if (response.success == 1) {
                            resetControls("formAddProductMaster")
                            ProductMaster.reloadDatatable();
                            showToastSuccessMessage(response.message)
                        }
                    })
            }
        }
        else {
        }
    },

    GetByID_ProductMaster: function (ID) {      
        var obj = { ProductID: ID }
        console.log(obj)
        ProductMaster.OpenAddModal(function () {
            apiCall.ajaxCallWithReturnData(obj, "GET", 'MasterSetup/GetByID_ProductMaster')
                .then( (response)=> {
                    console.log(response)
                    apiCall.bindModel('formAddProductMaster', response.data)
                    $("#chkIsActive").bootstrapSwitch('state', response.data.IsActive);

                })
        })
    },
  
}


