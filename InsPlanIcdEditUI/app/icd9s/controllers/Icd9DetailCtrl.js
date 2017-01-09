angular.module("plicd.icd9s.controllers")

.controller("Icd9DetailCtrl", ["icd9DetailService", "$stateParams", "$state", "$timeout", "authService", function (icd9DetailService, $stateParams, $state, $timeout, authService) {

    var self = this;

    /////////////////////////////
    // Public properties
    /////////////////////////////

    self.icd9 = new icd9();
    self.items = []; //insurance plans    
    self.filterText = icd9DetailService.getFilterText();
    self.planStatus = icd9DetailService.getPlanStatus();
    self.sortColumn = icd9DetailService.getSortColumn();
    self.sortDesc = icd9DetailService.getSortDesc();
    self.querySize = icd9DetailService.getQuerySize();
    self.startIndex = -1;
    self.endIndex = -1;
    self.filterTextHasFocus = true;

    //This is all used for infinite scrolling
    self.itemsLoaded = false;
    self.loadError = false;
    self.isLoading = false; //true when loading initial data
    self.isLoadingMore = false; //true when loading more data after scroll
    self.minItems = icd9DetailService.getMinItemsToLoad();
    self.totalItems = 0;
    self.itemCount = 0;
    self.windowResized = false;
    self.scrollBuffer = 50;

    /////////////////////////////
    // Private properties
    /////////////////////////////

    var loadCount = 0; //used to determine if async results of loadMoreItems() are stale already


    /////////////////////////////
    // Public functions
    /////////////////////////////
    
    self.clearFilter = function () {
        self.filterText = "";
        self.updateSearch();
    };

    self.updateSearch = function () {
        //Reset query parameters   
        self.startIndex = -1;
        self.endIndex = -1;
        self.isLoading = true;
        self.loadMoreItems();
    };

    self.updateStatusHeader = function () {
        var status = self.planStatus.toLowerCase();
        self.planStatus = status === "on" ? "off" : status === "" ? "on" : "";
        self.updateSearch();
    };

    self.getStatusHeader = function () {
        var status = self.planStatus.toLowerCase();
        return status === "on" ? "LMN (On)" : status === "off" ? "LMN (Off)" : "LMN (All)";
    };

    self.updateSort = function (column) {
        if (self.sortColumn.toLowerCase() === column.toLowerCase()) {
            self.sortDesc = !self.sortDesc;
        }
        else {
            self.sortDesc = false;
        }
        self.sortColumn = column;
        self.updateSearch();
    };

    self.getSortClass = function (column) {
        if (self.sortColumn.toLowerCase() === column.toLowerCase()) {
            return self.sortDesc ? "sort-desc" : "sort-asc";
        }
        return "";
    };

    self.getIcd9 = function () {
        var code = $stateParams.code;
        icd9DetailService.getIcd9(code)
            .then(function (results) {

                if (results.icd9 && results.icd9 != "null") {
                    self.icd9 = results.icd9;
                }
                else {
                    self.icd9 = new icd9();
                    self.icd9.code = "Error";
                    self.icd9.description = "There was a problem loading the ICD9.";
                }
            },
            function (error) {
                var message = error.message ? error.message : error.exceptionMessage ? error.exceptionMessage : "";
                alert("There was a problem loading the ICD9. " + message);

                self.icd9 = new icd9();
                self.icd9.code = "Error";
                self.icd9.description = "There was a problem loading the ICD9.";
            });
    };

    self.loadMoreItems = function () {

        //Return if we're already at the end of the results
        if (self.itemsLoaded && self.endIndex > 0 && self.endIndex == self.totalItems - 1)
            return;

        var loadCountInstance = ++loadCount; //track so we know if the async results are stale upon return

        //Define the start and end indexes
        var startIndex = self.endIndex == -1 ? 0 : self.endIndex + 1;
        var endIndex = self.endIndex == -1 ? startIndex + self.querySize - 1 : startIndex + self.querySize;

        //Go get the data
        var code = $stateParams.code;
        icd9DetailService.getIcd9Plans(code, self.filterText, startIndex, endIndex, self.planStatus, self.sortColumn, self.sortDesc)
            .then(function (results) {

                if (loadCountInstance != loadCount)
                    return; //another async call came after us

                self.items = results.startIndex == 0 ? results.plans : self.items.concat(results.plans);

                angular.forEach(self.items, function (value, key) {
                    var item = this[key];
                    item.statusAlert = {};
                    item.statusAlert.cssClass = "";
                    item.statusAlert.text = "";
                    item.statusAlert.updating = false;
                    item.statusAlert.show = false;
                }, self.items);

                self.totalItems = results.totalPlans;
                self.startIndex = results.startIndex;
                self.endIndex = results.endIndex;
                self.itemCount = results.endIndex + 1;

                setItemsLoaded(true); //alerts the infiniteScroll directive we loaded items
            },
            function (error) {
                var message = error.message ? error.message : error.exceptionMessage ? error.exceptionMessage : "";
                alert("There was a problem loading insurance plans. " + message);
                self.items = [];
                self.totalItems = 0;
                self.startIndex = -1;
                self.endIndex = -1;
                self.itemsCount = 0;

                setLoadError(true); //alerts the infiniteScroll directive we had an error
            });
    };

    self.itemClicked = function (itemId) {
        $state.transitionTo("home.plans.detail", { planId: itemId });
    };

    self.updateStatus = function (index) {
        var item = self.items[index];
        if (item) {
            if (item.enabledOnIcd9) {
                disableItem(item);
            } else {
                enableItem(item);
            }
        }
    };

    self.onWindowResize = function () {
        self.windowResized = false;
        $timeout(function () {
            self.windowResized = true;
        });
    };

    /////////////////////////////
    // Private functions
    /////////////////////////////

    var setItemsLoaded = function (val) {
        self.itemsLoaded = !val;
        $timeout(function () {
            self.itemsLoaded = val;
        });
    };

    var setLoadError = function (val) {
        self.loadError = !val;
        $timeout(function () {
            self.loadError = val;
        });
    };

    //Set focus on the filterText input control
    var setFocusToFilterText = function () {
        self.filterTextHasFocus = false;
        $timeout(function () {
            self.filterTextHasFocus = true;
        });
    };

    var enableItem = function (item) {

        //If status is in the middle of being updated just exit
        if (item.statusAlert.updating)
            return;

        item.statusAlert.updating = true;
        item.enabledOnIcd9 = true;

        icd9DetailService.addIcd9Plan(self.icd9.code, item.planId)
            .then(function (results) {
                item.icd9Count = item.icd9Count + 1;
                item.statusAlert.cssClass = "added";
                item.statusAlert.text = "Added " + item.planId;
                item.statusAlert.show = true;
            },
            function (error) {
                item.statusAlert.cssClass = "error";
                item.statusAlert.text = "Error adding " + item.planId;
                item.statusAlert.show = true;
                item.enabledOnIcd9 = false;
            });
    };

    var disableItem = function (item) {

        //If status is in the middle of being updated just exit
        if (item.statusAlert.updating)
            return;

        item.statusAlert.updating = true;
        item.enabledOnIcd9 = false;

        icd9DetailService.removeIcd9Plan(self.icd9.code, item.planId)
            .then(function (results) {
                item.icd9Count = item.icd9Count >= 1 ? item.icd9Count - 1 : 0;
                item.statusAlert.cssClass = "removed";
                item.statusAlert.text = "Removed " + item.planId;
                item.statusAlert.show = true;
            },
            function (error) {
                item.statusAlert.cssClass = "error";
                item.statusAlert.text = "Error removing " + item.planId;
                item.statusAlert.show = true;
                item.enabledOnIcd9 = false;
            });
    };

    var init = function () {
        
        //Don't proceed if user is not authenticated or authorized
        if (!authService.isCurrentUserAuthenticated() || !authService.isCurrentUserAuthorized())
            return;

        self.isLoading = true;
        self.getIcd9();
        self.loadMoreItems();
    };

    /////////////////////////////
    // Initial data load
    /////////////////////////////

    init();

}]);