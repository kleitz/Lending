function AjaxSave() {
    var deferred = $.Deferred();
    var ref_no = $('#txtreference_no').val();

    var myObj = {
        "date_trans": $('#txtdate_trans').val(),
        "cash_begin": $('#txtcash_begin').val(),
        "cash_release": $('#txtcash_release').val(),
        "cash_collected": $('#txtcash_collected').val(),
        "cash_pulled_out": $('#txtcash_pulled_out').val(),
        "cash_end": $('#txtcash_end').val()
    };

    $.ajax({
        url: RootUrl + "/EndDayTransaction/Save",
        type: "POST",
        data: JSON.stringify(myObj),
        contentType: 'application/json',
        success: function (response, status, xhr) {
            if (response == "Success") {
                toastr.success('Successfully saved.', 'Save');
                List.InitializeEvents();
            }
            else {
                toastr.error('Failed to Save', 'Cancel');
            }
        },
        error: ""
    });

    return deferred.promise();
}