function ShowModal() {
    $('#Modal').modal('show');
    $('#txtdate_trans').val(mm + "/" + dd + "/" + yyyy);

    GetCashBeginning();

    setTimeout(function () {
    }, 300);
};

function Save() {
    $('#Modal').modal('hide');

    AjaxSave();

    setTimeout(function () {
    }, 300);
};

function Cancel() {
    $('#Modal').modal('hide');

    setTimeout(function () {
    }, 300);
};