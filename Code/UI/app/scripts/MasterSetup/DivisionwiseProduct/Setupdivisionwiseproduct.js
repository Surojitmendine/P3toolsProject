var DivisionWiseProduct = {
    Initialize: function () {
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'MasterSetup/DivisionProduct_SearchFields').then(response => {
            if (typeof response != typeof undefined) {                
                DivisionWiseProduct.FillcomboDivision(response.data.divisions)
                DivisionWiseProduct.FillcomboddlDepot(response.data.depots)
                DivisionWiseProduct.FillcomboProduct(response.data.products)
                DivisionWiseProduct.FillcomboPackUnit(response.data.packunits)
            }
        })
    },
    FillcomboDivision: function (data) {
        $('#ddlDivision').empty();
        $('#ddlDivision').select2({
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

    FillcomboddlDepot: function (data) {
        $('#ddlDepot').empty();
        $('#ddlDepot').select2({
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

    FillcomboProduct: function (data) {
        $('#ddlProductName').empty();
        $('#ddlProductName').select2({
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
    FillcomboPackUnit: function (data) {
        $('#ddlPackUnit').empty();
        $('#ddlPackUnit').select2({
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

    ListDivisionWiseProduct: function () {
        var queryparams = {
            Division: $('#ddlDivision').val().join(),
            DepotName: $('#ddlDepot').val().join(),
            Product: $('#ddlProductName').val().join(),
            PackUnit: $('#ddlPackUnit').val().join()                   
        }

        clearDatatable('dtDivisionWiseProductlist')
        apiCall.ajaxCallWithReturnData(queryparams, 'GET', 'MasterSetup/List_Divisionwise_Product')
        .done(function (response) {
            clearDatatable('dtDivisionWiseProductlist')
            if (typeof response !== typeof undefined) {
                DivisionWiseProduct.onSuccess_ListDivisionWiseProduct(response)
            }
    
        })
    },


    onSuccess_ListDivisionWiseProduct: function (response) {        
        var dtDivisionWiseProductlist = $('#dtDivisionWiseProductlist').DataTable({
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
                info: "Showing _START_ - _END_ of _TOTAL_ DivisionWiseProducts",
            },
            aoColumns: [
                {
                    mData: "ID", sTitle: "ID", sClass: "head1", bSortable: true, bVisible: false,
                }, 
                {
                    mData: "DivisionName", sTitle: "Division Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "DepotName", sTitle: "Depot", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductCode", sTitle: "Code", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "ProductName", sClass: "head1", bSortable: true,
                },
                {
                    mData: "PackUnit", sTitle: "PackUnit", sClass: "head1", bSortable: true,
                },
                {
                    mData: "IsActive", sTitle: "Active Product", sClass: "head1", bSortable: true,
                },
                {
                    mData: null, defaultContent: "", sTitle: "Edit", sClass: "head1", bSortable: true,
                    mRender: function (data, type, full) {
                        var markup = '<a href="javascript:void(0)" onclick="DivisionWiseProduct.GetByID_Divisionwise_Product(' + full.ID + ')">Edit</a>'
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
                    var tmptable = $('#dtDivisionWiseProductlist').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Export',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "DivisionWiseProductList",
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
                    var tmptable = $('#dtDivisionWiseProductlist').DataTable();
                    tmptable.buttons().destroy();
                }

            }
        });

    },
    reloadDatatable: function () {
        clearDatatable("dtDivisionWiseProductlist")
        this.ListDivisionWiseProduct();
    },

    initializeControls: function () {       
        return new Promise(function (resolve, reject) {
            resolve(

            )      
            }
        )},
        
    OpenAddModal: function (OpenCallBack) {   
        CreateModal('modAddDivisionWiseProduct', 'pages/MasterSetup/DivisionwiseProduct/divisionwiseproduct-add.html', function () {
            //resetControls("formAddDivisionWiseProduct")
            //DivisionWiseProduct.initializeControls()
                //.then(() => {
                    if (typeof OpenCallBack != typeof undefined) {
                        OpenCallBack()
                        OpenCallBack = undefined
                    }
               // })
        })
    },
//Sajal - Open Confirmation Box for YES- Detele the Row
    OpenDeleteModal: function (OpenCallBack) {          
        CreateModal('modAddDivisionWiseProduct', 'pages/MasterSetup/DivisionWiseProduct/divisionwiseproduct-add.html', function () {
            resetControls("formAddDivisionWiseProduct")
           //DivisionWiseProduct.initializeControls()
                .then(() => {
                    if (typeof OpenCallBack != typeof undefined) {
                        OpenCallBack()
                        OpenCallBack = undefined
                    }
                })
        })
    },


    CloseAddModal: function () {
        $("#modAddDivisionWiseProduct").modal('hide')
        onModalHidden('modAddDivisionWiseProduct', function () {
            resetControls("formAddDivisionWiseProduct")
        })
    },

    AddUpdateDivisionWiseProduct: function () {

        if (fieldValidation('formAddDivisionWiseProduct') == true) {
            console.log('hi')
            if ($("#hdnPK_ID").val() <= 0) {
                apiCall.ajaxCall('formAddDivisionWiseProduct', 'POST', 'MasterSetup/AddNew_Divisionwise_Product')
                    .done(function (response) {
                        if (response.success == 1) {
                           // resetControls("formAddDivisionWiseProduct")
                            DivisionWiseProduct.reloadDatatable();
                            showToastSuccessMessage(response.message)
                        }
                    })
            }

            else if ($("#hdnPK_ID").val() > 0) {
                apiCall.ajaxCall('formAddDivisionWiseProduct', 'POST', 'MasterSetup/Update_Divisionwise_Product')
                    .done(function (response) {
                        if (response.success == 1) {
                            resetControls("formAddDivisionWiseProduct")
                            DivisionWiseProduct.reloadDatatable();
                            showToastSuccessMessage(response.message)
                        }
                    })
            }
        }
        else {
        }
    },

    GetByID_Divisionwise_Product: function (ID) {      
        var obj = { ProductID: ID }
        console.log(obj)
        DivisionWiseProduct.OpenAddModal(function () {
            apiCall.ajaxCallWithReturnData(obj, "GET", 'MasterSetup/GetByID_Divisionwise_Product')
                .then( (response)=> {
                    console.log(response)
                    apiCall.bindModel('formAddDivisionWiseProduct', response.data)
                    $("#chkIsActive").bootstrapSwitch('state', response.data.IsActive);

                })
        })
    },
  
    Delete_Divisionwise_Product:function (ID) {        
        function callbackfunc(result){
            if(result){                
                apiCall.ajaxCall(undefined,'GET','MasterSetup/Delete_Divisionwise_Product',{ProductID:this.ID})
                .then((resp)=>{
                    if(resp.success==1){
                        DivisionWiseProduct.ListDivisionWiseProduct()
                        bootbox.alert("Division's Product deleted successfully.");                        
                    }
                })
            }
        }
        bootbox.confirm({
            message: "Are you sure to delete this entry?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-danger'
                }
            },
            callback:callbackfunc.bind({ProductID:ID})
        });
    },
}


