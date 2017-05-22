$(document).ready(function () {
    if (RootUrl == "/") {
        RootUrl = ""
    }
    loanslist.InitializeEvents();
});

var loanslist =
    {
        InitializeEvents: function () {
            var table = $('#loantype-table').DataTable({
                "ajax": {
                    "url": RootUrl + "/Loans/LoadLoans",
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                        { "data": "autonum", "className": "dt-center" },
                        { "data": "description", "className": "dt-right" },
                        {
                            "render": function (data, type, row) {
                                return '<a href="' + RootUrl + '/Loans/UpdateLoans?id=' + row.autonum + '"><span class="fa fa-pencil" style="font-size: 18px" title="Edit"></span></a>'
                                       + ' <a href="' + RootUrl + '/Loans/Details?id=' + row.autonum + '"><span class="fa fa-search" style="font-size: 18px" title="Details"></span></a>'
                                //+ ' <a href="' + RootUrl + '/Customer/Delete?id=' + row.autonum + '"><span class="fa fa-trash-o" style="font-size: 18px" title="Delete"></span></a>'
                            }
                        },
                ]
            });
        },
    }