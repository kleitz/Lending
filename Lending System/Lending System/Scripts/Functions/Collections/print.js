function ShowModalPrint() {
    $('#ModalPrint').modal('show');
    LoadReceipt();
    setTimeout(function () {
    }, 300);
};

function HideModalPrint() {

    $('#ModalPrint').modal('hide');
    window.location.href = RootUrl + "/Collections/Index";

    setTimeout(function () {
    }, 300);
}

function Print(divId) {
    $('#ModalPrint').modal('hide');

    setTimeout(function () {
        showPrintDialog(divId);
    }, 500);

}
function showPrintDialog(div_id) {
    //var printContents = document.getElementById(div_id).innerHTML;
    //var originalContents = document.body.innerHTML;

    //document.body.innerHTML = printContents;

    //window.print();
    //setTimeout(function () { window.print(); }, 500);

    //document.body.innerHTML = originalContents;

    //window.onfocus = function () { setTimeout(function () { window.close(); }, 500); }

    //window.location.href = RootUrl + "/Collections/Index";

    var content = document.getElementById(div_id);
    var mapSrc = window.open("", "PRINT MAP", "width=200,top=0,left=0,toolbar=no,scrollbars=no,status=no,resizable=no");
    mapSrc.document.write('<html><head>');
    mapSrc.document.write(content.innerHTML);
    mapSrc.document.write('</div></body></html>');
    mapSrc.document.close();
    mapSrc.focus();
    mapSrc.print();
    setTimeout(function () { mapSrc.close(); }, 300);
    setTimeout(function () { window.location.href = RootUrl + "/Collections/Index"; }, 300);
    //window.location.href = RootUrl + "/Collections/Index";
}

function ComputeTotals() {
    var rowText;

    //Interest
    var interestpayment = 0;
    var totalRowCount = $("#interest-payment-table tr").length;

    for (var i = 1; i < totalRowCount; i++) {
        rowText = document.getElementById("interest-payment-table").rows[i].cells[0].innerText;

        if (rowText != "No data available in table") {
            var amountdue = document.getElementById("interest-payment-table").rows[i].cells[3].innerText;
            var amountdue_number = parseFloat(amountdue.replace(/[^0-9\.]+/g, ""));

            interestpayment = interestpayment + amountdue_number;
        }
    }
    $('#txt_interest_payment').val(interestpayment);


    //Principal
    var principalpayment = 0;
    var totalRowCount = $("#principal-payment-table tr").length;

    for (var i = 1; i < totalRowCount; i++) {
        rowText = document.getElementById("principal-payment-table").rows[i].cells[0].innerText;

        if (rowText != "No data available in table") {
            var amountdue = document.getElementById("principal-payment-table").rows[i].cells[3].innerText;
            var amountdue_number = parseFloat(amountdue.replace(/[^0-9\.]+/g, ""));

            principalpayment = principalpayment + amountdue_number;
        }
    }
    $('#txtprincipal_payment').val(principalpayment);

    $('#txttotal_payment').val(parseFloat(principalpayment) + parseFloat(interestpayment));
}