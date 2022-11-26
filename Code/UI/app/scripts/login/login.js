var login = {

    /**
 * notification configuration
 */
    Toast: Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    }),
    logintoSystem: function () {

        $("#btnLogin").prop('disabled', true)
        $("#loginloader").show().addClass('fa-spin')

        var ajaxdata = {
            uname: $('#txtemail').val(),
            pwd: $('#txtpassword').val()
        };

        console.log(ajaxdata);
        
        apiCall.ajaxCallWithReturnData(ajaxdata, 'POST', 'Auth/ValidLogin')
            .done(function (res) {
                console.log(res)
                 if (res.success == true) {
                    sessionStorage.setItem('sysdate', moment(new Date()).format('DD/MM/YYYY'))
                    sessionStorage.setItem("username", res.data[0].Empemail)
                    window.location.href = "/index.html"
                    //console.log("Pallab");
                }
                else{
                    $("#loginloader").hide().removeClass('fa-spin')
                    $("#btnLogin").prop('disabled', false)
                    login.runEffect()
                    login.Toast.fire({
                        type: 'error',
                        title: 'Invalid Username or Passrord',
                    })
                }

            })
            
    },

    runEffect: function () {
        {
            $("#divlogin").effect('shake', null, 500, () => {
                setTimeout(function () {
                    $("#divlogin").removeAttr("style").hide().fadeIn();
                }, 1000);
                return false;
            });
        };
    },

    GetBingImageofTheDay: function () {
        return new Promise((resolve, reject) => {
            apiCall.ajaxCallWithReturnData(undefined, 'GET', 'Common/GetBingImageofTheDay')
                .then((res) => {
                    if (typeof res !== typeof undefined) {
                        localStorage.setItem('bgimage', res.data.imageurl)
                        localStorage.setItem('bgimage_source', res.data.source)
                        return res.data.imageurl;
                    }
                })
                .then((imgurl)=>{
                    resolve(imgurl)
                    
                })                               
            })
    },
}