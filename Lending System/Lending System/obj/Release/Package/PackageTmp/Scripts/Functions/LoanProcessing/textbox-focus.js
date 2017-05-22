//Loan Grant
$('#txtloan_granted').on('focusout ', function () {
    ComputeTotalReceivables(interesttype)
})
//Interest Rate
$('#txtloan_interest_rate').on('focus ', function () {
    ComputeTotalReceivables(interesttype)
})