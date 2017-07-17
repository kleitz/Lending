function GetCashReleased() {
    $.ajax({
        url: RootUrl + "Home/GetCashRelease",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            
            $('#txtcash_release').val(response);
            GetCashCollected();
        },
        error: ""
    });
}
function GetCashCollected() {
    $.ajax({
        url: RootUrl + "Home/GetCashCollect",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            
            $('#txtcash_collected').val(response);
            GetReceivablesForTheDay();
        },
        error: ""
    });
}
function GetCashPulledOut() {
    $.ajax({
        url: RootUrl + "Home/GetCashPullOut",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#txtcash_pulled_out').val(response);
        },
        error: ""
    });
}
function GetReceivablesForTheDay() {
    $.ajax({
        url: RootUrl + "Home/GetReceivablesForTheDay",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            $('#txtcash_pulled_out').val(response);
        },
        error: ""
    });
}