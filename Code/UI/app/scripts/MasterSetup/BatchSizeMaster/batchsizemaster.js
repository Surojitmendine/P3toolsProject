var BatchSizeMaster = {

    ListBatchSizeMaster: function () {

        clearDatatable('dtBatchSizeMasterlist')
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'MasterSetup/List_BatchSizeMaster')
        .done(function (response) {
            clearDatatable('dtBatchSizeMasterlist')
            if (typeof response !== typeof undefined) {
                BatchSizeMaster.onSuccess_ListBatchSizeMaster(response)
            }
    
        })
    },


    onSuccess_ListBatchSizeMaster: function (response) {        
        var dtBatchSizeMasterlist = $('#dtBatchSizeMasterlist').DataTable({
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
                    mData: "BatchSizeID", sTitle: "ID", sClass: "head1", bSortable: true, bVisible: false,
                }, 
                {
                    mData: "ProductType", sTitle: "Product Type", sClass: "head1", bSortable: true,
                },
                {
                    mData: "ProductName", sTitle: "Product Name", sClass: "head1", bSortable: true,
                },
                {
                    mData: "UOM", sTitle: "UOM", sClass: "head1", bSortable: true,
                },   
                {
                    mData: "BatchSize", sTitle: "Batch Size", sClass: "head1", bSortable: true,
                },
                {
                    mData: null, defaultContent: "", sTitle: "Action", sClass: "head1", bSortable: true,
                    mRender: function (data, type, full) {
                        console.log("hi")
                        var markup = '<a href="javascript:void(0)" onclick="BatchSizeMaster.GetByID_BatchSizeMaster(' + full.BatchSizeID +  ')">Edit</a>'
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
                    var tmptable = $('#dtBatchSizeMasterlist').DataTable();
                    tmptable.buttons().destroy();
                    new $.fn.DataTable.Buttons(tmptable, {
                        buttons: [
                            {
                                text: 'Export',
                                extend: 'excel',
                                className: 'btn btn-info float-right fas fa-file-excel',
                                title: "BatchSizeMasterList",
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
                    var tmptable = $('#dtBatchSizeMasterlist').DataTable();
                    tmptable.buttons().destroy();
                }

            }
        });

    },
    reloadDatatable: function () {
        clearDatatable("dtBatchSizeMasterlist")
        this.ListBatchSizeMaster();
    },

    initializeControls: function () {       
        return new Promise(function (resolve, reject) {
            resolve(

            )      
            }
        )},
        
    OpenAddModal: function (OpenCallBack) {   
        CreateModal('modAddBatchSizeMaster', 'pages/MasterSetup/BatchSizeMaster/batchsizemaster-add.html', function () {
                    if (typeof OpenCallBack != typeof undefined) {
                        OpenCallBack()
                        OpenCallBack = undefined
                    }
        })
    },

    CloseAddModal: function () {
        $("#modAddBatchSizeMaster").modal('hide')
        onModalHidden('modAddBatchSizeMaster', function () {
            resetControls("formAddBatchSizeMaster")
        })
    },

    OpenEditModal: function (OpenCallBack) {   
        CreateModal('modEditBatchSizeMaster', 'pages/MasterSetup/BatchSizeMaster/batchsizemaster-edit.html', function () {
                    if (typeof OpenCallBack != typeof undefined) {
                        OpenCallBack()
                        OpenCallBack = undefined
                    }
        })
    },

    CloseEditModal: function () {
        $("#modEditBatchSizeMaster").modal('hide')
        onModalHidden('modEditBatchSizeMaster', function () {
            resetControls("formEditBatchSizeMaster")
        })
    },

    AddBatchSizeMaster: function () {

        var ajaxdata = {
            ProductType: $('#txtProductType').val(),
            ProductName: $('#txtProductName').val(),
            UOM: $('#txtUOM').val(),
            BatchSize: $('#txtBatchSize').val()

        };
        //console.log(ajaxdata);

        apiCall.ajaxCall(undefined, 'POST', 'MasterSetup/AddNew_BatchSizeMaster',ajaxdata)
        .then((res)=>{
            console.log(res.success + ',' + res.data);
            $("#modAddBatchSizeMaster").modal('hide');
            if(res.success == 1){
                showToastSuccessMessage(res.data);
            }
            else{
                showToastErrorMessage(res.data);
            }
            BatchSizeMaster.reloadDatatable();
        })

        
    },

    EditBatchSizeMaster: function () {

        var ajaxdata = {
            BatchSizeID: $('#hdnPK_BatchSizeID').val(),
            ProductType: $('#txtProductType').val(),
            ProductName: $('#txtProductName').val(),
            UOM: $('#txtUOM').val(),
            BatchSize: $('#txtBatchSize').val()
            
        };
        //console.log(ajaxdata);

        apiCall.ajaxCall(undefined, 'POST', 'MasterSetup/Update_BatchSizeMaster',ajaxdata)
        .then((res)=>{
            $("#modEditBatchSizeMaster").modal('hide');
            showToastSuccessMessage(res.data);
            BatchSizeMaster.reloadDatatable();
        })

        
    },

    GetByID_BatchSizeMaster: function (ID) {
        var obj = { BatchSizeID: ID }
        //console.log(obj)
        BatchSizeMaster.OpenEditModal(function () {
            apiCall.ajaxCallWithReturnData(obj, "GET", 'MasterSetup/GetByID_BatchSizeMaster')
                .then((response) => {
                    //console.log(response.data[0].PK_ProductID)
                    //apiCall.bindModel('formEditProductMappingMaster', response.data)
                    $("#hdnPK_BatchSizeID").val(response.data[0].BatchSizeID);
                    $("#txtProductName").val(response.data[0].ProductName);
                    $('#txtProductType').val(response.data[0].ProductType);
                    $('#txtUOM').val(response.data[0].UOM);
                    $('#txtBatchSize').val(response.data[0].BatchSize)
                })
        })
    }
    
  
}


