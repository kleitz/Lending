var Helper = (function () {
    "use strict";

    var _parseDate = function (jsonDate) {
        //console.log(jsonDate);
        if (jsonDate == null) {
            return "";
        }
        else if (jsonDate == "") {
            return "";
        }
        else if (typeof (jsonDate) === 'undefined') {
            return "";
        }
        else {
            var re = /-?\d+/;
            var m = re.exec(jsonDate);
            var d = new Date(parseInt(m[0]));
            var curr_date = d.getDate();
            var curr_month = d.getMonth() + 1; //Months are zero based
            var curr_year = d.getFullYear();
            return curr_month + "/" + curr_date + "/" + curr_year;
        }
    };

    var _toastr = function () {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": false,
            "positionClass": "toast-top-center",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    };

    // targetId = element id from dom
    var _openModal = function (targetId) {
        Custombox.open({
            target: targetId,
            animation: 'blur',
            overlayColor: '#36404a',
            overlaySpeed: 300,
            loading: true
        });
    };

    /// msgText = shows message body confirmation; msgType = [success,warning,info,error]
    var _sweetAlert = function (msgText, msgType) {
        swal({
            title: "Are you sure?",
            text: msgText,
            type: msgType,
            showCancelButton: true,
            confirmButtonClass: 'btn-warning',
            confirmButtonText: "Continue anyway",
            closeOnConfirm: false
        });
    };

    var _addCommas = function (nStr) {
        nStr += '';
        var x = nStr.split('.', 2);
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    };

    var _deleteComma = function (MyString) {
        //Replace Comma By Null....
        var afterreplace = MyString.replace(',', '');
        return (afterreplace);
    }

    return {
        parseDate: _parseDate,
        toastr: _toastr,
        sweetAlert: _sweetAlert,
        openModal: _openModal,
        addCommas: _addCommas,
        deleteComma: _deleteComma
    };
}());


// time helper
//s - seconds
//ss - seconds, 2 digits
//m - minutes
//mm - minutes, 2 digits
//h - hours, 12-hour format
//hh - hours, 12-hour format, 2 digits
//H - hours, 24-hour format
//HH - hours, 24-hour format, 2 digits
//d - date number
//dd - date number, 2 digits
//ddd - date name, short
//dddd - date name, full
//M - month number
//MM - month number, 2 digits
//MMM - month name, short
//MMMM - month name, full
//yy - year, 2 digits
//yyyy - year, 4 digits
//t - 'a' or 'p'
//tt - 'am' or 'pm'
//T - 'A' or 'P'
//TT - 'AM' or 'PM'
//u - ISO8601 format
//S - 'st', 'nd', 'rd', 'th' for the date
//W - the ISO8601 week number