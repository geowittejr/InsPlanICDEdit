angular.module("plicd.plans.services")

.factory("planListService", ["planRepository", "$q", "utilities", function (planRepository, $q, utilities) {

    var self = this;

    /////////////////////////////
    // Private properties
    /////////////////////////////

    var _plans = []; //insurance plans
    var _filterText = "";
    var _sortColumn = "InsPlanId";
    var _sortDesc = false;
    var _minItemsToLoad = 150;
    var _defaultQuerySize = 50;
    var _querySize = _defaultQuerySize;
    var _selectedItem = new selectedPlanData();    

    /////////////////////////////
    // Private functions
    /////////////////////////////

    var getPlans = function (filterText, startIndex, endIndex, sortColumn, sortDesc) {

        var deferred = $q.defer();

        planRepository.getPlans(filterText, startIndex, endIndex, sortColumn, sortDesc)
            .then(function (results) {

                _plans = results.plans;
                _filterText = filterText;
                _sortColumn = sortColumn;
                _sortDesc = sortDesc;

                //deferred.reject("test");
                deferred.resolve(results);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };
        
    var resetSelectedItem = function () {
        _selectedItem.planId = "";
        _selectedItem.itemIndex = -1;
  
        return angular.copy(_selectedItem);
    };

    var incrementSelectedItem = function () {
        if (_selectedItem.itemIndex < _plans.length - 1) {
            _selectedItem.itemIndex++;
        }
        _selectedItem.planId = _plans[_selectedItem.itemIndex].planId;
        return angular.copy(_selectedItem);
    };

    var decrementSelectedItem = function () {

        if (_selectedItem.itemIndex == -1) {
            _selectedItem.itemIndex = 0;
        }
        else if (_selectedItem.itemIndex > 0) {
            _selectedItem.itemIndex--;
        }
        _selectedItem.planId = _plans[_selectedItem.itemIndex].planId;
        return angular.copy(_selectedItem);
    };
    

    /////////////////////////////
    // Public functions
    /////////////////////////////

    return {
        getPlans: function (filterText, startIndex, endIndex, sortColumn, sortDesc) {
            return getPlans(filterText, startIndex, endIndex, sortColumn, sortDesc);
        },
        getFilterText: function () {
            return _filterText;
        },
        getSortColumn: function(){
            return _sortColumn;
        },
        getSortDesc: function(){
            return _sortDesc;
        },
        getQuerySize: function(){
            return parseInt(_querySize);
        },
        setQuerySize: function(value){
            _querySize = utilities.ensureInt(value, _defaultQuerySize);
        },
        getMinItemsToLoad: function(){
            return parseInt(_minItemsToLoad);
        },
        getSelectedItem: function () {
            return angular.copy(_selectedItem);
        },        
        resetSelectedItem: function () {
            return resetSelectedItem();
        },
        incrementSelectedItem: function () {
            return incrementSelectedItem();
        },
        decrementSelectedItem: function () {
            return decrementSelectedItem();
        }
    }
}]);