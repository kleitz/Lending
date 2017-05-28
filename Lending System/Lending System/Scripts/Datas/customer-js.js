$(document).ready(function () {
    if (RootUrl == "/") {
        RootUrl = ""
    }
    customerlisting.InitializeEvents();
});

var customerlisting =
    {
        InitializeEvents: function () {
            var table = $('#customer-table').DataTable({
                "ajax": {
                    "url": RootUrl + "/Customer/LoadCustomer",
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                        { "data": "autonum", "className": "dt-center" },
                        {
                            "data": "firstname", "className": "dt-right",
                            "render": function (data, type, row) {
                                return row.firstname + " " + row.lastname;
                            }
                        },
                        { "data": "address", "className": "dt-right" },
                        { "data": "credit_limit", "className": "dt-right", render: $.fn.dataTable.render.number(',', '.', 0, '₱') },
                        {
                            "render": function (data, type, row) {
                                return '<a href="' + RootUrl + '/Customer/Details?id=' + row.autonum + '"><span>Update</span></a>'
                            }
                        },
                ]
            });
        },
    }