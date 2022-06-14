var app = angular.module("IpscManagement", ["ngRoute", "LocalStorageModule", "ngMaterial"]);

app.controller("memberCtrl", function () {
    this.myDate = new Date();
    this.isOpen = false;
});

app.config(function ($routeProvider) {

    $routeProvider.when("/",
    {
        templateUrl: "LogIn.html",
        controller: "LoginController as loginCtrl"
    });
    $routeProvider.when("/Member",
    {
        templateUrl: "Member.html",
        controller: "MemberController as memberCtrl"
    });

    $routeProvider.when("/BulletsStockManagement",
    {
        templateUrl: "BulletsStockManagement.html",
        controller: "BulletsStockManagementController as bulletsStockManagementCrtl"
    });

    $routeProvider.when("/MemberBulletsStockHistoryChanges",
    {
        templateUrl: "MemberBulletsStockHistoryChanges.html",
        controller: "MemberBulletsStockHistoryChangesController as memberBulletsStockHistoryChangesCrtl"
    });

    $routeProvider.when("/WarehouseManagment",
    {
        templateUrl: "WarehouseManagement.html",
        controller: "WarehouseManagementController as warehouseManagementCrtl"
    });

    $routeProvider.when("/WarehouseManagementStockHistoryChanges",
    {
        templateUrl: "WarehouseManagementStockHistoryChanges.html",
        controller: "WarehouseManagementStockHistoryChangesController as warehouseManagementStockHistoryChangesCrtl"
    });
    $routeProvider.when("/Reports",
    {
        templateUrl: "Reports.html",
        controller: "ReportsController as reportsCtrl"
    });
    $routeProvider.when("/NewCompetition",
    {
        templateUrl: "NewCompetition.html",
        controller: "NewCompetitionController as newCompetitionCtrl"
    });
});
