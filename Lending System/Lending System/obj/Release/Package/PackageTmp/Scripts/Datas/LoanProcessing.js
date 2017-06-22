$(document).ready(function () {
    if (RootUrl == "/") {
        RootUrl = ""
    }
    List.InitializeEvents();
});

var List =
    {
        InitializeEvents: function () {
            var table = $('#LoanProcessing-table').DataTable({
                "ajax": {
                    "url": RootUrl + "/LoanProcessing/LoadList",
                    "type": "GET",
                    "datatype": "json"
                },
                "order": [[6, "asc"]],
                "columns": [
                        { "data": "autonum", "className": "hide"},
                        { "data": "loan_no", "className": "dt-center" },
                        { "data": "customer_name", "className": "dt-left" },
                        { "data": "loan_name", "className": "dt-left" },
                        { "data": "loan_granted", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                        {
                            "data": "loan_date", "className": "dt-left",
                            "render": function (data, type, row) {
                                var pattern = /Date\(([^)]+)\)/;
                                var results = pattern.exec(row.loan_date);
                                var dt = new Date(parseFloat(results[1]));
                                return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                            }
                        },
                        { "data": "status", "className": "dt-left" },
                        {
                            "render": function (data, type, row) {
                                return '<a href="' + RootUrl + '/LoanProcessing/Details?id=' + row.autonum + '"><span class="fa fa-search" style="font-size: 18px" title="Details"></span></a>'
                            }
                        },
                ]
            });
        },
    }