$(document).ready(function () {
    $("#btnnext").click(function () {
        if ($('#txtloan_granted').val() == 0) {
            alert("Please input loan amount.");
        }
        else {
            CreateSchedule();
            $('.nav-pills a[href="#step2"]').tab('show');
        }
    });
    $("#btnnext1").click(function () {
        if ($('#txtloan_granted').val() == 0) {
            alert("Please input loan amount.");
        }
        else {
            $('.nav-pills a[href="#step3"]').tab('show');
        }
    });
});