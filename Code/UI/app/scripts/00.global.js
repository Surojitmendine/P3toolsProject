

/*
//http://stackoverflow.com/a/14081487
jQuery(function ($) {
    $.validator.addMethod('date',
    function (value, element) {
        if (this.optional(element)) {
            return true;
        }
        //alert(1)

        var ok = true;
        try {
            // console.log(new Date(value))
            //element.parseDate('DD/MM/YYYY', value);
        }
        catch (err) {
            console.log(err)
            ok = false;
        }
        return ok;
    });
});

jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "date-uk-pre": function (a) {
        var ukDatea = a.split('/');
        return (ukDatea[2] + ukDatea[1] + ukDatea[0]) * 1;
    },

    "date-uk-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "date-uk-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});

$.fn.dataTable.moment('DD/MM/YYYY HH:mm');*/


/*
NProgress.configure({ trickle: true, showSpinner: true });
*/
/*$(document).ajaxStart(function () {
    NProgress.start();
    console.log('ajaxStart')
     $("#root").faLoading('fa-sync-alt');
});
$(document).ajaxStop(function () {
    NProgress.done();    
    console.log('ajaxStop')
    $("#root").faLoadingStop();
});


$(document).ajaxComplete(function (event, xhr, settings) {
    //console.log('ajaxComplete')

});*/

moment.locale('en');
/**
 * scripts array for all pages 
 */
//
var scripts = [
    'framework-scripts.js'

]

/*window.onerror = function (msg, url, lineNo, columnNo, error) {
    var errmsg = msg.toLowerCase();
    var substring = "script error";
    if (typeof errmsg==typeof this.String && errmsg.indexOf(substring) > -1) {
        alert('Script Error: See Browser Console for Detail');
    } else {
        var message = [
            'Message: ' + msg,
            'URL: ' + url,
            'Line: ' + lineNo,
            'Column: ' + columnNo,
            'Error object: ' + JSON.stringify(error)
        ].join(' - ');

        alert(message);
    }

    return false;
};*/

/*$.Deferred.exceptionHook = function (err, stackTrace) {
    // 'err' is what you throw in your deferred's catch.
    window.dispatchEvent(new CustomEvent('error', {
        detail: err
    }));
}*/

NProgress.configure({ showSpinner: false });

/**
 * 
 */
$.getMultiScripts = function (arr, path) {

    var _arr = $.map(arr, function (scr) {
        return $.getScript((path || "") + scr);
    });

    _arr.push($.Deferred(function (deferred) {
        $(deferred.resolve);
    }));

    return $.when.apply($, _arr);
}




window.onload = function () {
    //console.log('window.onload')

}


/*This Now added in Index.html
$.getMultiScripts(scripts, "").then(function () {
    console.log("all scripts loaded");
    loadHtml("pages/home.html")
    addnavigation();
});
*/





$.ajaxSetup({
    cache: false,
    beforeSend: function (xhr) {

        //$("#root").faLoading('fa-sync-alt');
        xhr.setRequestHeader('Access-Control-Allow-Origin', null);
        xhr.setRequestHeader('Access-Control-Allow-Headers', '*');
        xhr.setRequestHeader('Authorization', 'Bearer ' + sessionStorage.getItem("token"));
        //xhr.setRequestHeader('Cache-Control', 'private');
    },
    complete: function () {
        //console.log('ajaxSetup-complete')
        //$("#root").faLoadingStop();
    }
});

$(document).ajaxStart(function () {
    $("#root").faLoading('fa-sync-alt');
    NProgress.start();
});

$(document).ajaxStop(function () {
    NProgress.done();
    $("#root").faLoadingStop();
});

/*$(document).ajaxComplete(function (event, xhr, settings) {
    $("#root").faLoadingStop();
    NProgress.done();
    

});*/


$(document).ajaxError(function (event, jqxhr, settings, thrownError) {
    NProgress.done();

    switch (jqxhr.status) {
        case 401:
            $('#root').load('pages/shared/mod-session-lost.html', function (responseText, textStatus, jqXHR) {
                $("#modal-danger").modal({ backdrop: 'static' })
                    .on('shown.bs.modal', function (e) {

                        $("#modal-danger #sessionlost").show()
                    }).modal('show')
            })
            break;
    }
    return false;
});

/**
 * Load HTML with ajax into index.html=>root div
 * @param {*} FilePath 
 */
function loadHtml(FilePath, element) {

    $('a.nav-link').each(function (index, el) {
        $(el).removeClass('active')
    })
    $(element).addClass('active')
    $('#root').empty()
    return new Promise(function (resolve, reject) {
        resolve(
            $('#root').load(FilePath, function (responseText, textStatus, jqXHR) {
                // console.log(responseText)
                // console.log(textStatus)
                // console.log(jqXHR)
                // $('h1.text-dark').removeClass('text-dark').addClass('text-white')

                validateRequiredField();
                fieldValidationNumeric()
                createDatePicker()
                createTimePicker()
                $("input:text").on('click', function () {                
                    $(this).select();
                });
                
            })
        )
    })


}
/**
 * add a click event listner to menu link for loading HTML 
 */
function addnavigation() {
    $("a.nav-link").each(function (index, element) {
        //console.log(element)
        var attr = $(element).attr('data-filepath');
        //console.log("Pallab " + attr)
        // For some browsers, `attr` is undefined; for others,
        // `attr` is false.  Check for both.
        if (typeof attr !== typeof undefined && attr !== false) {
            $(element).on('click', function () {
                loadHtml(attr, element).then(() => {
                    console.info('Page Loaded !!!!')
                })
            })
        }

    })
}


/**
 * check a datatable instance exists or not
 */
isDataTableExits = function (dataTableID) {
    return $.fn.DataTable.isDataTable("#" + dataTableID + "");
}
/**
 * empty datatable instance
 */
emptyDataTable = function (dataTableID) {
    $("#" + dataTableID + "").empty();
}

/**
 * destroy datatable instance
 */
destroyDataTable = function (dataTableID) {
    //  $("#" + dataTableID + "").dataTable().fnDestroy();
    //https://datatables.net/forums/discussion/comment/93067/#Comment_93067
    $("#" + dataTableID + "").DataTable().destroy();
}

clearDatatable = function (dataTableID) {
    if (isDataTableExits(dataTableID) == true) {
        destroyDataTable(dataTableID)
        emptyDataTable(dataTableID)
    }
}

/**
 * notification configuration
 */
var toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000
});

showToastSuccessMessage = function (message) {
    toast.fire({
        type: 'success',
        title: message,
    })
}

showToastErrorMessage = function (message) {
    toast.fire({
        type: 'error',
        title: message,
    })
}
/**
 * clear form's element
 */
resetControls = function (formid/*, objFieldsValue*/) {

    /*
        assign objFieldsValue with some value when need to set value into field after reset
        checkboxValue => true/false ; default =>true
        dropdownValue => selectedvalue [any numeric] ;  default =>0
    */
    var fieldDefaultValue = {
        checkboxValue: true,
        dropdownValue: 0,
        selectedIndex: 0
    }

    /* if ('undefined' !== typeof objFieldsValue) {
         for (var i in objFieldsValue) {
             if ('undefined' !== typeof objFieldsValue[i]) {
                 fieldDefaultValue[i] = objFieldsValue[i];
             }
         }
     }*/
    $('.validationerror').remove()
    $('#' + formid).find(':input').each(function (index, element) {
        /*
        In future We can bellow values
        console.log(element)
        console.log($(element).is("input"))
        console.log($(element).get(0).tagName)
        console.log($(element).attr('id'))
        console.log($(element).attr('type'))            
       */

        //if ($(element).attr('type') != 'hidden') {
        switch ($(element).get(0).tagName) {
            case 'INPUT':
                if (typeof ($(element).attr('type')) == 'undefined' || $(element).attr('type') == 'text'
                    || $(element).attr('type') == 'email' || $(element).attr('type') == 'tel'
                    || $(element).attr('type') == 'password') {
                    $(element).val('');
                    $(element).removeClass('is-invalid')
                }
                else {
                    switch ($(element).attr('type')) {
                        case 'checkbox':
                            $(element).prop('checked', false)
                            break;
                        case 'hidden':
                            $(element).val('0');

                            break;
                    }
                }
                break;
            case 'SELECT':

                $(element).val(fieldDefaultValue.dropdownValue).trigger('change');
                /*if (fieldDefaultValue.selectedIndex != "") {
                    $(element)[0].selectedIndex = fieldDefaultValue.selectedIndex;
                }
                else {
                    $(element).val(fieldDefaultValue.dropdownValue).trigger('change');
                }

                if ('undefined' !== typeof $(element).attr('combodefaultvalue')) {
                    $(element).val($(element).attr('combodefaultvalue')).trigger('change');
                }*/

                break;
            case 'TEXTAREA':
                $('.editor').summernote('reset');
                $(element).val('');
                break;
        }
        //}
    })
}
/**
 * creating new modal
 * need to add bellow markup on which page need the modal
 * <div class="modal fade" role="dialog" id="modAddSample" aria-hidden="true">
        <div class="modal-dialog">
          <div class="modal-content">

          </div>
        </div>
      </div>

     ModalID=> id="modAddSample"
     FilePath => file location from which modal content will creating
     CreateModalCallBack => method fires after showing the modal
 */
CreateModal = function (ModalID, FilePath, CreateModalCallBack) {
    $.get(FilePath, function (responseText, textStatus, jqXHR) {

        $('#root').append(responseText)
        var modal = $("#" + ModalID).modal({ backdrop: 'static' })

            .on('shown.bs.modal', function (e) {
                validateRequiredField();
                fieldValidationNumeric();
                createDatePicker();
                createTimePicker();                
                $("input:text").on('click', function () {                
                    $(this).select();
                });

                if (typeof CreateModalCallBack != typeof undefined) {
                    CreateModalCallBack()
                    CreateModalCallBack = undefined
                }
            })
            .on('hidden.bs.modal', function (e) {
                $(modal).remove()
            })
            .modal('show')

    })

}

onModalShown = function (ModalID, ShownCallBack) {

    $('#' + ModalID).on('shown.bs.modal', function (e) {
        // do something...
        if (typeof ShownCallBack != typeof undefined) {
            ShownCallBack()
        }
    })
}
onModalHidden = function (ModalID, HiddenCallback) {
    $('#' + ModalID).on('hidden.bs.modal', function (e) {
        if (typeof HiddenCallback != typeof undefined) {
            HiddenCallback()
        }
    })
}
/**
 * call before submit a form to check any required field empty or not.
 * add required prop to element to mark as required
 */
fieldValidation = function (parentForm) {

    var invalidcount = 0
    var invalidelement
    $('#' + parentForm).find(':input').each(function (index, element) {
        //console.log($(element))
        if ($(element).attr('type') != "hidden") {

            if ($(element).prop('required') && ($(element).val() == "" || $(element).val() == 0)) {

                invalidelement = $(element)

                invalidcount++
                return false;
            }

            if ($(element).prop('numeric') == true && $.isNumeric($(element).val()) === false) {
                invalidelement = $(element)

                invalidcount++
                return false;
            }

        }
        ;
    })

    if (invalidcount > 0) {

        let validationmessage = document.getElementById('' + $(invalidelement).attr('id') + '').hasAttribute('data-validationmessage') == true ?
            $(invalidelement).attr('data-validationmessage')
            : 'Required field.Enter value.'

        $(invalidelement).addClass('is-invalid')

        $('.validationerror').remove()
        $(invalidelement).parent().append('<label class="validationerror text-danger">' + validationmessage + '</label>')

        /*$('<label class="validationerror text-danger">' + validationmessage + '</label>')
            .insertAfter($(invalidelement))*/

        $('.modal').animate({
            scrollTop: $(invalidelement).offset().top
        }, 2000);
        $(invalidelement).focus()
        return false
    }
    else if (invalidcount == 0) {
        return true
    }
}


/**
 * call before submit a form to check any required field empty or not.
 * add required prop to element to mark as required
 */
specificFieldValidation = function (controls) {

    var invalidcount = 0
    var invalidelement
    // controls.forEach(function (element, index) {
    $.each(controls, function (index, element) {
        //console.log($(element))
        element = $('#' + element)

        if ($(element).attr('type') != "hidden") {

            if ($(element).prop('required') && ($(element).val() == "" || $(element).val() == 0)) {
                invalidelement = $(element)
                invalidcount++
                return false;
            }

            if ($(element).prop('numeric') == true && $.isNumeric($(element).val()) === false) {
                invalidelement = $(element)
                invalidcount++
                return false;
            }

        };
    })


    if (invalidcount > 0) {

        let validationmessage = document.getElementById('' + $(invalidelement).attr('id') + '').hasAttribute('data-validationmessage') == true ?
            $(invalidelement).attr('data-validationmessage')
            : 'Required field.Enter value.'

        $(invalidelement).addClass('is-invalid')

        $('.validationerror').remove()
        $(invalidelement).parent().append('<label class="validationerror text-danger">' + validationmessage + '</label>')

        /*$('<label class="validationerror text-danger">' + validationmessage + '</label>')
            .insertAfter($(invalidelement))*/

        $('.modal').animate({
            scrollTop: $(invalidelement).offset().top
        }, 2000);
        $(invalidelement).focus()
        return false
    }
    else if (invalidcount == 0) {
        return true
    }
}



/**
 * on document.ready() inputs with numeric prop , a keypress hendler added to allow only numeric value to elemnts.
 * add numeric prop to element to mark as numeric field
 */
fieldValidationNumeric = function () {
    $('input[type=text]').each(function (index, element) {
        if (typeof $(element).attr('numeric') != typeof undefined) {

            $(element).on('keypress', function (e) {
                if (e.which != 8 && e.which != 0 && e.which != 46 && (e.which < 48 || e.which > 57)) {

                    return false;
                }
            })
        }

    })
}
/**
 * on document.ready()  , a onchange hendler added to check inputs with required prop has value
 * and then remove validation error. 
 */
validateRequiredField = function () {
    $('input,select').each(function (index, element) {
        if ($(element).attr('type') != "hidden") {

            $(element).on('change', function () {
                if ($(element).prop('required') && ($(element).val() != "" || $(element).val() > 0)) {
                    $(element).removeClass('is-invalid')
                    $('.validationerror').remove()

                }
            })
        }
    })
}
/**
 * creating datepicker .
 * ElementID => on which elemt datepicker is needed.
 */
createDatePickerByID = function (ElementID) {

    $('#' + ElementID).datetimepicker({
        format: 'DD/MM/YYYY',
        useCurrent: false,
        allowInputToggle: true,
    })
}

createDatePicker = function () {

    $('div.date').each(function (index, element) {
        //console.log($(element).hasClass('time'))
        if (!$(element).hasClass('time')) {
            $(element).datetimepicker({
                defaultDate: sessionStorage.getItem('sysdate') == null ? new Date() : moment(sessionStorage.getItem('sysdate'), "DD/MM/YYYY"),
                format: $(element).hasClass('time') ? 'LT' : 'DD/MM/YYYY'
            }).on('dp.change', function (e) {
                $(this).find(":input").val(moment(e.date).format('DD/MM/YYYY'))
                $(this).find(":input").trigger("change");

            });
        }
    })

}

createTimePicker = function () {
    //console.log($('div.date.time'))
    $('div.date.time').datetimepicker({
        format: 'LT'
    });
}

GetQueryStringParams = function (sParam) {

    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return decodeURIComponent(sParameterName[1]);
        }
    }
}

openErrorModal = function () {
    $.get('pages/shared/mod-gen-error.html', function (responseText, textStatus, jqXHR) {

        $('#root').append(responseText)
        $("#modal-danger").modal({ backdrop: 'static' })
            .modal('show')

    })
}

parseDecimal = function (value) {
    //https://stackoverflow.com/questions/2304052/check-if-a-number-has-a-decimal-place-is-a-whole-number
    var parsedvalue = $.isNumeric(value) == true ? parseFloat(value) : 0;
    var roundvalue = (parsedvalue % 1) != 0 ? parsedvalue : Math.round(parsedvalue).toFixed(2);
    return roundvalue;
}



sortListofObjectArray = (field, reverse, primer) => {
    //https://stackoverflow.com/a/979325/4336330
    const key = primer ?
        function (x) {
            return primer(x[field])
        } :
        function (x) {
            return x[field]
        };

    reverse = !reverse ? 1 : -1;

    return function (a, b) {
        return a = key(a), b = key(b), reverse * ((a > b) - (b > a));
    }
}
