//Computation of total receivables
function ComputeTotalReceivables(status) {
    var loanGranted = parseFloat($('#txtloan_granted').val().replace(/[^0-9\.]+/g, ""));
    var loanInterestRate = parseFloat($('#txtloan_interest_rate').val().replace(/[^0-9\.]+/g, ""));

    totalinterest = (loanGranted * (loanInterestRate / 100));

    if (parseInt($('#txtinstallment_no').val()) != "") {
        totalinterest = totalinterest * parseFloat($('#txtinstallment_no').val().replace(/[^0-9\.]+/g, ""));
    }

    $('#txttotal_receivable').val(0);
    $('#txtnet_proceeds').val(0);
    if (status == 1) {
        $('#txttotal_receivable').val(parseInt($('#txtloan_granted').val()) + totalinterest);
        $('#txtnet_proceeds').val(parseInt($('#txtloan_granted').val()));
    }
    else {
        $('#txttotal_receivable').val($('#txtloan_granted').val())
        $('#txtnet_proceeds').val(parseInt($('#txtloan_granted').val()) - totalinterest);
    }
}
//Create Amortization Schedule
function CreateSchedule() {
    if (interesttype == 1) {
        if ($('#txtloan_granted').val() !== 0) {
            CreateAmortizationSchedule(
                $('#txtdue_date').val(),
                $('#txtloan_granted').val(),
                totalinterest,
                $('#txttotal_receivable').val(),
                $('#txttotal_receivable').val(),
                $('#txtinstallment_no').val(),
                $('#txtpayment_scheme').find('option:selected').text());
        }
    }
    else {
        if ($('#txtloan_granted').val() !== 0) {
            CreateAmortizationSchedule(
                $('#txtdue_date').val(),
                $('#txtloan_granted').val(),
                0,
                $('#txttotal_receivable').val(),
                $('#txttotal_receivable').val(),
                $('#txtinstallment_no').val(),
                $('#txtpayment_scheme').find('option:selected').text());
        }
    }
}
// Creating Amortization Schedule
function CreateAmortizationSchedule(due_date, loan_granted, interest, total_receivable, balance, installment, payment_scheme) {
    $('#Amortization-table tbody > tr').empty()
    var table = $('#Amortization-table').DataTable();
    var line_no = table.rows().clear()
     line_no = table.rows().eq(0).length + 1;

    
    if (line_no == 1) {
        table.row.add({
            "due_date": "",
            "loan_granted": "",
            "interest": "",
            "net_proceeds": "",
            "balance": total_receivable,
        }).draw(true);
    }

    var tbl_principal_balance = total_receivable;
    var tbl_principal = (tbl_principal_balance - interest) / installment;
    var tbl_due_date = new Date(due_date);
    var tbldd = tbl_due_date.getDate();
    var tblmm = tbl_due_date.getMonth() + 1; //January is 0!
    var tblyyyy = tbl_due_date.getFullYear();
    var tbl_due_date_display = tblmm + "/" + tbldd + "/" + tblyyyy

    var interest = interest / installment;;
    var amount = tbl_principal + interest;

    for (i = 0; i < installment; i++) {
        line_no = table.rows().eq(0).length + 1;

        tbl_principal_balance = tbl_principal_balance - amount
        if (line_no >= 2) {
            table.row.add({
                "due_date": tbl_due_date_display,
                "loan_granted": tbl_principal,
                "interest": interest,
                "net_proceeds": amount,
                "balance": tbl_principal_balance ,
            }).draw(true);
        }
       
        if (payment_scheme == "Daily") {
            tbl_due_date.setDate(tbl_due_date.getDate() + 1)
            tbldd = tbl_due_date.getDate()
            tblmm = tbl_due_date.getMonth() + 1
            tblyyyy = tbl_due_date.getFullYear()
            tbl_due_date_display = tblmm + "/" + tbldd + "/" + tblyyyy
        }
        else if (payment_scheme == "Weekly") {
            tbl_due_date.setDate(tbl_due_date.getDate() + 7)
            tbldd = tbl_due_date.getDate()
            tblmm = tbl_due_date.getMonth() + 1
            tblyyyy = tbl_due_date.getFullYear()
            tbl_due_date_display = tblmm + "/" + tbldd + "/" + tblyyyy
        }
        else if (payment_scheme == "Semi-Monthly") {
            tbl_due_date.setDate(tbl_due_date.getDate() + 15)
            tbldd = tbl_due_date.getDate()
            tblmm = tbl_due_date.getMonth() + 1
            tblyyyy = tbl_due_date.getFullYear()
            tbl_due_date_display = tblmm + "/" + tbldd + "/" + tblyyyy
        }
        else if (payment_scheme == "Monthly") {
            tbl_due_date.setDate(tbl_due_date.getDate() + 30)
            tbldd = tbl_due_date.getDate()
            tblmm = tbl_due_date.getMonth() + 1
            tblyyyy = tbl_due_date.getFullYear()
            tbl_due_date_display = tblmm + "/" + tbldd + "/" + tblyyyy
        }
    }
};
//Request for Loan No.
var interval = 1000;
var GetLoanNo =
    {
        InitializeEvents: function () {
            $.ajax({
                url: RootUrl + "LoanProcessing/getLoanNo",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) { $('#txtloan_no').val(response) },
                complete: function (response) {
                    // Schedule the next
                    setTimeout(GetLoanNo.InitializeEvents, interval)
                },
                error: ""
            });
        }
    }
//Initialize Amortization Table
var AmortizationTable =
        {
            InitializeEvents: function () {
                var table = $('#Amortization-table').DataTable({
                    "bPaginate": false,
                    "bLengthChange": false,
                    "bFilter": false,
                    "bInfo": false,
                    "columns": [
                            { "data": "due_date", "className": "dt-left" },
                            { "data": "loan_granted", "autoWidth": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                            { "data": "interest", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                            { "data": "net_proceeds", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') },
                            { "data": "balance", "className": "text-right", render: $.fn.dataTable.render.number(',', '.', 2, '') }
                    ]
                });
            }
        }
//Initialize Charges Table
function LoadCharges(chargeid) {
    if (chargetype === "1") {
        var table = $('#charges-table').DataTable({
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": false,
            "bInfo": false,

            "ajax": {
                "url": RootUrl + "LoanProcessing/LoadLoanTypeCharges?id=" + chargeid,
                "type": "GET",
                "datatype": "json"
            }
        });
    }
    else {
        var table = $('#charges-table').DataTable({
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": false,
            "bInfo": false,

            "ajax": {
                "url": RootUrl + "LoanProcessing/LoadLoanTypeCharges?id=" + chargeid,
                "type": "GET",
                "datatype": "json"
            }
        });
    }
}
