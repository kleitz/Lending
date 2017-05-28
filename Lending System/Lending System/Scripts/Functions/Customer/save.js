function AjaxSave() {
    var id = $('#autonum').val();

    var myObj = {
        "customer_no": $('#customer_no').val(),
        "date_registered": $('#date_registered').val(),
        "firstname": $('#firstname').val(),
        "middlename": $('#middlename').val(),
        "lastname": $('#lastname').val(),
        "civil_status": $('#civil_status').val(),
        "address": $('#address').val(),
        "contact_no": $('#contact_no').val(),
        "email": $('#email').val(),
        "date_of_birth": $('#date_of_birth').val(),
        "birth_place": $('#birth_place').val(),
        "occupation": $('#occupation').val(),
        "credit_limit": $('#credit_limit').val(),
        "annual_income": $('#annual_income').val()
    };

    $.ajax({
        url: RootUrl + "Customer/Update?id=" + id,
        type: "POST",
        data: JSON.stringify(myObj),
        contentType: 'application/json',
        success: function (response, status, xhr) {
            if (response == "Success") {
                toastr.success('Successfully reviewed.');
                window.location.href = RootUrl + "Customer/Index";
            }
            else {
                toastr.error('failed to review.');
            }
        },
        error: function (response, status, xhr) {
            toastr.error('failed to review.');
        }
    });
}