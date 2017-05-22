$(document).ready(function () {
    if (RootUrl == "/") {
        RootUrl = ""
    }
    feeslist.InitializeEvents();
});

var feeslist =
    {
        InitializeEvents: function () {
            var table = $('#fees-table').DataTable({
                "ajax": {
                    "url": RootUrl + "/Fees/LoadFees",
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                        { "data": "autonum", "className": "dt-center" },
                        { "data": "fees_description", "className": "dt-right" },
                        {
                            "render": function (data, type, row) {
                                return '<a href="' + RootUrl + '/Fees/UpdateFee?id=' + row.autonum + '"><span class="fa fa-pencil" style="font-size: 18px" title="Edit"></span></a>'
                                       + ' <a href="' + RootUrl + '/Fees/Details?id=' + row.autonum + '"><span class="fa fa-search" style="font-size: 18px" title="Details"></span></a>'
                                //+ ' <a href="' + RootUrl + '/Customer/Delete?id=' + row.autonum + '"><span class="fa fa-trash-o" style="font-size: 18px" title="Delete"></span></a>'

                            }
                        },
                ]
            });
        },
    }