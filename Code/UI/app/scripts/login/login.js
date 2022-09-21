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
        apiCall.ajaxCall('formLogin', 'GET', 'Auth/token')
            .done((data, textStatus, jqXHR) => {

                $("#loginloader").hide().removeClass('fa-spin')
                $("#btnLogin").prop('disabled', false)
                if (data.access_token) {

                    sessionStorage.setItem("token", data.access_token)
                    sessionStorage.setItem('sysdate', moment(new Date()).format('DD/MM/YYYY'))
                    window.location.href = "index.html"
                    //page.redirect('./');
                }
            })
            .fail((jqXHR, textStatus, errorThrown) => {
                /*console.log(jqXHR)
                console.log(textStatus)
                console.log(errorThrown)*/
                $("#loginloader").hide().removeClass('fa-spin')
                $("#btnLogin").prop('disabled', false)
                login.runEffect()
                login.Toast.fire({
                    type: 'error',
                    title: 'Invalid Username or Passrord',
                })
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