angular.module("plicd.icd9s.services")

.factory("icd9DetailService", ["icd9Repository", "$q", "utilities", function (icd9Repository, $q, utilities) {

    var self = this;

    /////////////////////////////
    // Private properties
    /////////////////////////////

    var _icd9 = new icd9();
    var _plans = [];            //icd9 plans
    var _filterText = "";
    var _sortColumn = "InsPlanId";
    var _sortDesc = false;
    var _minItemsToLoad = 150;
    var _defaultQuerySize = 50;
    var _querySize = _defaultQuerySize;
    var _defaultPlanStatus = "";
    var _planStatus = _defaultPlanStatus; //could be "on", "off", or ""    

    /////////////////////////////
    // Private functions
    /////////////////////////////

    var getIcd9 = function (code) {

        var deferred = $q.defer();

        icd9Repository.getIcd9(code)
            .then(function (results) {
                var resolved = {};
                resolved.icd9 = results.icd9;

                _icd9 = results.icd9; //store this so we can check it later

                deferred.resolve(resolved);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };

    var getIcd9Plans = function (code, filterText, startIndex, endIndex, status, sortColumn, sortDesc) {

        var deferred = $q.defer();

        icd9Repository.getIcd9Plans(code, filterText, startIndex, endIndex, status, sortColumn, sortDesc)
            .then(function (results) {

                _plans = results.plans;
                _filterText = filterText;
                _planStatus = status;
                _sortColumn = sortColumn;
                _sortDesc = sortDesc;

                deferred.resolve(results);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };

    var addIcd9Plan = function (code, planId) {

        var deferred = $q.defer();

        icd9Repository.addIcd9Plan(code, planId)
            .then(function (results) {
                var resolved = {};
                resolved.data = results.data;
                deferred.resolve(resolved);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };

    var removeIcd9Plan = function (code, planId) {

        var deferred = $q.defer();

        icd9Repository.removeIcd9Plan(code, planId)
            .then(function (results) {
                var resolved = {};
                resolved.data = results.data;
                deferred.resolve(resolved);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };

    /////////////////////////////
    // Public functions
    /////////////////////////////

    return {
        getIcd9: function (code) {
            return getIcd9(code);
        },
        getIcd9Plans: function (code, filterText, startIndex, endIndex, status, sortColumn, sortDesc) {
            return getIcd9Plans(code, filterText, startIndex, endIndex, status, sortColumn, sortDesc);
        },
        addIcd9Plan: function (code, planId) {
            return addIcd9Plan(code, planId);
        },
        removeIcd9Plan: function (code, planId) {
            return removeIcd9Plan(code, planId);
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
        getPlanStatus: function () {
            return _planStatus;
        },
        setPlanStatus: function (value) {
            _planStatus = value == "on" || value == "off" || value == "" ? value : _defaultPlanStatus;
        }
    }
}]);