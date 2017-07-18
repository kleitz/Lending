app.vm = (function() {
    //"use strict";
    var forSaveingModel = new app.createCollectionModel();

    // #region CONTROLS                
    var isPaymentListShowed = ko.observable(true);
    var isCreateModeShow = ko.observable(false);

    var interestAllItems = ko.observableArray([]);
    var interestItems = ko.pureComputed(function () {
        return interestAllItems();
    });
    var principalAllItems = ko.observableArray([]);
    var principalItems = ko.pureComputed(function () {
        return principalAllItems();
    });

    // #endregion

    // #region BEHAVIORS
    // initializers
    function activate() {
        loadList();
        //hideSidebar();
    }

    function addPayment() {
        isPaymentListShowed(false);
        isCreateModeShow(true);

        getServerDate();
        //setTimeout(function () { $('#customerId').focus() }, 800);
        clearControls();
    }

    function savePayment() {
        //setTimeout(function () {
        //    printReceipt('receipt');
        //}, 300);

        loaderApp.showPleaseWait();
        var param = ko.toJS(forSaveingModel);
        var url = RootUrl + "Administrator/Collection/Save";
        $.ajax({
            type: 'POST',
            url: url,
            data: ko.utils.stringifyJson(param),
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (result.success) {
                    //swal("Success", result.message, "success",
                  
                    //        function (isConfirm) {
                    //            if (isConfirm) {
                    //                reloadPage();
                    //            }
                    //        }                    
                    //    );    

                    swal({ title: "Success!", text: result.message, type: "success" }, function () { printReceipt('receipt'); });

                    loaderApp.hidePleaseWait();
                } else {
                    loaderApp.hidePleaseWait();

                    swal("Error", result.message, "error");
                }
            }
        });
    }

    function backToPaymentList() {
        isPaymentListShowed(true);
        isCreateModeShow(false);
        clearControls();
    }

    function loadList() {
        $("#payment-table").dataTable().fnDestroy();
        $('#payment-table').DataTable({
            "ajax": {
                "url": RootUrl + "Administrator/Collection/LoadList",
                "type": "GET",
                "datatype": "json"
            },
            "order": [[2, "desc"]],
            "columns": [
                { "data": "autonum", "className": "hide" },
                {
                    "data": "date_trans",
                    "className": "text-left",
                    "render": function(data, type, row) {
                        var pattern = /Date\(([^)]+)\)/;
                        var results = pattern.exec(row.date_trans);
                        var dt = new Date(parseFloat(results[1]));
                        return (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
                    }
                },
                { "data": "reference_no", "className": "text-left" },
                { "data": "payor_name", "className": "text-left" },
                {
                    "data": "total_amount",
                    "className": "text-right",
                    render: $.fn.dataTable.render.number(',', '.', 2, '')
                },
                {
                    "render": function(data, type, row) {
                        return '<ul class="icons-list">' +
                            '<li class="dropdown">' +
                            '<a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="icon-menu7"></i></a>' +
                            '<ul class="dropdown-menu dropdown-menu-right">' +
                            '<li><a href="#"><i class="icon-file-stats"></i> View receipt</a></li>' +
                            '</ul>' +
                            '</li>' +
                            '</ul>';
                    }
                }
            ]
        });
    }

    function getServerDate() {
        $.getJSON(RootUrl + "Administrator/Collection/GetServerDate",
            function(result) {
                $('#PaymentDate').val(result);
            });
    };

    function generateInterest(arg) {
        $.getJSON(RootUrl + "Administrator/Collection/GenerateInterest?loanNo=" + arg,
            function (result) {});
    };

    function hideSidebar() {
        $('#hide-sidebar').trigger('click');
    };

    function generatePrintValues() {
        //$('#ReceiptNo').html('Whatever <b>HTML</b> you want here.');

        $('#ReceiptNo').html('Receipt No: ' + $('#PaymentNo').val());
        $('#Date').html('Date: ' + $('#PaymentDate').val());
        $('#Borrower').html('Borrower: ' + $('#CustomerName').val());
        $('#IdNo').html('ID No: ' + $('#customerId').val());

        var paymentTotal = $('#Payment').val();
        paymentTotal = parseFloat(paymentTotal.toString().replace(/[^0-9\.]+/g, ""));

        var amountDueInterest = forSaveingModel.AmountDueInterest;
        amountDueInterest = parseFloat(amountDueInterest.toString().replace(/[^0-9\.]+/g, ""));

        var amountDuePrincipal = forSaveingModel.AmountDuePrincipal;
        amountDuePrincipal = parseFloat(amountDuePrincipal.toString().replace(/[^0-9\.]+/g, ""));

        if (amountDueInterest > 0) {
            if (paymentTotal >= amountDueInterest) {
                $('#interestReference').html(forSaveingModel.LoanNo);
                $('#interestParticulars').html('Interest payment');
                $('#interestAmount').html(amountDueInterest.toFixed(2));
                paymentTotal = paymentTotal - amountDueInterest;
            } else {
                $('#interestReference').html(forSaveingModel.LoanNo);
                $('#interestParticulars').html('Interest payment');
                $('#interestAmount').html(paymentTotal.toFixed(2));
                paymentTotal = 0;
            }
        }
        if (paymentTotal > 0) {
            if (amountDuePrincipal > 0) {
                $('#principalReference').html(forSaveingModel.LoanNo);
                $('#principalParticulars').html('Principal payment');
                $('#principalAmount').html(paymentTotal.toFixed(2));
            }
        }

        $('#balancLoanNo').html(forSaveingModel.LoanNo);
        $('#balanceAmount').html($('#Change').val());
    };

    function printReceipt(divId) {
        setTimeout(function () {
            showPrintDialog(divId);
        }, 500);

    }
    function showPrintDialog(divId) {
        var content = document.getElementById(divId);
        var mapSrc = window.open("", "PRINT MAP", "width=200,top=0,left=0,toolbar=no,scrollbars=no,status=no,resizable=no");
        mapSrc.document.write('<html><head>');
        mapSrc.document.write(content.innerHTML);
        mapSrc.document.write('</div></body></html>');
        mapSrc.document.close();
        mapSrc.focus();
        mapSrc.print();
        setTimeout(function () { mapSrc.close(); }, 300);
        setTimeout(function () { reloadPage(); }, 300);
    }
    // #endregion

    //FROM STEPY
    function getPaymentNo() {
        $.getJSON(RootUrl + "Administrator/Collection/GetPaymentNo", function (result) {
            $('#PaymentNo').val(result);
            app.vm.forSaveingModel.PaymentNo = result;
        });
    }
    function loadAccountList(arg) {

        $("#account-table").dataTable().fnDestroy();
        $('#account-table').dataTable({
            ajax: {
                url: RootUrl + "Administrator/Collection/LoadAccountList?id=" + arg
            },
            columns: [
                { data: "LoanNo", "className": "text-left" },
                { data: "CustomerName", "className": "text-left" },
                { data: "Balance", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                {
                    "render": function (data, type, row) {
                        return '<input type="radio" class="styled" name="radAnswer">';
                    }
                },
                { data: "Balance", "className": "hide", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                { data: "Balance", "className": "hide", render: $.fn.dataTable.render.number(',', '.', 2, '') }
            ]
        });
    }
    function loadAccountDues() {
        var validate = false;
        var selectedRow;
        var oTable = $('#account-table').dataTable();
        $("input:checked", oTable.fnGetNodes()).each(function () {
            var tdColumn = $(this).closest("td");
            var trRow = $(this).closest("tr");
            var rowIndex = trRow.index();
            var columnIndex = tdColumn.index();
            selectedRow = rowIndex + 1;
        });

        var totalRowCount = $("#account-table tr").length;
        for (var i = 1; i < totalRowCount; i++) {
            if (i === selectedRow) {
                var customerName = document.getElementById("account-table").rows[i].cells[1].innerText;
                $('#CustomerName').val(customerName);

                var loanNo = document.getElementById("account-table").rows[i].cells[0].innerText;

                generateInterest(loanNo);
                loadPrincipal(loanNo);
                loadInterest(loanNo);

                forSaveingModel.LoanNo = loanNo;

                validate = true;
            }
        }
        return validate;
    }

    function loadAmountDue() {
        var totalAmountDue = 0.00;
        var totalPrincipalRowCount = $("#principalTable tr").length;
        var totalInterestRowCount = $("#interestTable tr").length;

        forSaveingModel.AmountDuePrincipal = 0;
        forSaveingModel.AmountDueInterest = 0;

        for (var i = 1; i < totalPrincipalRowCount; i++) {
            var amountPrincipalDue = document.getElementById("principalTable").rows[i].cells[3].innerText;
            totalAmountDue = parseFloat(amountPrincipalDue.replace(/[^0-9\.]+/g, ""));

            var loanName = document.getElementById("principalTable").rows[i].cells[1].innerText;
            var loanDueDate = document.getElementById("principalTable").rows[i].cells[2].innerText;
            forSaveingModel.LoanName = loanName;
            forSaveingModel.LoanDueDate = loanDueDate;

            forSaveingModel.AmountDuePrincipal = parseFloat(amountPrincipalDue.replace(/[^0-9\.]+/g, "")) + 0;
        }
        for (var c = 1; c < totalInterestRowCount; c++) {
            var amountInterestDue = document.getElementById("interestTable").rows[c].cells[3].innerText;
            totalAmountDue = totalAmountDue + parseFloat(amountInterestDue.replace(/[^0-9\.]+/g, ""));

            forSaveingModel.AmountDueInterest = parseFloat(amountInterestDue.replace(/[^0-9\.]+/g, "")) + 0;
        }

        $('#AmountDue').val(totalAmountDue.toFixed(2));
       
        setTimeout(function () { $('#Payment').focus() }, 800);
    }
    
    function loadPrincipal(arg) {
        var getUrl = RootUrl + "Administrator/Collection/GetPrincipal?loanNo=" + arg;
        $.get(getUrl,
            function (result) {
                principalAllItems.removeAll();

                var data = result.data;
                var temp = principalAllItems();
                data.forEach(function (o) {
                    var loanAccountModelPrincipal = new app.loanAccountModelPrincipal(
                        o.LoanNo,
                        o.LoanType,
                        o.DueDate,
                        o.AmountDue);
                    temp.push(loanAccountModelPrincipal);
                });
                principalAllItems.valueHasMutated();

                return principalAllItems();
            });
    };
    function loadInterest(arg) {
        var getUrl = RootUrl + "Administrator/Collection/GetInterest?loanNo=" + arg;
        $.get(getUrl,
            function (result) {
                interestAllItems.removeAll();

                var data = result.data;
                var temp = interestAllItems();
                data.forEach(function (o) {
                    var loanAccountModelInterest = new app.loanAccountModelInterest(
                        o.LoanNo,
                        o.LoanType,
                        o.DueDate,
                        o.AmountDue);
                    temp.push(loanAccountModelInterest);
                });
                interestAllItems.valueHasMutated();

                return interestAllItems();
            });
    };

    function clearControls() {
        document.getElementById("stepyProject").reset();
        //document.getElementById('customerId').selectedIndex = 0;

        //loadAccountList();

        //principalAllItems.removeAll();
        //interestAllItems.removeAll();

        //$('#PaymentNo').val("");
        //$('#PaymentDate').val("");
        //$('#CustomerName').val("");

        //$('#AmountDue').val("");
        //$('#Payment').val("");
        //$('#Change').val("");
    }
    function reloadPage() {
        window.location.reload();
    }

    var vm = {
        interestItems: interestItems,
        principalItems: principalItems,
        activate: activate,
        isPaymentListShowed: isPaymentListShowed,
        isCreateModeShow: isCreateModeShow,
        addPayment: addPayment,
        savePayment: savePayment,
        backToPaymentList: backToPaymentList,
        loadAccountList,
        loadAccountDues: loadAccountDues,
        getPaymentNo: getPaymentNo,
        loadAmountDue: loadAmountDue,
        forSaveingModel: forSaveingModel,
        clearControls: clearControls,
        generatePrintValues: generatePrintValues,
        printReceipt: printReceipt
    };
    return vm;

})();


$(function () {
    "use strict";

    app.vm.activate();

    // Success
    $(".control-success").uniform({
        radioClass: 'choice',
        wrapperClass: 'border-success-600 text-success-800'
    });

    //Switchery
    var switches = Array.prototype.slice.call(document.querySelectorAll('.switchery'));
    switches.forEach(function (html) {
        var switchery = new Switchery(html, { color: '#4CAF50' });
    });

    $('#Payment').keyup(function () {
        var amountDue = $('#AmountDue').val();
        amountDue = parseFloat(amountDue.replace(/[^0-9\.]+/g, ""));

        var payment = $('#Payment').val();
        payment = parseFloat(payment.replace(/[^0-9\.]+/g, ""));

        if (payment > amountDue) {
            $('#Payment').val(amountDue);

            $('#Change').val(0.00);
        } else {
            var change = amountDue - payment;
            $('#Change').val(change.toFixed(2));
        }


    });

    ko.applyBindings(app.vm);
});
