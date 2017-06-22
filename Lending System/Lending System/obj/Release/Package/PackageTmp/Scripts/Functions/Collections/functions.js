$(document).ready(function () {
    ShowModalValidation();

    $('input.number').number(true, 2);
    $('span.number').number(true, 4);

    if (RootUrl == "/") {
        RootUrl = ""
    }
});

function ShowModalValidation() {
    $('#ModalDate').modal('show');
    $('#txtdate_trans').val(mm + "/" + dd + "/" + yyyy);

    setTimeout(function () {
    }, 300);
};

function ShowModalPayment() {
    if ($('#btn-payment').val() == "Proceed to payment") {
        $('#ModalPayment').modal('show');
        Balance();

        $('#txtpayment_modal').focus();
        setTimeout(function () {
        }, 300);
    }
    else {
        AjaxSave();
        //TryDeferred();
    }
};
function HideModalPayment() {
    $('#ModalPayment').modal('hide');

    setTimeout(function () {
    }, 300);
};

function ProceedModalPayment() {
    $('#ModalPayment').modal('hide');

    setTimeout(function () {
    }, 300);

    PrepareSave();
};

function PrepareSave() {
    var payment = 0;
    payment = parseFloat($('#txtpayment_modal').val());
    var rowText;

    $('#btn-payment').val("Save");
    $("#btn-payment").removeClass("btn btn-info pull-right").addClass("btn btn-success pull-right");
    
    //Interest
    var interestpayment = 0;
    var totalRowCount = $("#interest-payment-table tr").length;
  
    for (var i = 1; i < totalRowCount; i++) {
        rowText = document.getElementById("interest-payment-table").rows[i].cells[0].innerText;

        if (rowText != "No data available in table") {
            var amountdue = document.getElementById("interest-payment-table").rows[i].cells[3].innerText;
            var amountdue_number = parseFloat(amountdue.replace(/[^0-9\.]+/g, ""));

            if (payment >= amountdue_number) {

                $('#interest-payment-table').dataTable().fnUpdate(amountdue_number, $("#interest-payment-table tr")[i], 4);
                interestpayment = interestpayment + amountdue_number;
                payment = payment - amountdue_number;
            }
            else if (payment < amountdue_number) {

                $('#interest-payment-table').dataTable().fnUpdate(parseFloat(Math.round(payment * 100) / 100).toFixed(2), $("#interest-payment-table tr")[i], 4);
                interestpayment = interestpayment + payment;
                payment = payment - payment;
            }
        }
    }
    
    $('#txt_interest_payment').val(parseFloat(interestpayment));


    //Principal
    var principalpayment = 0;
    var totalRowCount = $("#principal-payment-table tr").length;

    for (var i = 1; i < totalRowCount; i++) {
        rowText = document.getElementById("principal-payment-table").rows[i].cells[0].innerText;

        if (rowText != "No data available in table") {
            var amountdue = document.getElementById("principal-payment-table").rows[i].cells[3].innerText;
            var amountdue_number = parseFloat(amountdue.replace(/[^0-9\.]+/g, ""));

            if (payment >= amountdue_number) {

                $('#principal-payment-table').dataTable().fnUpdate(amountdue_number, $("#principal-payment-table tr")[i], 4);
                principalpayment = principalpayment + amountdue_number;
                payment = payment - amountdue_number;
            }
            else if (payment < amountdue_number) {

                $('#principal-payment-table').dataTable().fnUpdate(parseFloat(Math.round(payment * 100) / 100).toFixed(2), $("#principal-payment-table tr")[i], 4);
                principalpayment = principalpayment + payment;
                payment = payment - payment;
            }
        }
    }
    $('#txtprincipal_payment').val(parseFloat(principalpayment));

    $('#txttotal_payment').val(parseFloat(principalpayment) + parseFloat(interestpayment));
}


$('#txtpayment_modal').on('keyup', function () {
    var bal = 0.00;
    var pay = 0.00;

    bal = $('#txtamount_due_modal').val();
    pay = $('#txtpayment_modal').val();
    
    if (parseFloat(bal) < parseFloat(pay)) {
        $('#txtpayment_modal').val(bal);
    }
})

//Request for Reference No.
var interval = 1000;
var getReferenceNo =
    {
        InitializeEvents: function () {
            $.ajax({
                url: RootUrl + "/Collections/getReferenceNo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) { $('#txtreference_no').val(response) },
                complete: function (response) {
                    // Schedule the next
                    setTimeout(getReferenceNo.InitializeEvents, interval);
                },
                error: ""
            });
        }
    }