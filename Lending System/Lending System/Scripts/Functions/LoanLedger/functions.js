$(document).ready(function () {
    LoadLedger(id);
    var id = [];
    var table = $('#loans-table').DataTable();
    $('#loans-table tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
        id = [];
        id.push($(this).find('td').eq(2).text());
        LoadLedger(id);
    });
});

function LoadLedger(id) {
    $("#ledger-table").dataTable().fnDestroy()
    var table = $('#ledger-table').DataTable({
        "bLengthChange": false,
        "bFilter": false,
        "bInfo": false,
        "ajax": {
            "url": RootUrl + "/LoanLedger/LoadLedger?id=" + id,
            "type": "GET",
            "datatype": "json"
        },
        "language": {
            "decimal": ",",
            "thousands": "."
        },
        "columns": [
                { "data": "autonum", "className": "hide" },
                {
                    "data": "date_trans", "className": "dt-left",
                    "render": function (data, type, row) {
                        var pattern = /Date\(([^)]+)\)/;
                        var results = pattern.exec(row.date_trans);
                        var dt = new Date(parseFloat(results[1]));
                        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                    }
                },
                { "data": "reference_no", "className": "dt-left"},
                { "data": "amount_paid", "className": "dt-right"},
                { "data": "principal", "className": "dt-right" },
                { "data": "interest", "className": "dt-right" },
                { "data": "balance", "className": "dt-right" },
        ]
    });
}