var dashboard = {
    GetMeterReading: function () {
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Dashboard/GetMeterReading')
            .then((response) => {
                return response.data;
            })
            .then((data) => {
                //console.log(data)
                let months = []
                Array.from(new Set(data.map(s => s.Month)))
                    .map(month => {
                        months.push(month)
                    })
            })
    },

    ListCreditSale: function () {

        clearDatatable('creditsalelist')
        apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Dashboard/CreditSalesList')
            .then((response) => {
                if (typeof response != typeof undefined) {
                    dashboard.onSuccess_ListCreditSale(response.data)
                }

            })
    },

    onSuccess_ListCreditSale: function (data) {

        var dtVaultTransfer = $('#creditsalelist').DataTable({
            bServerSide: false,
            bDestroy: true,
            paging: true,
            autoWidth: false,
            bStateSave: false,
            searching: false,
            data: data,
            lengthChange: false,
            info: false,
            pageLength:10,
            aaSorting: [],
            language: {
                paginate: {
                    previous: "<",
                    next: ">"
                },
                info: "Showing _START_ - _END_ of _TOTAL_ readings",
            },
            aoColumns: [
                {
                    mData: "Invoice_Date", sTitle: "Date", sClass: "head1",
                },
                {
                    mData: 'Staff', sTitle: "By", sClass: "head1",
                },

                {
                    mData: 'ExpensesHead', sTitle: "ExpensesHead", sClass: "head1",
                },
                {
                    mData: 'Amount', sTitle: "Amount", sClass: "head1",
                },


            ],
            "dom": '<<"float-right"l>f<t><"#df"<"float-left" i><"float-right pagination pagination-sm p-1"p>>>',
            "lengthChange": true,
            "lengthMenu": [[10, 20, 30, -1], [10, 20, 30, "All"]],
            bUseRendered: true,
            drawCallback: function () {
                $('.dataTables_paginate > .pagination').addClass('pagination pagination-sm m-0 float-right');
            }

        });


    },

    FillMonthList: function () {

        common.MonthList().then((res) => {
            
            $('#ddlMonth').empty();
            $('#ddlMonth').prepend('<option value="0" selected> Select Month </option>')
            $('#ddlMonth').select2({
                multiple: false,
                data: res.data,
                closeOnSelect: true,
                theme: 'bootstrap4'
            });

        })


       

    },
}