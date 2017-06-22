
var LoanPrincipalDue =
        {
            InitializeEvents: function () {
                
                var table = $('#principal-payment-table').DataTable({
                    "bPaginate": false,
                    "bFilter": false,
                    "bInfo": false,
                    "ajax": {
                        "url": RootUrl + "/Collections/LoadPrincipalDues?id=" + $('#txtpayor').val(),
                        "type": "GET",
                        "datatype": "json",
                    },
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "columns": [
                            { "data": "loan_no", "className": "dt-left" },
                            { "data": "loan_type", "className": "dt-left" },
                            {
                                "data": "due_date", "className": "dt-left",
                                "render": function (data, type, row) {
                                    var pattern = /Date\(([^)]+)\)/;
                                    var results = pattern.exec(row.due_date);
                                    var dt = new Date(parseFloat(results[1]));
                                    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                                }
                            },
                            { "data": "amount_due", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                            { "data": "payment", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                            { "data": "interest_type", "className": "hide" },
                            { "data": "interest", "className": "hide" }
                    ],
                });
            }
        }
var LoanInterestDue =
        {
            InitializeEvents: function () {

                var table = $('#interest-payment-table').DataTable({
                    "bPaginate": false,
                    "bFilter": false,
                    "bInfo": false,
                    "ajax": {
                        "url": RootUrl + "/Collections/LoadInterestDues?id=" + $('#txtpayor').val(),
                        "type": "GET",
                        "datatype": "json",
                    },
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "columns": [
                            { "data": "loan_no", "className": "dt-left" },
                            { "data": "loan_type", "className": "dt-left" },
                            {
                                "data": "due_date", "className": "dt-left",
                                "render": function (data, type, row) {
                                    var pattern = /Date\(([^)]+)\)/;
                                    var results = pattern.exec(row.due_date);
                                    var dt = new Date(parseFloat(results[1]));
                                    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                                }
                            },
                            { "data": "amount_due", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                            { "data": "payment", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                            { "data": "interest_type", "className": "hide" },
                            { "data": "interest", "className": "hide" }
                    ]
                });
            }
        }
function Balance() {
    var balance;
    var balance1;
    var balance2;

    balance1 = DuePrincipal();
    balance2 = DueInterest();

    balance = Number(balance1) + Number(balance2);

    $("#txtamount_due_modal").val(balance.toFixed(2));
}

function DuePrincipal() {
    var totalRowCount = $("#principal-payment-table tr").length;
    var balance =0;

    for (var i = 1; i < totalRowCount; i++) {
        var rowText = document.getElementById("principal-payment-table").rows[i].cells[0].innerText;

        if (rowText == "No data available in table") {
            return false;
        }

        var amountdue = document.getElementById("principal-payment-table").rows[i].cells[3].innerText;
        var amountdue_number = parseFloat(amountdue.replace(/[^0-9\.]+/g, ""));

        balance = balance + amountdue_number;
    }
    return balance;
}
function DueInterest() {
    var totalRowCount = $("#interest-payment-table tr").length;
    var balance = 0;
    
    for (var i = 1; i < totalRowCount; i++) {
        var rowText = document.getElementById("interest-payment-table").rows[i].cells[0].innerText;

        if (rowText == "No data available in table") {
            return false;
        }

        var amountdue = document.getElementById("interest-payment-table").rows[i].cells[3].innerText;
        var amountdue_number = parseFloat(amountdue.replace(/[^0-9\.]+/g, ""));

        balance = balance + amountdue_number;
    }
    return balance;
}
// FOR VIEWING
var LoanPrincipalDueViewing =
        {
            InitializeEvents: function () {
                var deferred = $.Deferred();
                var table = $('#principal-payment-table').DataTable({
                    "bPaginate": false,
                    "bFilter": false,
                    "bInfo": false,
                    "ajax": {
                        "url": RootUrl + "/Collections/ViewPrincipalDues?id=" + $('#txtreference_no').val(),
                        "type": "GET",
                        "datatype": "json",
                    },
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "columns": [
                            { "data": "loan_no", "className": "dt-left" },
                            { "data": "loan_name", "className": "dt-left" },
                            {
                                "data": "due_date", "className": "dt-left",
                                "render": function (data, type, row) {
                                    var pattern = /Date\(([^)]+)\)/;
                                    var results = pattern.exec(row.due_date);
                                    var dt = new Date(parseFloat(results[1]));
                                    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                                }
                            },
                            { "data": "amount", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') }
                    ],
                    "fnInitComplete": function (oSettings, json) {
                        $.when(LoanInterestDueViewing.InitializeEvents());
                    }
                });

                return deferred.promise();
            }
        }
var LoanInterestDueViewing =
        {
            InitializeEvents: function () {
                var deferred = $.Deferred();
                var table = $('#interest-payment-table').DataTable({
                    "bPaginate": false,
                    "bFilter": false,
                    "bInfo": false,
                    "ajax": {
                        "url": RootUrl + "/Collections/ViewInterestDues?id=" + $('#txtreference_no').val(),
                        "type": "GET",
                        "datatype": "json",
                    },
                    "language": {
                        "decimal": ",",
                        "thousands": "."
                    },
                    "columns": [
                            { "data": "loan_no", "className": "dt-left" },
                            { "data": "loan_name", "className": "dt-left" },
                            {
                                "data": "due_date", "className": "dt-left",
                                "render": function (data, type, row) {
                                    var pattern = /Date\(([^)]+)\)/;
                                    var results = pattern.exec(row.due_date);
                                    var dt = new Date(parseFloat(results[1]));
                                    return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                                }
                            },
                            { "data": "amount", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') }
                    ],
                    "fnInitComplete": function (oSettings, json) {
                        ComputeTotals();
                        deferred.resolve();
                    }
                });

                return deferred.promise();
            }
        }

function LoadReceipt(arg) {
    var url = RootUrl + "/Collections/Print?id=" + arg;
    setTimeout(function() {
            $('#receipt').load(url,
                function () {
                    $('#ModalPrint').modal('show');
                });
        },
        300);
}
function LoadReceiptRePrint() {

    var url = RootUrl + "/Collections/Print?id=" + $('#txtreference_no').val();
    var encodedParam = encodeURIComponent(url);

    $('#receipt').load(url,
        function () {
            $('#ModalPrint').modal('show');
        });
}