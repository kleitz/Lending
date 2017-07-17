
//Customer
$('#txtcustomer').on('change', function () {
    $('#txtcustomer_id').val($('#txtcustomer').find('option:selected').val())
    $('#txtcustomer_name').val($('#txtcustomer').find('option:selected').text())

    $('#txtdue_date').val(mm + "/" + dd + "/" + yyyy)
    $('#txtloan_date').val(mm + "/" + dd + "/" + yyyy)
})
//Loan Type
$('#txtloantype').on('change', function () {
    ChangeLoanType();
    ChangePaymentScheme(loandays);
})
//Payment Scheme
$('#txtpayment_scheme').on('change', function () {
    ChangePaymentScheme(loandays);
})
//Installment No
$('#txtinstallment_no').on('change', function () {
    ChangeDueDate();
    ComputeTotalReceivables(interesttype)
});
function ChangeLoanType() {
    $('#txtloantype_id').val($('#txtloantype').find('option:selected').val())
    $('#txtloan_name').val($('#txtloantype').find('option:selected').text())
    $('#txtinstallment_no').val("1")

    var id = $('#txtloantype').find('option:selected').val()

    $.ajax({
        url: RootUrl + "LoanProcessing/LoadLoanTypeInterestRate?id=" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) { $('#txtloan_interest_rate').val(response) },
        error: ""
    });

    $.ajax({
        url: RootUrl + "LoanProcessing/LoadLoanTypeDays?id=" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) { ChangePaymentScheme(response), loandays = response },
        error: ""
    });

    $.ajax({
        url: RootUrl + "LoanProcessing/LoadLoanTypeInterestType?id=" + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) { ComputeTotalReceivables(response), interesttype = response },
        error: ""
    });

    //$.ajax({
    //    url: RootUrl + "LoanProcessing/LoadLoanTypeChargeType?id=" + id,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (response) { chargetype = response, LoadCharges(response); },
    //    error: ""
    //});
}
function ChangeDueDate() {
    var condition = $('#txtpayment_scheme').find('option:selected').text()
    var dateholder = new Date();

    if (condition == "Daily") {
        dateholder.setDate(dateholder.getDate() + 1);
        dd = dateholder.getDate();
        mm = dateholder.getMonth() + 1;
        yyyy = dateholder.getFullYear();

        $('#txtdue_date').val(mm + "/" + dd + "/" + yyyy)
    }
    else if (condition == "Weekly") {
        dateholder.setDate(dateholder.getDate() + 7);
        dd = dateholder.getDate();
        mm = dateholder.getMonth() + 1;
        yyyy = dateholder.getFullYear();

        $('#txtdue_date').val(mm + "/" + dd + "/" + yyyy)
    }
    else if (condition == "Semi-Monthly") {
        dateholder.setDate(dateholder.getDate() + 15);
        dd = dateholder.getDate();
        mm = dateholder.getMonth() + 1;
        yyyy = dateholder.getFullYear();

        $('#txtdue_date').val(mm + "/" + dd + "/" + yyyy)
    }
    else if (condition == "Monthly") {
        dateholder.setDate(dateholder.getDate() + 30);
        dd = dateholder.getDate();
        mm = dateholder.getMonth() + 1;
        yyyy = dateholder.getFullYear();

        $('#txtdue_date').val(mm + "/" + dd + "/" + yyyy)
    }
    else {
        today = dateholder
        today.setDate(today.getDate() + 1);
        dd = today.getDate();
        mm = today.getMonth() + 1;
        yyyy = today.getFullYear();

        $('#txtdue_date').val(mm + "/" + dd + "/" + yyyy)
    }
}
function ChangePaymentScheme(loandays) {
    var condition = $('#txtpayment_scheme').find('option:selected').text()
    if (loandays >= 1) {
        if (loandays == 1) {
            $('#txtpayment_scheme').val(0);
            $('#txtinstallment_no').val("1")
        }
        else if (loandays >= 30) {
            $('#txtpayment_scheme').val(3);
            condition = $('#txtpayment_scheme').find('option:selected').text()
            if (condition == "Daily") {
                $('#txtinstallment_no').val(loandays)
            }
            else if (condition == "Weekly") {
                $('#txtinstallment_no').val(loandays / 7)
            }
            else if (condition == "Semi-Monthly") {
                $('#txtinstallment_no').val(loandays / 15)
            }
            else if (condition == "Monthly") {
                $('#txtinstallment_no').val(loandays / 30)
            }
        }
    }
    ChangeDueDate();
}

function totalRows(tableSelector) {
    var table = $(tableSelector);
    var rows = table.find("tr");
    var val, totals = [];

    //loop through the rows getting values in the rowDataSd columns
    rows
        .each(function (rIndex) {
            if (rIndex > 0 && rIndex < (rows.length - 1)) { //not first or last row
                //loop through the columns
                $(this).find("td").each(function (cIndex) {
                    val = parseInt($(this).html());
                    (totals.length > cIndex) ? totals[cIndex] += val : totals.push(val);
                });
            }
        })
        .last().find("td").each(function (index) {
            val = (totals.length > index) ? totals[index] : 0;
            $(this).html("Total: " + val);
        });
}