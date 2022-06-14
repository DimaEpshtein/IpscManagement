app.controller("LoginController", function ($scope, $rootScope, $http, $location, ipscManagementService, localStorageService) {

    var loginCtrl = this;
    loginCtrl.loading = false;
    loginCtrl.login = function () {
        loginCtrl.loading = true;
        var login = ipscManagementService.login(loginCtrl);
        login.then(function (d) {
            var response = d.data;
            localStorageService.set("authorizationData", { token: response.access_token, userName: loginCtrl.userName });
            loginCtrl.loading = false;
            $location.path("/BulletsStockManagement");
        }, function (err) {
            alert("אין אפשרות כניסה למערכת");
            $location.path("/");
        });
    };
});

app.controller("MemberController", function ($scope, $rootScope, $http, $location, ipscManagementService) {

    ipscManagementService.IsLogined();
    $("ul.navbar-nav li").removeClass("active");
    $("#member").addClass("active");
    $("#logout").click(function () {
        ipscManagementService.clearAllCookie();
    });
    if (!ipscManagementService.IsLogined()) {
        $location.path("/");
    }

    function isEmpty(val) {
        return (val === undefined || val == null || val.length <= 0) ? true : false;
    }

    var memberCtrl = this;
    memberCtrl.saveMember = function () {

        if (!isEmpty(memberCtrl.member.identity) && !isEmpty(memberCtrl.member.shooterIdentity) && !isEmpty(memberCtrl.member.firstName) && !isEmpty(memberCtrl.member.lastName) && !isEmpty(memberCtrl.member.email)) {

            var saveMember = ipscManagementService.saveMember(memberCtrl.member);
            saveMember.then(function (d) {
                memberCtrl.resultData = d.data;
                memberCtrl.member = {};
                alert("נשמר");
            }, function (err) {
                alert(err.data.message);
                alert("הפעולה נכשלה");
                //$location.path("/");
            });
        }
    };



    memberCtrl.loadMember = function () {
        var loadMember = ipscManagementService.loadMember(memberCtrl.member.identity);
        loadMember.then(function (d) {
            memberCtrl.member = d.data;
        }, function (err) {
            alert("הפעולה נכשלה");
            $location.path("/");
        });
    };
});

app.controller("BulletsStockManagementController", function ($scope, $rootScope, $http, $location, localStorageService, ipscManagementService) {

    ipscManagementService.IsLogined();
    $("ul.navbar-nav li").removeClass("active");
    $("#bulletsStockManagement").addClass("active");
    $("#logout").click(function () {
        ipscManagementService.clearAllCookie();
    });
    if (!ipscManagementService.IsLogined()) {
        $location.path("/");
    }

    var bulletsStockManagementCrtl = this;
    bulletsStockManagementCrtl.loading = true;
    function isEmpty(val) {
        return (val === undefined || val == null || val.length <= 0) ? true : false;
    }

    function isNormalInteger(str) {
        var n = Math.floor(Number(str));
        return String(n) === str && n >= 0;
    }

    var getStocks = ipscManagementService.getStocks();
    getStocks.then(function (d) {

        bulletsStockManagementCrtl.members = d.data;
        bulletsStockManagementCrtl.loading = false;
    }, function (err) {
        alert("הפעולה נכשלה");
        $location.path("/");
    });

    bulletsStockManagementCrtl.addAmount = function (identity, amount, remarks) {

        if (amount > 0 && !isEmpty(amount) && isNormalInteger(amount)) {
            bulletsStockManagementCrtl.loading = true;
            var updateAddAmount = ipscManagementService.updateAmount(identity, amount, 1, remarks);
            updateAddAmount.then(function (d) {
                var getStocks = ipscManagementService.getStocks();
                getStocks.then(function (d) {
                    bulletsStockManagementCrtl.members = d.data;
                    bulletsStockManagementCrtl.loading = false;
                }, function (err) {
                    alert("הפעולה נכשלה");
                    $location.path("/");
                });
                bulletsStockManagementCrtl.disableBtn = false;
            }, function (err) {
                alert("הפעולה נכשלה");
                $location.path("/");
            });
        }
        else {
            bulletsStockManagementCrtl.disableBtn = false;
        }
    };

    bulletsStockManagementCrtl.removeAmount = function (identity, amount, remarks) {
        if (amount > 0 && !isEmpty(amount) && isNormalInteger(amount)) {
            bulletsStockManagementCrtl.loading = true;
            var updateRemoveAmount = ipscManagementService.updateAmount(identity, amount, 2, remarks);
            updateRemoveAmount.then(function (d) {
                var getStocks = ipscManagementService.getStocks();
                getStocks.then(function (d) {
                    bulletsStockManagementCrtl.members = d.data;
                    bulletsStockManagementCrtl.loading = false;
                }, function (err) {
                    alert("הפעולה נכשלה");
                    $location.path("/");
                });
                bulletsStockManagementCrtl.disableBtn = false;
            }, function (err) {
                alert("הפעולה נכשלה");
                $location.path("/");
            });
        }
        else {
            bulletsStockManagementCrtl.disableBtn = false;
        }

    };

    bulletsStockManagementCrtl.history = function (identity) {
        localStorageService.set("selectedMemberHistoryId", { identity });
        $location.path("/MemberBulletsStockHistoryChanges");
        };
        });


app.controller("MemberBulletsStockHistoryChangesController", function ($scope, $rootScope, $http, $location, ipscManagementService) {

    ipscManagementService.IsLogined();
    $("ul.navbar-nav li").removeClass("active");
    $("#bulletsStockManagement").addClass("active");
    $("#logout").click(function () {
        ipscManagementService.clearAllCookie();
    });
    if (!ipscManagementService.IsLogined()) {
        $location.path("/");
    }

    var memberBulletsStockHistoryChangesCrtl = this;
    var loadMemberHistory = ipscManagementService.getMemberHistory();
    loadMemberHistory.then(function (d) {
        memberBulletsStockHistoryChangesCrtl.bulletsStockHistoryChanges = d.data;
    }, function (err) {
        alert("הפעולה נכשלה");
        $location.path("/");
    });
});

app.controller("WarehouseManagementController", function ($scope, $rootScope, $http, $location, ipscManagementService) {
    ipscManagementService.IsLogined();
    $("ul.navbar-nav li").removeClass("active");
    $("#warehouseManagement").addClass("active");
    $("#logout").click(function () {
        ipscManagementService.clearAllCookie();
    });
    if (!ipscManagementService.IsLogined()) {
        $location.path("/");
    }

    var warehouseManagementCrtl = this;

    var getWarehouseStock = ipscManagementService.getWarehouseBulletsStockAmmount();
    getWarehouseStock.then(function (d) {
        warehouseManagementCrtl.amount = d.data;
        warehouseManagementCrtl.loading = false;
    }, function (err) {
        alert("הפעולה נכשלה");
        //$location.path("/");
    });

    warehouseManagementCrtl.addAmount = function (changeAmount, remarks) {
        warehouseManagementCrtl.loading = true;
        var addAmount = ipscManagementService.updateWarehouseBulletsStock(changeAmount, 2, remarks);
        addAmount.then(function (d) {
            var getWarehouseStock = ipscManagementService.getWarehouseBulletsStockAmmount();
            getWarehouseStock.then(function (d) {
                warehouseManagementCrtl.amount = d.data;
                warehouseManagementCrtl.changeAmount = "";
                warehouseManagementCrtl.remarks = "";

                warehouseManagementCrtl.loading = false;
            }, function (err) {
                alert("הפעולה נכשלה");
                $location.path("/");
            });

        }, function (err) {
            alert("הפעולה נכשלה");
            $location.path("/");
        });
    };

    warehouseManagementCrtl.removeAmount = function (changeAmount, remarks) {
        warehouseManagementCrtl.loading = true;
        var addAmount = ipscManagementService.updateWarehouseBulletsStock(changeAmount, 1, remarks);
        addAmount.then(function (d) {
            var getWarehouseStock = ipscManagementService.getWarehouseBulletsStockAmmount();
            getWarehouseStock.then(function (d) {
                warehouseManagementCrtl.amount = d.data;
                warehouseManagementCrtl.changeAmount = "";
                warehouseManagementCrtl.remarks = "";

                warehouseManagementCrtl.loading = false;
            }, function (err) {
                alert("הפעולה נכשלה");
                $location.path("/");
            });

        }, function (err) {
            alert("הפעולה נכשלה");
            $location.path("/");
        });
    };


    warehouseManagementCrtl.updateAmount = function (changeAmount, remarks) {
        warehouseManagementCrtl.loading = true;
        var updateAmount = ipscManagementService.updateWarehouseBulletsStock(changeAmount, 3, remarks);
        updateAmount.then(function (d) {
            var getWarehouseStock = ipscManagementService.getWarehouseBulletsStockAmmount();
            getWarehouseStock.then(function (d) {
                warehouseManagementCrtl.amount = d.data;
                warehouseManagementCrtl.changeAmount = "";
                warehouseManagementCrtl.remarks = "";
                warehouseManagementCrtl.loading = false;
            }, function (err) {
                alert("הפעולה נכשלה");
                $location.path("/");
            });

        }, function (err) {
            alert("הפעולה נכשלה");
            $location.path("/");
        });
    };

    warehouseManagementCrtl.history = function (identity) {
        $location.path("/WarehouseManagementStockHistoryChanges");
    };
});

app.controller("WarehouseManagementStockHistoryChangesController", function ($scope, $rootScope, $http, $location, ipscManagementService) {

    ipscManagementService.IsLogined();
    $("ul.navbar-nav li").removeClass("active");
    $("#warehouseManagement").addClass("active");
    $("#logout").click(function () {
        ipscManagementService.clearAllCookie();
    });
    if (!ipscManagementService.IsLogined()) {
        $location.path("/");
    }

    var warehouseManagementStockHistoryChangesCrtl = this;
    var loadWarehouseHistory = ipscManagementService.getWarehouseHistory();
    loadWarehouseHistory.then(function (d) {
        warehouseManagementStockHistoryChangesCrtl.bulletsStockHistoryChanges = d.data;
    }, function (err) {
        alert("הפעולה נכשלה");
        $location.path("/");
    });
});

app.controller("ReportsController", function ($scope, $rootScope, $http, $location, ipscManagementService) {
    var reportsCtrl = this;
    $("ul.navbar-nav li").removeClass("active");
    $("#reports").addClass("active");
    $("#logout").click(function () {
        ipscManagementService.clearAllCookie();
    });

    if (!ipscManagementService.IsLogined()) {
        $location.path("/");
    }

    reportsCtrl.getReport = function () {
        ipscManagementService.getReport(reportsCtrl);
    };

    reportsCtrl.getWarehouseStocStatusReport = function () {
        ipscManagementService.getWarehouseStocStatusReport(reportsCtrl);
    };

});

