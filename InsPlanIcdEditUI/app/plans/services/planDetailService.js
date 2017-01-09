angular.module("plicd.plans.services")

.factory("planDetailService", ["planRepository", "$q", "utilities", function (planRepository, $q, utilities) {

    var self = this;

    /////////////////////////////
    // Private properties
    /////////////////////////////

    var _plan = new insPlan();
    var _icd9s = [];            //plan icd9s
    var _filterText = "";
    var _sortColumn = "Icd9";
    var _sortDesc = false;
    var _minItemsToLoad = 150;
    var _defaultQuerySize = 50;
    var _querySize = _defaultQuerySize;
    var _defaultIcd9Status = "";
    var _icd9Status = _defaultIcd9Status; //could be "on", "off", or ""    

    /////////////////////////////
    // Private functions
    /////////////////////////////
        
    var getPlan = function (planId) {

        var deferred = $q.defer();

        planRepository.getPlan(planId)
            .then(function (results) {
                var resolved = {};
                resolved.plan = results.plan;

                _plan = results.plan; //store this so we can check it later

                deferred.resolve(resolved);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };

    var getPlanIcd9s = function (planId, filterText, startIndex, endIndex, status, sortColumn, sortDesc) {

        var deferred = $q.defer();

        planRepository.getPlanIcd9s(planId, filterText, startIndex, endIndex, status, sortColumn, sortDesc)
            .then(function (results) {
                                
                _icd9s = results.icd9s;
                _filterText = filterText;
                _icd9Status = status;
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

    var addPlanIcd9 = function (planId, icd9) {

        var deferred = $q.defer();

        planRepository.addPlanIcd9(planId, icd9)
            .then(function (results) {
                var resolved = {};
                resolved.data = results.data;
                deferred.resolve(resolved);

                //TODO: Update local plans cached to reflect new icd9s count and icd9s cache to reflect plans count
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };

    var removePlanIcd9 = function (planId, icd9) {

        var deferred = $q.defer();

        planRepository.removePlanIcd9(planId, icd9)
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
        getPlan: function (planId) {
            return getPlan(planId);
        },
        getPlanIcd9s: function (planId, filterText, startIndex, endIndex, status, sortColumn, sortDesc) {
            return getPlanIcd9s(planId, filterText, startIndex, endIndex, status, sortColumn, sortDesc);
        },
        addPlanIcd9: function(planId, icd9){
            return addPlanIcd9(planId, icd9);
        },
        removePlanIcd9: function(planId, icd9){
            return removePlanIcd9(planId, icd9);
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
        getIcd9Status: function(){
            return _icd9Status;
        },
        setIcd9Status: function(value){
            _icd9Status = value == "on" || value == "off" || value == "" ? value : _defaultIcd9Status;
        }
    }
}]);