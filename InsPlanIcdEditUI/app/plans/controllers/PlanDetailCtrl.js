angular.module("plicd.plans.controllers")

.controller("PlanDetailCtrl", ["planDetailService", "$stateParams", "$state", "$timeout", "authService", function (planDetailService, $stateParams, $state, $timeout, authService) {

    var self = this;

    /////////////////////////////
    // Public properties
    /////////////////////////////
    
    self.alertMessages = [];

    self.plan = new insPlan();
    self.items = []; //icd9s    
    self.filterText = planDetailService.getFilterText();
    self.icd9Status = planDetailService.getIcd9Status();
    self.sortColumn = planDetailService.getSortColumn();
    self.sortDesc = planDetailService.getSortDesc();
    self.querySize = planDetailService.getQuerySize();
    self.startIndex = -1;
    self.endIndex = -1;
    self.filterTextHasFocus = true;    

    //This is all used for infinite scrolling
    self.itemsLoaded = false;
    self.loadError = false;
    self.isLoading = false; //true when loading initial data
    self.isLoadingMore = false; //true when loading more data after scroll
    self.minItems = planDetailService.getMinItemsToLoad();
    self.totalItems = 0;
    self.itemCount = 0;
    self.windowResized = false;
    self.scrollBuffer = 50;

    /////////////////////////////
    // Private properties
    /////////////////////////////

    var loadCount = 0; //used to determine if async results of loadMoreItems() are stale already
    var enableCount = 0; //used to determine if async results of enableItem() are stale already
    var disableCount = 0; //used to determine if async results of disableItem() are stale already
    var statusUpdateCount = 0; //used to determine if async results of disableItem() or enableItem() are stale already

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
        var status = self.icd9Status.toLowerCase();
        self.icd9Status = status === "on" ? "off" : status === "" ? "on" : "";
        self.updateSearch();
    };

    self.getStatusHeader = function () {
        var status = self.icd9Status.toLowerCase();
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

    self.getPlan = function () {
        var planId = $stateParams.planId;
        planDetailService.getPlan(planId)
            .then(function (results) {

                if (results.plan && results.plan != "null") {
                    self.plan = results.plan;
                }
                else {
                    self.plan = new insPlan();
                    self.plan.planId = "Error";
                    self.plan.insCoDesc = "There was a problem loading the plan.";
                }
            },
            function (error) {
                var message = error.message ? error.message : error.exceptionMessage ? error.exceptionMessage : "";
                alert("There was a problem loading the plan. " + message);

                self.plan = new insPlan();
                self.plan.planId = "Error";
                self.plan.insCoDesc = "There was a problem loading the plan.";
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
        var planId = $stateParams.planId;
        planDetailService.getPlanIcd9s(planId, self.filterText, startIndex, endIndex, self.icd9Status, self.sortColumn, self.sortDesc)
            .then(function (results) {

                if (loadCountInstance != loadCount)
                    return; //another async call came after us

                self.items = results.startIndex == 0 ? results.icd9s : self.items.concat(results.icd9s);

                angular.forEach(self.items, function (value, key) {
                    var item = this[key];
                    item.statusAlert = {};
                    item.statusAlert.cssClass = "";
                    item.statusAlert.text = "";
                    item.statusAlert.updating = false;
                    item.statusAlert.show = false;
                }, self.items);

                self.totalItems = results.totalIcd9s;
                self.startIndex = results.startIndex;
                self.endIndex = results.endIndex;
                self.itemCount = results.endIndex + 1;

                setItemsLoaded(true); //alerts the infiniteScroll directive we loaded items
            },
            function (error) {
                var message = error.message ? error.message : error.exceptionMessage ? error.exceptionMessage : "";
                alert("There was a problem loading ICD9 codes. " + message);
                self.items = [];
                self.totalItems = 0;
                self.startIndex = -1;
                self.endIndex = -1;
                self.itemsCount = 0;

                setLoadError(true); //alerts the infiniteScroll directive we had an error
            });
    };

    self.itemClicked = function (itemId) {
        $state.transitionTo("home.icd9s.detail", { code: itemId.replace(".", "-") });
    };

    self.updateStatus = function (index) {
        var item = self.items[index];
        if (item) {
            if (item.enabledOnPlan) {
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
        item.enabledOnPlan = true;

        planDetailService.addPlanIcd9(self.plan.planId, item.code)
            .then(function (results) {
                item.insPlanCount = item.insPlanCount + 1;
                item.statusAlert.cssClass = "added";
                item.statusAlert.text = "Added " + item.code;
                item.statusAlert.show = true;
            },
            function (error) {
                item.statusAlert.cssClass = "error";
                item.statusAlert.text = "Error adding " + item.code;
                item.statusAlert.show = true;
                item.enabledOnPlan = false;
            });
    };

    var disableItem = function (item) {

        //If status is in the middle of being updated just exit
        if (item.statusAlert.updating)
            return;

        item.statusAlert.updating = true;
        item.enabledOnPlan = false;

        planDetailService.removePlanIcd9(self.plan.planId, item.code)
            .then(function (results) {
                item.insPlanCount = item.insPlanCount >= 1 ? item.insPlanCount - 1 : 0;
                item.statusAlert.cssClass = "removed";
                item.statusAlert.text = "Removed " + item.code;
                item.statusAlert.show = true;
            },
            function (error) {
                item.statusAlert.cssClass = "error";
                item.statusAlert.text = "Error removing " + item.code;
                item.statusAlert.show = true;
                item.enabledOnPlan = true;
            });
    };

    var init = function () {

        //Don't proceed if user is not authenticated or authorized
        if (!authService.isCurrentUserAuthenticated() || !authService.isCurrentUserAuthorized())
            return;

        self.isLoading = true;
        self.getPlan();
        self.loadMoreItems();
    };

    /////////////////////////////
    // Initial data load
    /////////////////////////////
        
    init();

}]);