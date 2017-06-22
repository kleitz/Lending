var loaderApp;
loaderApp = loaderApp || (function () {
    var pleaseWaitDiv = $('<div class="modal" id="pleaseWaitDialog" data-backdrop="static" data-keyboard="false"> <div class="modal-dialog modal-sm" style="margin-top: 20%;"> <div class="modal-content"> <div class="modal-body" style="padding: 15px;"> <h5>Processing...</h5> <div class="progress"> <div class="progress-bar progress-bar-primary progress-bar-striped active" role="progressbar" aria-valuemax="100" style="width: 100%;"></div></div></div></div></div></div>');
    return {
        showPleaseWait: function() {
            pleaseWaitDiv.modal();
        },
        hidePleaseWait: function () {
            pleaseWaitDiv.modal('hide');
        },

    };
})();