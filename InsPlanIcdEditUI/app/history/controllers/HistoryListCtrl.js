angular.module("plicd.history.controllers")

.controller("HistoryListCtrl", ["historyListService", "$stateParams", "$state", "$timeout", "authService",
    function (historyListService, $stateParams, $state, $timeout, authService) {

    var self = this;

    /////////////////////////////
    // Public properties
    /////////////////////////////

    self.entityType = $stateParams.type; //Possible values are "Icd9", "InsPlanId", "Username", or ""
    self.entityId = $stateParams.id;
    self.entityTitle = "";
    self.entityDesc = "";

    self.items = []; //history trans
    self.sortColumn = historyListService.getSortColumn();
    self.sortDesc = historyListService.getSortDesc();
    self.querySize = historyListService.getQuerySize();
    self.startIndex = -1
    self.endIndex = -1;

    //This is all used for infinite scrolling
    self.itemsLoaded = false;
    self.loadError = false;
    self.isLoading = false; //true when loading initial data
    self.isLoadingMore = false; //true when loading more data after scroll
    self.minItems = historyListService.getMinItemsToLoad();
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

    self.updateSearch = function () {
        //Reset query parameters   
        self.startIndex = -1;
        self.endIndex = -1;
        self.isLoading = true;
        self.loadMoreItems();
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
        
    self.loadMoreItems = function () {
        
        //Return if we're already at the end of the results
        if (self.itemsLoaded && self.endIndex > 0 && self.endIndex == self.totalItems - 1)
            return;

        var loadCountInstance = ++loadCount; //track so we know if the async results are stale upon return

        //Define the start and end indexes
        var startIndex = self.endIndex == -1 ? 0 : self.endIndex + 1;
        var endIndex = self.endIndex == -1 ? startIndex + self.querySize - 1 : startIndex + self.querySize;

        //Go get the data
        historyListService.getTrans(self.entityId, self.entityType, startIndex, endIndex, self.sortColumn, self.sortDesc)
            .then(function (results) {
                
                if (loadCountInstance != loadCount)
                    return; //another async call came after us

                self.items = results.startIndex == 0 ? results.trans : self.items.concat(results.trans);
                self.totalItems = results.totalTrans;
                self.startIndex = results.startIndex;
                self.endIndex = results.endIndex;
                self.itemCount = results.endIndex + 1;

                setItemsLoaded(true); //alerts the infiniteScroll directive we loaded items
            },
            function (error) {
                var message = error.message ? error.message : error.exceptionMessage ? error.exceptionMessage : "";
                alert("There was a problem loading history transactions. " + message);
                self.items = [];
                self.totalItems = 0;
                self.startIndex = -1;
                self.endIndex = -1;
                self.itemCount = 0;
                               
                setLoadError(true); //alerts the infiniteScroll directive we had an error
            });
    };

    self.itemClicked = function (itemId) {
        $state.transitionTo("home.plans.detail", { planId: itemId });
    };   

    //Used by dynamicResize directive to alert us when the window resizes
    self.onWindowResize = function () {
        self.windowResized = false;
        $timeout(function () {
            self.windowResized = true;
        });
    };

    /////////////////////////////
    // Private functions
    /////////////////////////////
    
    var getEntity = function () {

        //If nulls were passed to the page, don't bother calling the service
        if (self.entityId == null && self.entityType == null) {
            self.entityTitle = "All History";            
            return;
        }

        //Get the entity info so we can display it
        historyListService.getEntity(self.entityId, self.entityType)
            .then(function (results) {
                self.entityTitle = results.title + " History";
                self.entityDesc = results.description;
            },
            function (error) {
                self.entityTitle = self.entityId && self.entityId !== null ? self.entityId + " History" : "History";
                self.entityDesc = "";
            });
    };

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

    var init = function () {

        //Don't proceed if user is not authenticated or authorized
        if (!authService.isCurrentUserAuthenticated() || !authService.isCurrentUserAuthorized())
            return;

        getEntity();
        self.isLoading = true;
        self.loadMoreItems();
    };

    /////////////////////////////
    // Initially load data
    /////////////////////////////

    init();

}]);