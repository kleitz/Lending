function LoadPrint() {
    window.location.href = RootUrl + "/Receivables/Print?date=" + $('#asOf').val();
}
function LoadDetails(parameters) {

    var url = RootUrl + "/Receivables/PrintDetails?date=" + parameters;
    var encodedParam = encodeURIComponent(url);

    $('#report-body').load(url, function () { });
}
function Print(divId) {

    setTimeout(function () {
        showPrintDialog(divId);
    }, 500);

}
function showPrintDialog(divId) {

    var printContents = document.getElementById(divId).innerHTML;
    var originalContents = document.body.innerHTML;

    document.body.innerHTML = printContents;

    window.print();
    setTimeout(function () { window.print(); }, 500);
    setTimeout(function () { window.close(); }, 300);
    setTimeout(function () { window.location.href = RootUrl + "/Receivables/Index"; }, 300);

    document.body.innerHTML = originalContents;

    //window.location.href = RootUrl + "/Collections/Index";
}
