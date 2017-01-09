angular.module("plicd.icd9s.services")

.factory("icd9ListService", ["icd9Repository", "$q", "utilities", function (icd9Repository, $q, utilities) {

    var self = this;

    /////////////////////////////
    // Private properties
    /////////////////////////////

    var _icd9s = []; //icd9s
    var _filterText = "";
    var _sortColumn = "Icd9";
    var _sortDesc = false;
    var _minItemsToLoad = 150;
    var _defaultQuerySize = 50;
    var _querySize = _defaultQuerySize;
    var _selectedItem = new selectedPlanData();

    /////////////////////////////
    // Private functions
    /////////////////////////////

    var getIcd9s = function (filterText, startIndex, endIndex, sortColumn, sortDesc) {

        var deferred = $q.defer();

        icd9Repository.getIcd9s(filterText, startIndex, endIndex, sortColumn, sortDesc)
            .then(function (results) {

                _icd9s = results.icd9s;
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
        _selectedItem.code = "";
        _selectedItem.itemIndex = -1;

        return angular.copy(_selectedItem);
    };

    var incrementSelectedItem = function () {
        if (_selectedItem.itemIndex < _icd9s.length - 1) {
            _selectedItem.itemIndex++;
        }
        _selectedItem.icd9 = _icd9s[_selectedItem.itemIndex].code;
        return angular.copy(_selectedItem);
    };

    var decrementSelectedItem = function () {

        if (_selectedItem.itemIndex == -1) {
            _selectedItem.itemIndex = 0;
        }
        else if (_selectedItem.itemIndex > 0) {
            _selectedItem.itemIndex--;
        }
        _selectedItem.icd9 = _icd9s[_selectedItem.itemIndex].code;
        return angular.copy(_selectedItem);
    };


    /////////////////////////////
    // Public functions
    /////////////////////////////

    return {
        getIcd9s: function (filterText, startIndex, endIndex, sortColumn, sortDesc) {
            return getIcd9s(filterText, startIndex, endIndex, sortColumn, sortDesc);
        },
        getFilterText: function () {
            return _filterText;
        },
        getSortColumn: function () {
            return _sortColumn;
        },
        getSortDesc: function () {
            return _sortDesc;
        },
        getQuerySize: function () {
            return parseInt(_querySize);
        },
        setQuerySize: function (value) {
            _querySize = utilities.ensureInt(value, _defaultQuerySize);
        },
        getMinItemsToLoad: function () {
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