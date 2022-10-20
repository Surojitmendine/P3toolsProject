var pro_dataprocessing = {

    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
    },

    CallAPI: function (sptocall, customizedtext) {

        customizedtext=(typeof customizedtext==typeof undefined)?'':`Please Press Next ${customizedtext} to Process Planning`
       
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }

        var ajaxdata = {
            Year: $('#ddlYear').val(),
            Month: $('#ddlMonth').val(),
            SpToCall: sptocall
        };
        
        apiCall.ajaxCall(undefined, 'GET', 'ProductionPlan/ProductionPlaning_DataProcessing', ajaxdata)
            .then((res) => {
                console.log(res);
                if (res.success == true) {
                    showToastSuccessMessage(`Task Completed Successfully. ${customizedtext}`)
                }
                else{
                    showToastErrorMessage("Please Complete Previous Process")
                }

            })
    }
}