/* #####################################################################
   #
   #   Project       : Modal Login with jQuery Effects
   #   Author        : Rodrigo Amarante (rodrigockamarante)
   #   Version       : 1.0
   #   Created       : 07/29/2015
   #   Last Change   : 08/04/2015
   #
   ##################################################################### */

$(function () {

    var $formdatevalidation = $('#date-validation-form');
    var $formselectpayor = $('#select-payor-form');

    var $divForms = $('#div-forms');
    var $modalAnimateTime = 300;
    var $msgAnimateTime = 150;
    var $msgShowTime = 2000;

    $("form").submit(function () {
        switch (this.id) {
            case "date-validation-form":
                var $lg_username = $('#login_username').val();
                var $lg_password = $('#login_password').val();
                if ($lg_username == "ERROR") {
                    msgChange($('#div-validation-msg'), $('#icon-validation-msg'), $('#text-validation-msg'), "error", "glyphicon-remove", "Login error");
                } else {
                    msgChange($('#div-validation-msg'), $('#icon-validation-msg'), $('#text-validation-msg'), "success", "glyphicon-ok", "Login OK");
                }
                return false;
                break;
            case "select-payor-form":
                var $payor = $('#txtpayor').val();
                if ($payor == "-Select-") {
                    msgChange($('#div-payor-msg'), $('#icon-payor-msg'), $('#text-payor-msg'), "error", "glyphicon-remove", "Send error");
                } else {
                    msgChange($('#div-payor-msg'), $('#icon-payor-msg'), $('#text-payor-msg'), "success", "glyphicon-ok", "Send OK");
                    $('#ModalDate').modal("hide");
                    $('#txtdate_trans_main').val($('#txtdate_trans').val());
                    $('#txtpayor_id').val($('#txtpayor').val());
                    $('#txtpayor_name').val($('#txtpayor').find('option:selected').text());
                    getReferenceNo.InitializeEvents();
                    LoanPrincipalDue.InitializeEvents();
                    LoanInterestDue.InitializeEvents();
                    //Load();
                }
                return false;
                break;
            default:
                return false;
        }
        return false;
    });

    $('#btn-proceed-payor').click(function () { modalAnimate($formdatevalidation, $formselectpayor) });

    function modalAnimate($oldForm, $newForm) {

        var $oldH = $oldForm.height();
        var $newH = $newForm.height();
        $divForms.css("height", $oldH);
        $oldForm.fadeToggle($modalAnimateTime, function () {
            $divForms.animate({ height: $newH }, $modalAnimateTime, function () {
                $newForm.fadeToggle($modalAnimateTime);
            });
        });
    }

    function msgFade($msgId, $msgText) {
        $msgId.fadeOut($msgAnimateTime, function () {
            $(this).text($msgText).fadeIn($msgAnimateTime);
        });
    }

    function msgChange($divTag, $iconTag, $textTag, $divClass, $iconClass, $msgText) {
        var $msgOld = $divTag.text();
        msgFade($textTag, $msgText);
        $divTag.addClass($divClass);
        $iconTag.removeClass("glyphicon-chevron-right");
        $iconTag.addClass($iconClass + " " + $divClass);
        setTimeout(function () {
            msgFade($textTag, $msgOld);
            $divTag.removeClass($divClass);
            $iconTag.addClass("glyphicon-chevron-right");
            $iconTag.removeClass($iconClass + " " + $divClass);
        }, $msgShowTime);
    }
});

