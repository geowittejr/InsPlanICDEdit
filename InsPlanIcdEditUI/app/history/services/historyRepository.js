angular.module("plicd.history.services")

.factory("historyRepository", ["$http", "$q", "utilities", function ($http, $q, utilities) {

    var self = this;

    /////////////////////////////
    // Private properties
    /////////////////////////////


    /////////////////////////////
    // Private functions
    /////////////////////////////

    var getTrans = function (entityId, entityType, startIndex, endIndex, sortColumn, sortDesc) {
        var deferred = $q.defer();

        var sortDescending = sortDesc ? 1 : 0;

        apiGetTrans(entityId, entityType, startIndex, endIndex, sortColumn, sortDescending)
            .then(function (results) {

                var resolved = new historyListQueryResults();                
                resolved.trans = results.data.trans;
                resolved.totalTrans = results.data.totalTrans;
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
 
    var apiGetTrans = function (entityId, entityType, startIndex, endIndex, sortColumn, sortDesc) {
        var id = entityId && entityId.length > 0 ? entityId : "";
        var type = entityType && entityType.length > 0 ? entityType : "";
        if (type.toLowerCase() == "icd9")
            id = id.replace(".", "-"); //Can't pass dots in url, so replace with dashes to get this to work.

        return $http.get($http.apiUrl + "/history?id=" + id + "&type=" + type + "&start=" + startIndex + "&end=" + endIndex + "&sortCol=" + sortColumn + "&sortDesc=" + sortDesc);
    };


    /////////////////////////////
    // Public functions
    /////////////////////////////

    return {
        getTrans: function(entityId, entityType, startIndex, endIndex, sortColumn, sortDesc){
            return getTrans(entityId, entityType, startIndex, endIndex, sortColumn, sortDesc);
        }
    }

}]);