﻿
$(document).ready(function () {
    if (RootUrl == "/") {
        RootUrl = ""
    }
    List.InitializeEvents();
    $('input.number').number(true, 2);
    $('span.number').number(true, 4);

    setInterval(function () {
        table.ajax.reload();
    }, 10000);
    setInterval(GetCashReleased, 1000);
});
var table;
var List =
    {
        InitializeEvents: function () {          
            $("#loan-table").dataTable().fnDestroy()
            table = $('#loan-table').DataTable({
                "ajax": {
                    "url": RootUrl + "/Home/LoadList",
                    "type": "GET",
                    "datatype": "json",
                },
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
                        { "data": "customer_name", "className": "dt-left" },
                        { "data": "loan_granted", "className": "dt-right", render: $.fn.dataTable.render.number(',', '.', 0, '₱') }

                ]
            });
        }
    }