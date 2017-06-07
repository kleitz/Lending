$(document).ready(function () {
    if (RootUrl == "/") {
        RootUrl = ""
    }

    $('#asOf').val(mm + "/" + dd + "/" + yyyy);
});

$('input.number').number(true, 2);
$('span.number').number(true, 4);

var today = new Date();
var dd = today.getDate();
var mm = today.getMonth() + 1; //January is 0!
var yyyy = today.getFullYear();