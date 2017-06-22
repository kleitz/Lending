
$(document).ready(function () {
    AmortizationTable.InitializeEvents();
    CreateSchedule();
});

var interesttype = "";
var chargetype = "";
var totalinterest = "";
var loandays = "";
var loanstatus = "";

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

function CreateSchedule() {
    var id = $('#txtloantype_id').val()
    
    $.ajax({
        url: RootUrl + "LoanProcessing/LoadLoanTypeInterestType?id=" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response, data, status, xhr) {
            
            CreateSchedule1(response)
        },
        error: ""
    });
}
function CreateSchedule1(param) {
    var id = $('#txtloantype_id').val()
    var interesttype;
   
    interesttype = param;

    totalinterest = parseInt($('#txtloan_granted').val() * ($('#txtloan_interest_rate').val() / 100))

    if (parseInt($('#txtinstallment_no').val()) != "") {
        totalinterest = totalinterest * parseInt($('#txtinstallment_no').val())
    }
    
    if (interesttype == 1) {
        if ($('#txtloan_granted').val() != 0) {
            CreateAmortizationSchedule(
                                        $('#txtdue_date').val(),
                                        $('#txtloan_granted').val(),
                                        totalinterest,
                                        $('#txttotal_receivable').val(),
                                        $('#txttotal_receivable').val(),
                                        $('#txtinstallment_no').val(),
                                        $('#txtpayment_scheme').find('option:selected').text())
        }
    }
    else {
        if ($('#txtloan_granted').val() != 0) {
            CreateAmortizationSchedule(
                                        $('#txtdue_date').val(),
                                        $('#txtloan_granted').val(),
                                        0,
                                        $('#txttotal_receivable').val(),
                                        $('#txttotal_receivable').val(),
                                        $('#txtinstallment_no').val(),
                                        $('#txtpayment_scheme').find('option:selected').text())
        }
    }
}

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
                "balance": tbl_principal_balance,
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
var Review = function () {
    var id = $('#txtautonum').val();
    CheckStatusReview(id);

}
var Approve = function () {
    var id = $('#txtautonum').val();
    CheckStatusApprove(id);
}
var Release = function () {
    var id = $('#txtautonum').val();
    CheckStatusForRelease(id);
}
function CheckStatusReview(autonum) {
    $.ajax({
        url: RootUrl + "LoanProcessing/CheckLoanStatus?id=" + autonum,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            
            if (data != "Reviewed" && data != "Approved" && data != "Released") {
                AjaxReview(autonum)
            }
            else if (data == "Reviewed" && data != "Approved" && data != "Released")
            {
                toastr.error('Already reviewed.');
            }
            else if (data == "Approved" && data != "Released") {
                toastr.error('Already approved.');
            }
            else if (data == "Released") {
                toastr.error('Already released.');
            }
        },
        error: ""
    });
}
function CheckStatusApprove(autonum) {
    $.ajax({
        url: RootUrl + "LoanProcessing/CheckLoanStatus?id=" + autonum,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            
            if (data == "Reviewed" && data != "Approved" && data != "Released") {
                AjaxApprove(autonum)
            }
            else if (data != "Reviewed" && data != "Approved" && data != "Released") {
                toastr.error('Please review first.');
            }
            else if (data == "Approved" && data != "Released") {
                toastr.error('Already approved.');
            }
            else if (data == "Released") {
                toastr.error('Already released.');
            }
        },
        error: ""
    });
}
function CheckStatusForRelease(autonum) {
    $.ajax({
        url: RootUrl + "LoanProcessing/CheckLoanStatus?id=" + autonum,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data == "Released") {
                toastr.error('Already released.');
            }
            else {
                AjaxRelease(autonum)
            }
        },
        error: ""
    });
}
function AjaxReview(id) {
    var myObj = {
        "status": "Reviewed",
        "reviewed_by_name": $('#txtreviewed_by_name').find('option:selected').text()
    };

    $.ajax({
        url: RootUrl + "LoanProcessing/Review?id=" + id,
        type: "POST",
        data: JSON.stringify(myObj),
        contentType: 'application/json',
        success: function (response, status, xhr) {
            if (response == "Success") {
                UpdateDisplayStatus(id);
                toastr.success('Successfully reviewed.');
            }
            else {
                toastr.error('failed to review.');
            }
        },
        error: function (response, status, xhr) {
            toastr.error('failed to review.');
        }
    });
}
function AjaxApprove(id) {
    var myObj = {
        "status": "Approved",
        "reviewed_by_name": $('#txtapproved_by_name').find('option:selected').text()
    };

    $.ajax({
        url: RootUrl + "LoanProcessing/Approve?id=" + id,
        type: "POST",
        data: JSON.stringify(myObj),
        contentType: 'application/json',
        success: function (response, status, xhr) {
            if (response == "Success") {
                UpdateDisplayStatus(id);
                toastr.success('Successfully approved.');
            }
            else {
                toastr.error('failed.');
            }
        },
        error: function (response, status, xhr) {
            toastr.error('failed.');
        }
    });
}

function AjaxRelease(id) {
    var myObj = {
        "status": "Released"
    };

    $.ajax({
        url: RootUrl + "LoanProcessing/Release?id=" + id,
        type: "POST",
        data: JSON.stringify(myObj),
        contentType: 'application/json',
        success: function (response, status, xhr) {
            if (response == "Success") {
                UpdateDisplayStatus(id);
                toastr.success('Successfully released.');
            }
            else {
                toastr.error('failed.');
            }
        },
        error: function (response, status, xhr) {
            toastr.error('failed.');
        }
    });
    window.location.href = RootUrl + "LoanProcessing/Index"
}

function UpdateDisplayStatus(id) {
    $.ajax({
        url: RootUrl + "LoanProcessing/CheckLoanStatus?id=" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response, status, xhr) {      
            $('#StatusTitle').val(response);
            document.getElementById('StatusTitle').innerHTML = response;
        }
    });
}
