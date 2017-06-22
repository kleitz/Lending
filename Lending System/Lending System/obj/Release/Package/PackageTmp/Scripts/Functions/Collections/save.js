//SAVING PROCESS
function AjaxSave() {
    var deferred = $.Deferred();
    var ref_no = $('#txtreference_no').val();

    var myObj = {
        "reference_no": ref_no,
        "date_trans": $('#txtdate_trans_main').val(),
        "payor_id": $('#txtpayor_id').val(),
        "payor_name": $('#txtpayor_name').val(),
        "total_amount": $('#txttotal_payment').val(),
        "created_by": "",
        "date_created": ""
    };

    $.ajax({
        url: RootUrl + "/Collections/SavePayment",
        type: "POST",
        data: JSON.stringify(myObj),
        contentType: 'application/json',
        success: function (data) {

        },
        error: function(data) {
            
        },
        complete: function (data) {
            $.when(AjaxSaveDetailsInterest().done(AjaxSaveDetailsPrincipal().done(AjaxSaveLedger()))).then(
                toastr.success('Successfully saved.', 'Save')
            ).done(
            
            );
        }
    });
    
    //AjaxSaveDetailsInterest();
    //AjaxSaveDetailsPrincipal();
    //AjaxSaveToLedger();
}
//STEP 1
function AjaxSaveDetailsInterest() {
    var deferred = $.Deferred();

    var totalRowCount = $("#interest-payment-table tr").length;
    var rowText;
    var ref_no = $('#txtreference_no').val();

    for (var i = 1; i < totalRowCount; i++) {

        rowText = document.getElementById("interest-payment-table").rows[i].cells[0].innerText;

        if (rowText == "No data available in table") {
            deferred.resolve();
        }
        else {
            if (parseFloat(document.getElementById("interest-payment-table").rows[i].cells[4].innerText.replace(/[^0-9\.]+/g, "")) != 0) {
                var param1 = document.getElementById("interest-payment-table").rows[i].cells[0].innerText;//loan_no
                var param2 = document.getElementById("interest-payment-table").rows[i].cells[1].innerText;//loan_name
                var param3 = document.getElementById("interest-payment-table").rows[i].cells[2].innerText;//due_date
                var param4 = parseFloat(document.getElementById("interest-payment-table").rows[i].cells[3].innerText.replace(/[^0-9\.]+/g, ""));//amount_due
                var param5 = parseFloat(document.getElementById("interest-payment-table").rows[i].cells[4].innerText.replace(/[^0-9\.]+/g, ""));//payment
                var param6 = document.getElementById("interest-payment-table").rows[i].cells[5].innerText;//interest_type
                var param7 = document.getElementById("interest-payment-table").rows[i].cells[6].innerText;//interest_rate

                var myObj = {
                    "reference_no": ref_no,
                    "payment_type": "OR Payment Interest",
                    "loan_no": document.getElementById("interest-payment-table").rows[i].cells[0].innerText,
                    "loan_name": document.getElementById("interest-payment-table").rows[i].cells[1].innerText,
                    "due_date": document.getElementById("interest-payment-table").rows[i].cells[2].innerText,
                    "amount": parseFloat(document.getElementById("interest-payment-table").rows[i].cells[4].innerText.replace(/[^0-9\.]+/g, "")),
                    "created_by": "",
                    "date_created": ""
                };
                $.ajax({
                    url: RootUrl + "/Collections/SavePaymentDetails",
                    type: "POST",
                    data: JSON.stringify(myObj),
                    contentType: 'application/json',
                    success: function (response) { deferred.resolve(); },
                    error: function (response) { deferred.reject(); }
                });
            }        
        }
    }
    return deferred.promise();
}
//STEP 2
function AjaxSaveDetailsPrincipal() {
    var deferred = $.Deferred();

    var totalRowCount = $("#principal-payment-table tr").length;
    var rowText;
    var ref_no = $('#txtreference_no').val();

    for (var i = 1; i < totalRowCount; i++) {

        rowText = document.getElementById("principal-payment-table").rows[i].cells[0].innerText;
        
        if (rowText == "No data available in table") {
            deferred.resolve();
            return false;
        }
        else {
            if (parseFloat(document.getElementById("principal-payment-table").rows[i].cells[4].innerText.replace(/[^0-9\.]+/g, "")) != 0) {
                var param1 = document.getElementById("principal-payment-table").rows[i].cells[0].innerText;//loan_no
                var param2 = document.getElementById("principal-payment-table").rows[i].cells[1].innerText;//loan_name
                var param3 = document.getElementById("principal-payment-table").rows[i].cells[2].innerText;//due_date
                var param4 = parseFloat(document.getElementById("principal-payment-table").rows[i].cells[3].innerText.replace(/[^0-9\.]+/g, ""));//amount_due
                var param5 = parseFloat(document.getElementById("principal-payment-table").rows[i].cells[4].innerText.replace(/[^0-9\.]+/g, ""));//payment
                var param6 = document.getElementById("principal-payment-table").rows[i].cells[5].innerText;//interest_type
                var param7 = document.getElementById("principal-payment-table").rows[i].cells[6].innerText;//interest_rate

                var myObj = {
                    "reference_no": ref_no,
                    "payment_type": "OR Payment",
                    "loan_no": document.getElementById("principal-payment-table").rows[i].cells[0].innerText,
                    "loan_name": document.getElementById("principal-payment-table").rows[i].cells[1].innerText,
                    "due_date": document.getElementById("principal-payment-table").rows[i].cells[2].innerText,
                    "amount": parseFloat(document.getElementById("principal-payment-table").rows[i].cells[4].innerText.replace(/[^0-9\.]+/g, "")),
                    "created_by": "",
                    "date_created": ""
                };
                $.ajax({
                    url: RootUrl + "/Collections/SavePaymentDetails",
                    type: "POST",
                    data: JSON.stringify(myObj),
                    contentType: 'application/json',
                    success: function (response) { deferred.resolve(); },
                    error: function (response) { deferred.reject(); },
                    complete: function(data) {
                        setTimeout(LoadReceipt(ref_no), 300);
                    }
                });
            }      
        }
    }
    return deferred.promise();
}


function AjaxSaveLedger() {
    var totalRowCountP = $("#principal-payment-table tr").length;
    var rowTextP;
    var totalRowCountI = $("#interest-payment-table tr").length;
    var rowTextI;

    var ref_no = $('#txtreference_no').val();

    var principal_payment;
    var interest_payment;
    var total_payment;

    var principalArray = [[], []];
    var InterestArray = [[], []];

    var found = false;

    for (var i = 1; i < totalRowCountP; i++) {
        rowTextP = document.getElementById("principal-payment-table").rows[i].cells[0].innerText;
        principal_payment = 0;
        interest_payment = 0;
        total_payment = 0;

        found = false;

        if (rowTextP != "No data available in table") {
            var param1 = document.getElementById("principal-payment-table").rows[i].cells[0].innerText;//loan_no
            var param2 = document.getElementById("principal-payment-table").rows[i].cells[1].innerText;//loan_name
            var param3 = document.getElementById("principal-payment-table").rows[i].cells[2].innerText;//due_date
            var param4 = parseFloat(document.getElementById("principal-payment-table").rows[i].cells[3].innerText.replace(/[^0-9\.]+/g, ""));//amount_due
            var param5 = parseFloat(document.getElementById("principal-payment-table").rows[i].cells[4].innerText.replace(/[^0-9\.]+/g, ""));//payment
            var param6 = document.getElementById("principal-payment-table").rows[i].cells[5].innerText;//interest_type
            var param7 = document.getElementById("principal-payment-table").rows[i].cells[6].innerText;//interest_rate

            principal_payment = param5;

            for (var ii = 1; ii < totalRowCountI; ii++) {
                rowTextI = document.getElementById("interest-payment-table").rows[ii].cells[0].innerText;

                if (rowTextI != "No data available in table") {
                    if (param1 == document.getElementById("interest-payment-table").rows[ii].cells[0].innerText) {
                        found = true
                        var iparam1 = document.getElementById("interest-payment-table").rows[ii].cells[0].innerText;//loan_no
                        var iparam2 = document.getElementById("interest-payment-table").rows[ii].cells[1].innerText;//loan_name
                        var iparam3 = document.getElementById("interest-payment-table").rows[ii].cells[2].innerText;//due_date
                        var iparam4 = parseFloat(document.getElementById("interest-payment-table").rows[ii].cells[3].innerText.replace(/[^0-9\.]+/g, ""));//amount_due
                        var iparam5 = parseFloat(document.getElementById("interest-payment-table").rows[ii].cells[4].innerText.replace(/[^0-9\.]+/g, ""));//payment
                        var iparam6 = document.getElementById("interest-payment-table").rows[ii].cells[5].innerText;//interest_type
                        var iparam7 = document.getElementById("interest-payment-table").rows[ii].cells[6].innerText;//interest_rate

                        interest_payment = interest_payment + iparam5;                      
                    }
                }
            }

            if (found == false) {
                AjaxSaveLedgerDetail("OR Payment", ref_no, param1, param2, param6, param7, interest_payment, principal_payment, param5, "0");
            }
            else {
                AjaxSaveLedgerDetail("OR Payment", ref_no, iparam1, iparam2, iparam6, iparam7, interest_payment, interest_payment + principal_payment, principal_payment, "0");
            }
        }
    }
}

function AjaxSaveToLedger() {
    var deferred = $.Deferred();
    var totalRowCount1 = $("#interest-payment-table tr").length;
    var rowText1;

    var ref_no = $('#txtreference_no').val();

    var principal_payment;
    var interest_payment;


    for (var i = 1; i < totalRowCount1; i++) {

        rowText1 = document.getElementById("interest-payment-table").rows[i].cells[0].innerText;
        
        if (rowText1 == "No data available in table") {
            interest_payment = 0;

            var totalRowCount2 = $("#principal-payment-table tr").length;
            var rowText2;
            
            for (var ii = 1; ii < totalRowCount2; ii++) {
                rowText2 = document.getElementById("principal-payment-table").rows[ii].cells[0].innerText;

                if (rowText2 == "No data available in table") {
                  
                }
                else {
                    var param1 = document.getElementById("principal-payment-table").rows[ii].cells[0].innerText;//loan_no
                    var param2 = document.getElementById("principal-payment-table").rows[ii].cells[1].innerText;//loan_name
                    var param3 = document.getElementById("principal-payment-table").rows[ii].cells[2].innerText;//due_date
                    var param4 = parseFloat(document.getElementById("principal-payment-table").rows[ii].cells[3].innerText.replace(/[^0-9\.]+/g, ""));//amount_due
                    var param5 = parseFloat(document.getElementById("principal-payment-table").rows[ii].cells[4].innerText.replace(/[^0-9\.]+/g, ""));//payment
                    var param6 = document.getElementById("principal-payment-table").rows[ii].cells[5].innerText;//interest_type
                    var param7 = document.getElementById("principal-payment-table").rows[ii].cells[6].innerText;//interest_rate


                    AjaxSaveLedgerDetail("OR Payment", ref_no, param1, param2, param6, param7, interest_payment, param5 + interest_payment, param5, "0");
                }
            }
        }
        else {
            if (parseFloat(document.getElementById("interest-payment-table").rows[i].cells[4].innerText.replace(/[^0-9\.]+/g, "")) != 0) {
                var param1 = document.getElementById("interest-payment-table").rows[i].cells[0].innerText;//loan_no
                var param2 = document.getElementById("interest-payment-table").rows[i].cells[1].innerText;//loan_name
                var param3 = document.getElementById("interest-payment-table").rows[i].cells[2].innerText;//due_date
                var param4 = parseFloat(document.getElementById("interest-payment-table").rows[i].cells[3].innerText.replace(/[^0-9\.]+/g, ""));//amount_due
                var param5 = parseFloat(document.getElementById("interest-payment-table").rows[i].cells[4].innerText.replace(/[^0-9\.]+/g, ""));//payment
                var param6 = document.getElementById("interest-payment-table").rows[i].cells[5].innerText;//interest_type
                var param7 = document.getElementById("interest-payment-table").rows[i].cells[6].innerText;//interest_rate

                interest_payment = param5;

                var totalRowCount2 = $("#principal-payment-table tr").length;
                var rowText2;
                
                for (var ii = 1; ii < totalRowCount2; ii++) {
                    rowText2 = document.getElementById("principal-payment-table").rows[ii].cells[0].innerText;

                    if (param1 == document.getElementById("principal-payment-table").rows[ii].cells[0].innerText) {
                        if (rowText2 == "No data available in table") {
                           
                        }
                        else {
                            var param1 = document.getElementById("principal-payment-table").rows[ii].cells[0].innerText;//loan_no
                            var param2 = document.getElementById("principal-payment-table").rows[ii].cells[1].innerText;//loan_name
                            var param3 = document.getElementById("principal-payment-table").rows[ii].cells[2].innerText;//due_date
                            var param4 = parseFloat(document.getElementById("principal-payment-table").rows[ii].cells[3].innerText.replace(/[^0-9\.]+/g, ""));//amount_due
                            var param5 = parseFloat(document.getElementById("principal-payment-table").rows[ii].cells[4].innerText.replace(/[^0-9\.]+/g, ""));//payment
                            var param6 = document.getElementById("principal-payment-table").rows[ii].cells[5].innerText;//interest_type
                            var param7 = document.getElementById("principal-payment-table").rows[ii].cells[6].innerText;//interest_rate


                            AjaxSaveLedgerDetail("OR Payment", ref_no, param1, param2, param6, param7, interest_payment, param5 + interest_payment, param5, "0");                      
                        }
                    }
                }
            }
        }
    }
    deferred = deferred.resolve();

    return deferred.promise();
}

function AjaxSaveLedgerDetail(trans_type, reference_no, loan_id, loan_name, interest_type, interest_rate, interest, amount_paid, principal, balance) {
    var deferred = $.Deferred();

    var ref_no = $('#txtreference_no').val();
    
    var myObj = {

        "date_trans": $('#txtdate_trans_main').val(),
        "trans_type": trans_type,
        "reference_no": reference_no,
        "loan_no": loan_id,
        "loan_type_name": loan_name,
        "customer_id": $('#txtpayor_id').val(),
        "customer_name": $('#txtpayor_name').val(),
        "interest_type": interest_type,
        "interest_rate": interest_rate,
        "interest": interest,
        "amount_paid": amount_paid,
        "principal": principal,
        "balance": balance,
        "date_created": "",
        "created_by": ""
    };

    $.ajax({
        url: RootUrl + "/Collections/SaveLedger",
        type: "POST",
        data: JSON.stringify(myObj),
        contentType: 'application/json',
        success: function (response) { deferred.resolve(); },
        error: function (response) { deferred.reject(); },
        complete: function(data) {

        }

    });

    return deferred.promise();
}
