function GetCashBeginning() {
    
    $.ajax({
        url: RootUrl + "/EndDayTransaction/GetCashBegin",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            
            $('#txtcash_begin').val(response);
            GetCashReleased();
        },
        error: ""
    });
}
function GetCashReleased() {
    $.ajax({
        url: RootUrl + "/EndDayTransaction/GetCashRelease",
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
        url: RootUrl + "/EndDayTransaction/GetCashCollect",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            
            $('#txtcash_collected').val(response);
            GetCashPulledOut();
        },
        error: ""
    });
}
function GetCashPulledOut() {
    $.ajax({
        url: RootUrl + "/EndDayTransaction/GetCashPullOut",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            debugger
            $('#txtcash_pulled_out').val(response)
            GetCashEnd();
        },
        error: ""
    });
}
function GetCashEnd() {
    var begin = parseFloat($('#txtcash_begin').val().replace(/[^0-9\.]+/g, ""));
    var release = parseFloat($('#txtcash_release').val().replace(/[^0-9\.]+/g, ""));
    var collect = parseFloat($('#txtcash_collected').val().replace(/[^0-9\.]+/g, ""));
    var pullout = parseFloat($('#txtcash_pulled_out').val().replace(/[^0-9\.]+/g, ""));
    var balance = begin - release + collect - pullout;
    
    $('#txtcash_end').val(balance);
}

