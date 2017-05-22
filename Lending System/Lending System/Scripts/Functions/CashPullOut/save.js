function AjaxSave() {
    var deferred = $.Deferred();
    var ref_no = $('#txtreference_no').val();

    var myObj = {
        "date_trans": $('#txtdate_trans').val(),
        "username": $('#txtusername').val(),
        "datefrom": "",
        "dateto": "",
        "amount": $('#txtamount').val(),
        "created_by": "",
        "date_created": ""
    };

    $.ajax({
        url: RootUrl + "/CashPullOut/Save",
        type: "POST",
        data: JSON.stringify(myObj),
        contentType: 'application/json',
        success: function (response, status, xhr) {
            toastr.success('Successfully saved.', 'Save');
        },
        error: ""
    });

    window.location.href = RootUrl + "/CashPullOut/Index"

    return deferred.promise();
}