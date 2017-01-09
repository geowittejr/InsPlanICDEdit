angular.module("plicd.icd9s.services")

.factory("icd9Repository", ["$http", "$filter", "$q", "utilities", function ($http, $filter, $q, utilities) {

    var self = this;

    /////////////////////////////
    // Private properties
    /////////////////////////////


    /////////////////////////////
    // Private functions
    /////////////////////////////

    var getIcd9s = function (filterText, startIndex, endIndex, sortColumn, sortDesc) {
        var deferred = $q.defer();

        var sortDescending = sortDesc ? 1 : 0;

        apiGetIcd9s(filterText, startIndex, endIndex, sortColumn, sortDescending)
            .then(function (results) {

                var resolved = new icd9ListQueryResults();

                resolved.totalIcd9s = results.data.totalIcd9s;
                resolved.icd9s = results.data.icd9s;
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

    var getIcd9 = function (code) {

        var deferred = $q.defer();

        ///////////////////////////////////////
        //
        // TODO:    Should we go back to the API for this, or just the local cache??
        //          If the local cache, then we need to update the cache with icd9count
        //          when we add new records
        //

        apiGetIcd9(code)
            .then(function (results) {
                var resolved = {};
                resolved.icd9 = results.data.icd9;
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

        var sortDescending = sortDesc ? 1 : 0;

        apiGetIcd9Plans(code, filterText, startIndex, endIndex, status, sortColumn, sortDescending)
            .then(function (results) {

                var resolved = new icd9PlansQueryResults();
                resolved.plans = results.data.plans;
                resolved.totalPlans = results.data.totalPlans;
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

    var addIcd9Plan = function (code, planId) {
        var deferred = $q.defer();

        apiPutIcd9Plan(code, planId)
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

        apiDeleteIcd9Plan(code, planId)
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

    var apiGetIcd9s = function (filterText, startIndex, endIndex, sortColumn, sortDesc) {
        return $http.get($http.apiUrl + "/icd9s?filter=" + filterText + "&start=" + startIndex + "&end=" + endIndex + "&sortCol=" + sortColumn + "&sortDesc=" + sortDesc);
    };

    var apiGetIcd9 = function (code) {
        code = code.replace(".", "-"); //Can't pass dots in url, so replace with dashes to get this to work.
        return $http.get($http.apiUrl + "/icd9s/" + code);
    };

    var apiGetIcd9Plans = function (code, filterText, startIndex, endIndex, status, sortColumn, sortDesc) {
        code = code.replace(".", "-"); //Can't pass dots in url, so replace with dashes to get this to work.
        return $http.get($http.apiUrl + "/icd9s/" + code + "/plans?filter=" + filterText + "&start=" + startIndex + "&end=" + endIndex + "&status=" + status + "&sortCol=" + sortColumn + "&sortDesc=" + sortDesc);
    };

    var apiPutIcd9Plan = function (code, planId) {
        code = code.replace(".", "-"); //Can't pass dots in url, so replace with dashes to get this to work.
        return $http.put($http.apiUrl + "/icd9s/" + code + "/add/" + planId);
    };

    var apiDeleteIcd9Plan = function (code, planId) {
        code = code.replace(".", "-"); //Can't pass dots in url, so replace with dashes to get this to work.
        return $http.delete($http.apiUrl + "/icd9s/" + code + "/remove/" + planId);
    };

    /////////////////////////////
    // Public functions
    /////////////////////////////

    return {
        getIcd9s: function (filterText, startIndex, endIndex, sortColumn, sortDesc) {
            return getIcd9s(filterText, startIndex, endIndex, sortColumn, sortDesc);
        },
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
        }
    }

}]);