$(document).ready(function () {
    ciListing.InitializeEvents();
    //ciListing.AccountEntryTable();

});
var ciListing =
    {
        InitializeEvents: function () {
            var table = $('#ci-table').DataTable({
                "ajax": {
                    "url": RootUrl + "ci/loaddata",
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                        { "data": "ul_code", "className": "dt-center" },
                        {
                            "data": "doc_type", "className": "dt-right",
                            "render": function (data, type, row) {
                                return row.doc_type + "#" + row.doc_no;
                            }
                        },
                        {
                            "data": "date_trans", "className": "dt-left",
                            "render": function (data, type, row) {
                                var pattern = /Date\(([^)]+)\)/;
                                var results = pattern.exec(row.date_trans);
                                var dt = new Date(parseFloat(results[1]));
                                return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                            }
                        },
                        { "data": "id_code", "className": "dt-right" },
                        { "data": "customer", "autoWidth": true },
                        { "data": "terms_payment", "className": "dt-center" },
                        {
                            "data": "due_date", "className": "dt-left",
                            "render": function (data, type, row) {
                                var pattern = /Date\(([^)]+)\)/;
                                var results = pattern.exec(row.due_date);
                                var dt = new Date(parseFloat(results[1]));
                                return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                            }
                        },
                        { "data": "total_amount", "className": "dt-right", render: $.fn.dataTable.render.number(',', '.', 0, '₱') },
                        {
                            "render": function (data, type, row) {
                                return '<a href="' + RootUrl + 'CI/Edit?id=' + row.autonum + '"><span class="fa fa-pencil" style="font-size: 18px" title="Edit"></span></a>'
                                       + ' <a href="' + RootUrl + 'CI/Details?id=' + row.autonum + '"><span class="fa fa-search" style="font-size: 18px" title="Details"></span></a>'
                                       + ' <a href="' + RootUrl + 'CI/Delete?id=' + row.autonum + '"><span class="fa fa-trash-o" style="font-size: 18px" title="Delete"></span></a>'

                            }
                        },
                ]
            });
        },

        //AccountEntryTable: function () {
        //    var editor; // use a global for the submit and return data rendering in the examples

        //    var editor; // use a global for the submit and return data rendering in the examples

        //    $(document).ready(function () {
        //        editor = new $.fn.dataTable.Editor({
        //            ajax: "/accountingentry/loaddata",
        //            table: "#example",
        //            fields: [{
        //                label: "Account Code:",
        //                name: "primary_code"
        //            }, {
        //                label: "Account Title:",
        //                name: "acct_title"
        //            }, {
        //                label: "Particulars:",
        //                name: "particulars"
        //            }, {
        //                label: "Debit Amount:",
        //                name: "debit_amount"
        //            }, {
        //                label: "Credit Amount:",
        //                name: "credit_amount"
        //            }
        //            ],
        //            formOptions: {
        //                bubble: {
        //                    title: 'Edit',
        //                    buttons: false
        //                }
        //            }
        //        });

        //        $('button.new').on('click', function () {
        //            editor
        //                .title('Create new row')
        //                .buttons({ "label": "Add", "fn": function () { editor.submit() } })
        //                .create();
        //        });

        //        $('#example').on('click', 'tbody td', function (e) {
        //            if ($(this).index() < 6) {
        //                editor.bubble(this);
        //            }
        //        });

        //        $('#example').on('click', 'a.remove', function (e) {
        //            editor
        //                .title('Delete row')
        //                .message('Are you sure you wish to delete this row?')
        //                .buttons({ "label": "Delete", "fn": function () { editor.submit() } })
        //                .remove($(this).closest('tr'));
        //        });

        //        $('#example').DataTable({
        //            ajax: "/AccountingEntry/loaddata",
        //            columns: [
        //                { data: "primary_code" },
        //                { data: "acct_code" },
        //                { data: "particulars" },
        //                { data: "debit_amount", render: $.fn.dataTable.render.number(',', '.', 0, '$') },
        //                { data: "credit_amount", render: $.fn.dataTable.render.number(',', '.', 0, '$') },
        //                {
        //                    data: null,
        //                    defaultContent: '<a href="#" class="remove">Delete</a>',
        //                    orderable: false
        //                },
        //            ]
        //        });
        //    });
        //}
    }

