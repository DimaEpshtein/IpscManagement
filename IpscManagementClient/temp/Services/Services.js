app.service("ipscManagementService", function business($http, $location, localStorageService) {

    this.login = function (user) {
        var data = "grant_type=password&username=" + user.username + "&password=" + user.password;
        var req = $http.post("http://www.ipsc.co.il/Token", data,
            { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }
            );
        return req;
    };

	function formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [year, month, day].join('-');
    }

    this.saveMember = function (member) {
		
		member.dateofBirthStr = formatDate(member.dateofBirth);
		
        var req = $http.post("http://www.ipsc.co.il/api/IpscManagement/setMember", member,
            { headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': "Bearer " + localStorageService.get("authorizationData").token } }
            );
        return req;
    };

    this.loadMember = function (identity) {
        var req = $http.post("http://www.ipsc.co.il/api/IpscManagement/getMember", identity,
            { headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': "Bearer " + localStorageService.get("authorizationData").token } }
            );
        return req;
    };


    this.getStocks = function () {
        var req = $http.post("http://www.ipsc.co.il/api/IpscManagement/getStocks", null,
            { headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': "Bearer " + localStorageService.get("authorizationData").token } }
            );
        return req;
    };

    this.updateAmount = function (identity, amount, amountUpdateType, remarks) {
        var amountChange = {};
        amountChange.memberIdentity = identity;
        amountChange.amount = amount;
        amountChange.amountUpdateType = amountUpdateType;
        amountChange.remarks = remarks;
        
        var req = $http.post("http://www.ipsc.co.il/api/IpscManagement/updateAmount", amountChange, {
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': "Bearer " + localStorageService.get("authorizationData").token }

        });
        return req;
    };

    this.getMemberHistory = function () {
        
        var req = $http.post("http://www.ipsc.co.il/api/IpscManagement/getMemberBulletsStockHistoryChanges", localStorageService.get("selectedMemberHistoryId").identity, {
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': "Bearer " + localStorageService.get("authorizationData").token }
        });
        return req;
    };


    this.IsLogined = function ()
    {
        if (localStorageService.get("authorizationData") === null) {
            $location.path("/");
        }
    }
    

    this.getWarehouseBulletsStockAmmount = function () {
        var req = $http.post("http://www.ipsc.co.il/api/IpscManagement/getWarehouseBulletsStockAmmount", null,
            { headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': "Bearer " + localStorageService.get("authorizationData").token } }
            );
        return req;
    };

    this.updateWarehouseBulletsStock = function (amount, amountUpdateType, remarks) {
        var amountChange = {};
        amountChange.amount = amount;
        amountChange.amountUpdateType = amountUpdateType;
        amountChange.remarks = remarks;

        var req = $http.post("http://www.ipsc.co.il/api/IpscManagement/updateWarehouseBulletsStock", amountChange, {
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': "Bearer " + localStorageService.get("authorizationData").token }

        });
        return req;
    };

    this.getWarehouseHistory = function () {

        var req = $http.post("http://www.ipsc.co.il/api/IpscManagement/getWarehouseStockHistoryChanges", null, {
            headers: { 'Accept': 'application/json', 'Content-Type': 'application/json', 'Authorization': "Bearer " + localStorageService.get("authorizationData").token }
        });
        return req;
    };
	function parseDate(dateString) {
        var m = moment(dateString, 'D.M.YYYY', true);
        return m.isValid() ? m.toDate() : new Date(NaN);
    };
	 this.getReport = function (reporFilter) {
        window.open("http://www.ipsc.co.il/api/IpscManagement/getReport?bearer=" + localStorageService.get("authorizationData").token + "&identity=" + reporFilter.identity + "&fromDate=" + reporFilter.fromDate + "&toDate=" + reporFilter.toDate, '_blank', '');
    };
this.clearAllCookie = function () {
        localStorageService.clearAll();
    };

    this.IsLogined = function () {
        if (localStorageService.get("authorizationData") === null) {
            return false;
        }
        return true;
    };
});