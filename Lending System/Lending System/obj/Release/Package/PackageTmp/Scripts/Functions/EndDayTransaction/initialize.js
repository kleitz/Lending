﻿$(document).ready(function () {
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
            $("#end-day-table").dataTable().fnDestroy()
            var table = $('#end-day-table').DataTable({
                "ajax": {
                    "url": RootUrl + "/EndDayTransaction/LoadList",
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
                        { "data": "cash_begin", "className": "dt-right", render: $.fn.dataTable.render.number(',', '.', 0, '') },
                        { "data": "cash_release", "className": "dt-right", render: $.fn.dataTable.render.number(',', '.', 0, '') },
                        { "data": "cash_collected", "className": "dt-right", render: $.fn.dataTable.render.number(',', '.', 0, '') },
                        { "data": "cash_pulled_out", "className": "dt-right", render: $.fn.dataTable.render.number(',', '.', 0, '') },
                        { "data": "cash_end", "className": "dt-right", render: $.fn.dataTable.render.number(',', '.', 0, '') },
                        {
                            "render": function (data, type, row) {
                                return '<a href="' + RootUrl + '/EndDayTransaction/Print?id=' + row.autonum + '"><span title="Details">View</span></a>'
                            }
                        }
                ]
            });
        }
    }