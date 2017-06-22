$(document).ready(function () {
    if (RootUrl == "/") {
        RootUrl = ""
    }
    List.InitializeEvents();

    $('input.number').number(true, 2);
    $('span.number').number(true, 4);
});

var List =
    {
        InitializeEvents: function () {

            var table = $('#cashout-table').DataTable({
                "ajax": {
                    "url": RootUrl + "/CashPullOut/LoadList",
                    "type": "GET",
                    "datatype": "json"
                },
                "order": [[1, "desc"]],
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
                        { "data": "username", "className": "dt-left" },
                        { "data": "amount", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') }
                ],

            });
        },
    }