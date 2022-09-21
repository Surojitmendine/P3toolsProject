var dataprocessing = {

    Initialize: function () {
        common.FillMonthList('ddlMonth');
        common.FillYearList('ddlYear');
    },

    processData: function (sptocall) {
        if ($('#ddlYear').val() == "0") {
            showToastErrorMessage("Year can not be blank.Select Year and Month")
            return false;
        }
        if ($('#ddlMonth').val() == "0") {
            showToastErrorMessage("Month can not be blank.Select Year and Month")
            return false;
        }
       /* if ($('#ddlForecastingType').val() == "0") {
            showToastErrorMessage("Forecasting Type can not be blank.Select Forecasting Type")
            return false;
        }*/

        var ajaxdata = {
            Year: $('#ddlYear').val(),
            Month: $('#ddlMonth').val(),
          //  ForecastingType: $('#ddlForecastingType').val(),
            /* ToDate: $('#dtTodate').val(),*/

            SpToCall: sptocall
        };
       
        apiCall.ajaxCall(undefined, 'GET', 'SetupProcess/DataProcessing', ajaxdata)
            .then((res) => {
                showToastSuccessMessage("Task Completed Successfully.")
                if(sptocall=="DownloadProjection"){
                    tablesToExcel.exportJson([res.data], ['Sheet 1'], 'MonthlyProjection.xls', 'Excel')                                
                }
                
            })            
    }
}