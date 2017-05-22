$(document).ready(function () {
    if (RootUrl == "/") {
        RootUrl = ""
    }
    List.InitializeEvents();
});

var List =
    {
        InitializeEvents: function () {
            var table = $('#loans-table').DataTable({
                "ajax": {
                    "url": RootUrl + "/LoanLedger/LoadList",
                    "type": "GET",
                    "datatype": "json"
                },
                "order": [[1, "desc"]],
                "columns": [
                        { "data": "autonum", "className": "hide" },
                        {
                            "data": "loan_date", "className": "dt-left",
                            "render": function (data, type, row) {
                                var pattern = /Date\(([^)]+)\)/;
                                var results = pattern.exec(row.loan_date);
                                var dt = new Date(parseFloat(results[1]));
                                return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                            }
                        },
                        { "data": "loan_no", "className": "dt-center" },
                        { "data": "customer_name", "className": "dt-left" }
                ]
                
            });
        },
    }