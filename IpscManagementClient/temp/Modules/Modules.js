var app = angular.module("IpscManagement", ["ngRoute", "LocalStorageModule", "ngMaterial"]);

app.controller("memberCtrl", function () {
    this.myDate = new Date();
    this.isOpen = false;
});

app.config(function ($routeProvider) {

    $routeProvider.when("/",
    {
        templateUrl: "../Managment/ManagmentViews/LogIn.html",
        controller: "LoginController as loginCtrl"
    });
    $routeProvider.when("/Member",
    {
        templateUrl: "../Managment/ManagmentViews/Member.html",
        controller: "MemberController as memberCtrl"
    });

    $routeProvider.when("/BulletsStockManagement",
    {
        templateUrl: "../Managment/ManagmentViews/BulletsStockManagement.html",
        controller: "BulletsStockManagementController as bulletsStockManagementCrtl"
    });

    $routeProvider.when("/MemberBulletsStockHistoryChanges",
    {
        templateUrl: "../Managment/ManagmentViews/MemberBulletsStockHistoryChanges.html",
        controller: "MemberBulletsStockHistoryChangesController as memberBulletsStockHistoryChangesCrtl"
    });

    $routeProvider.when("/WarehouseManagment",
    {
        templateUrl: "../Managment/ManagmentViews/WarehouseManagement.html",
        controller: "WarehouseManagementController as warehouseManagementCrtl"
    });

    $routeProvider.when("/WarehouseManagementStockHistoryChanges",
    {
        templateUrl: "../Managment/ManagmentViews/WarehouseManagementStockHistoryChanges.html",
        controller: "WarehouseManagementStockHistoryChangesController as warehouseManagementStockHistoryChangesCrtl"
    });
	$routeProvider.when("/Reports",
    {
        templateUrl: "../Managment/ManagmentViews/Reports.html",
        controller: "ReportsController as reportsCtrl"
    });
});
