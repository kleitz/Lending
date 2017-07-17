var today = new Date();
var dd = today.getDate();
var mm = today.getMonth() + 1; //January is 0!
var yyyy = today.getFullYear();

app.loanAccountModelPrincipal =
    function (loanno,
        loantype,
        duedate,
        amountdue) {
        "use strict";

        var self = this;

        // #region MODEL TO BE MAP
        self.LoanNo = ko.observable(loanno);
        self.LoanType = ko.observable(loantype);
        self.DueDate = ko.observable(duedate);
        self.AmountDue = ko.observable(amountdue);
        // #endregion
    };
app.loanAccountModelInterest =
    function (loanno,
        loantype,
        duedate,
        amountdue) {
        "use strict";

        var self = this;

        // #region MODEL TO BE MAP
        self.LoanNo = ko.observable(loanno);
        self.LoanType = ko.observable(loantype);
        self.DueDate = ko.observable(duedate);
        self.AmountDue = ko.observable(amountdue);
        // #endregion
    };
app.createCollectionModel = function () {
    "use strict";

    var self = this;

    // #region MODEL TO CREATE/UPDATE
    self.LoanNo = ko.observable();
    self.LoanName = ko.observable();
    self.LoanDueDate = ko.observable();
    self.PaymentNo = ko.observable();
    self.PaymentDate = ko.observable();
    self.CustomerId = ko.observable();
    self.CustomerName = ko.observable();
    self.Payment = ko.observable();

    self.AmountDuePrincipal = ko.observable();
    self.AmountDueInterest = ko.observable();
    // #endregion   

    return self;
};