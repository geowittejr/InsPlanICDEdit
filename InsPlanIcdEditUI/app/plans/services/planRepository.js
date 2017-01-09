angular.module("plicd.plans.services")

.factory("planRepository", ["$http", "$filter", "$q", "utilities", function ($http, $filter, $q, utilities) {

    var self = this;

    /////////////////////////////
    // Private properties
    /////////////////////////////


    /////////////////////////////
    // Private functions
    /////////////////////////////

    var getPlans = function (filterText, startIndex, endIndex, sortColumn, sortDesc) {
        var deferred = $q.defer();

        var sortDescending = sortDesc ? 1 : 0;

        apiGetPlans(filterText, startIndex, endIndex, sortColumn, sortDescending)
            .then(function (results) {

                var resolved = new planListQueryResults();

                resolved.totalPlans = results.data.totalPlans;
                resolved.plans = results.data.plans;
                resolved.startIndex = results.data.startIndex;
                resolved.endIndex = results.data.endIndex;
                
                deferred.resolve(resolved);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };
     
    var getPlan = function (planId) {

        var deferred = $q.defer();

        ///////////////////////////////////////
        //
        // TODO:    Should we go back to the API for this, or just the local cache??
        //          If the local cache, then we need to update the cache with icd9count
        //          when we add new records
        //

        apiGetPlan(planId)
            .then(function (results) {
                var resolved = {};
                resolved.plan = results.data.plan;
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

        var sortDescending = sortDesc ? 1 : 0;

        apiGetPlanIcd9s(planId, filterText, startIndex, endIndex, status, sortColumn, sortDescending)
            .then(function (results) {

                var resolved = new planIcd9sQueryResults();                                
                resolved.icd9s = results.data.icd9s;
                resolved.totalIcd9s = results.data.totalIcd9s;
                resolved.startIndex = results.data.startIndex;
                resolved.endIndex = results.data.endIndex;

                deferred.resolve(resolved);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };

    var addPlanIcd9 = function (planId, icd9) {
        var deferred = $q.defer();

        apiPutPlanIcd9(planId, icd9)
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

    var removePlanIcd9 = function (planId, icd9) {
        var deferred = $q.defer();

        apiDeletePlanIcd9(planId, icd9)
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

    var apiGetPlans = function (filterText, startIndex, endIndex, sortColumn, sortDesc) {
        return $http.get($http.apiUrl + "/plans?filter=" + filterText + "&start=" + startIndex + "&end=" + endIndex + "&sortCol=" + sortColumn + "&sortDesc=" + sortDesc);
    };

    var apiGetPlan = function (planId) {
        return $http.get($http.apiUrl + "/plans/" + planId);
    };

    var apiGetPlanIcd9s = function (planId, filterText, startIndex, endIndex, status, sortColumn, sortDesc) {
        return $http.get($http.apiUrl + "/plans/" + planId + "/icd9s?filter=" + filterText + "&start=" + startIndex + "&end=" + endIndex + "&status=" + status + "&sortCol=" + sortColumn + "&sortDesc=" + sortDesc);
    };

    var apiPutPlanIcd9 = function (planId, icd9) {
        icd9 = icd9.replace(".", "-"); //Can't pass dots in url, so replace with dashes to get this to work.
        return $http.put($http.apiUrl + "/plans/" + planId + "/add/" + icd9);
    };

    var apiDeletePlanIcd9 = function (planId, icd9) {
        icd9 = icd9.replace(".", "-"); //Can't pass dots in url, so replace with dashes to get this to work.
        return $http.delete($http.apiUrl + "/plans/" + planId + "/remove/" + icd9);
    };    

    /////////////////////////////
    // Public functions
    /////////////////////////////

    return {
        getPlans: function(filterText, startIndex, endIndex, sortColumn, sortDesc){
            return getPlans(filterText, startIndex, endIndex, sortColumn, sortDesc);
        },
        getPlan: function (planId) {
            return getPlan(planId);
        },
        getPlanIcd9s: function (planId, filterText, startIndex, endIndex, status, sortColumn, sortDesc) {
            return getPlanIcd9s(planId, filterText, startIndex, endIndex, status, sortColumn, sortDesc);
        },
        addPlanIcd9: function (planId, icd9) {
            return addPlanIcd9(planId, icd9);
        },
        removePlanIcd9: function (planId, icd9) {
            return removePlanIcd9(planId, icd9);
        }
    }

}]);