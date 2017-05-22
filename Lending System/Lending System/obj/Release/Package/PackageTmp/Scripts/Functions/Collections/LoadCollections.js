$(document).ready(function () {
    if (RootUrl == "/") {
        RootUrl = ""
    }
    List.InitializeEvents();
});

var List =
    {
        InitializeEvents: function () {
            
            var table = $('#payment-table').DataTable({
                "ajax": {
                    "url": RootUrl + "/Collections/LoadList",
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
                        { "data": "reference_no", "className": "dt-left" },
                        { "data": "payor_name", "className": "dt-left" },
                        { "data": "total_amount", "className": "dt-right", render: $.fn.dataTable.render.number(',', '.', 0, '₱') },
                ],
              
            });
        },
    }