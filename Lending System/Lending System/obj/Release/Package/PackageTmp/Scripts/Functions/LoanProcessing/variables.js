$('input.number').number(true, 2);
$('span.number').number(true, 4);

var today = new Date();
var dd = today.getDate();
var mm = today.getMonth() + 1; //January is 0!
var yyyy = today.getFullYear();
var interesttype = "";
var chargetype = "";
var totalinterest = "";
var loandays = "";