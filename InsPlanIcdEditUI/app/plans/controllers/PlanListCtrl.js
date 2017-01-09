angular.module("plicd.plans.controllers")

.controller("PlanListCtrl", ["planListService", "$state", "$timeout", "authService", function (planListService, $state, $timeout, authService) {

    var self = this;

    /////////////////////////////
    // Public properties
    /////////////////////////////

    self.items = []; //insurance plans
    self.filterText = planListService.getFilterText();
    self.sortColumn = planListService.getSortColumn();
    self.sortDesc = planListService.getSortDesc();
    self.querySize = planListService.getQuerySize();
    self.startIndex = -1
    self.endIndex = -1;
    self.filterTextHasFocus = true;
    self.selectedItem = planListService.getSelectedItem();

    //This is all used for infinite scrolling
    self.itemsLoaded = false;
    self.loadError = false;
    self.isLoading = false; //true when loading initial data
    self.isLoadingMore = false; //true when loading more data after scroll
    self.minItems = planListService.getMinItemsToLoad();
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
        resetSelectedItem();
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
        planListService.getPlans(self.filterText, startIndex, endIndex, self.sortColumn, self.sortDesc)
            .then(function (results) {

                if (loadCountInstance != loadCount)
                    return; //another async call came after us

                self.items = results.startIndex == 0 ? results.plans : self.items.concat(results.plans);
                self.totalItems = results.totalPlans;
                self.startIndex = results.startIndex;
                self.endIndex = results.endIndex;
                self.itemCount = results.endIndex + 1;

                setItemsLoaded(true); //alerts the infiniteScroll directive we loaded items
            },
            function (error) {
                var message = error.message ? error.message : error.exceptionMessage ? error.exceptionMessage : "";
                alert("There was a problem loading plans. " + message);
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

    /////////////////////////////////////////////////
    //
    // TODO:    Move the keydown functionality for selecting a row to a directive that uses jquery to increment or decrement
    //          the selected row, then make a call to scope.updateSelectedItem. Doing it that way
    //          would probably not be as sluggish on the UI.
    //

    self.keydown = function (event) {
        switch (event.keyCode) {
            case 13: //Enter key

                if (self.selectedItem.planId) {
                    self.itemClicked(self.selectedItem.planId);
                }

                break;
            case 40: //Down arrow

                setFocusToFilterText();
                incrementSelectedItem();
                adjustScrollView(true);

                break;
            case 38: //Up arrow

                setFocusToFilterText();
                decrementSelectedItem();
                adjustScrollView(false);
                break;
        }
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

    var resetSelectedItem = function () {
        self.selectedItem = planListService.resetSelectedItem();
    };

    var incrementSelectedItem = function () {
        self.selectedItem = planListService.incrementSelectedItem();
    };

    var decrementSelectedItem = function () {
        self.selectedItem = planListService.decrementSelectedItem();
    };

    //Set focus on the filterText input control
    var setFocusToFilterText = function () {
        self.filterTextHasFocus = false;
        $timeout(function () {
            self.filterTextHasFocus = true;
        });
    };


    /////////////////////////////////////////////////
    //
    // TODO:    This doesn't work if you scroll back up to the top manually after selecting a row and then hit the down arrow
    //          when a selected row is below the viewable area.  It doesn't bring the row into view.
    //

    //Adjust the scrolltop so that our selected row is in the viewable area.
    var adjustScrollView = function (movingDown) {

        var row = angular.element("#row-" + self.selectedItem.itemIndex.toString());
        var rowTop = row.offset().top;
        var rowBottom = rowTop + row.height();

        var container = angular.element("#plansTableContainer");
        var containerTop = container.offset().top;
        var containerBottom = containerTop + container.height();

        //Adjust the scroll view area

        if (rowBottom > containerBottom) {
            //element is below the viewable area

            if (movingDown) {
                container.scrollTop(container.scrollTop() + row.height());
            }
            else {
                var amt = containerTop - rowTop + row.height();
                container.scrollTop(container.scrollTop() - amt);
            }
        }
        else if (rowTop < containerTop) {
            //element is above the viewable area
            if (movingDown) {
                var amt = containerTop - rowTop + row.height();
                container.scrollTop(container.scrollTop() - amt);
            }
            else {
                var amt = containerTop - rowTop + row.height();
                container.scrollTop(container.scrollTop() - amt);
            }
        }
    };

    var init = function () {

        //Don't proceed if user is not authenticated or authorized
        if (!authService.isCurrentUserAuthenticated() || !authService.isCurrentUserAuthorized())
            return;

        self.isLoading = true;
        self.loadMoreItems();
    };

    /////////////////////////////
    // Initially load data
    /////////////////////////////

    init();

}]);