angular.module("plicd.history.services")

.factory("historyListService", ["historyRepository", "planRepository", "icd9Repository", "$q", "utilities",
    function (historyRepository, planRepository, icd9Repository, $q, utilities) {

    var self = this;

    /////////////////////////////
    // Private properties
    /////////////////////////////

    var _sortColumn = "ActionDate";
    var _sortDesc = true;
    var _minItemsToLoad = 150;
    var _defaultQuerySize = 50;
    var _querySize = _defaultQuerySize;   

    /////////////////////////////
    // Private functions
    /////////////////////////////

    var getTrans = function (entityId, entityType, startIndex, endIndex, sortColumn, sortDesc) {

        var deferred = $q.defer();

        historyRepository.getTrans(entityId, entityType, startIndex, endIndex, sortColumn, sortDesc)
            .then(function (results) {

                //Uncomment the following lines to "remember" sort settings upon history page revisits
                //_sortColumn = sortColumn;
                //_sortDesc = sortDesc;

                deferred.resolve(results);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };

    var getEntity = function (entityId, entityType) {

        var deferred = $q.defer();

        //Validate entity data passed in
        var id = entityId && entityId !== null ? entityId : "";
        var type = entityType && entityType !== null ? entityType.toLowerCase() : "";

        //Set default return values
        var resolved = {};
        resolved.title = id;
        resolved.description = "";

        if (entityId == null && entityType == null) {

            //NULL values denote that all history was requested.            
            resolved.title = "";
            resolved.description = "";

            deferred.resolve(resolved);
        }
        else if (type == "icd9") {

            //Get the ICD-9 info
            icd9Repository.getIcd9(entityId)
                .then(function (results) {

                    if (results.icd9) {
                        resolved.title = results.icd9.code;
                        resolved.description = results.icd9.description;
                    }
                    deferred.resolve(resolved);
                },
                function (error) {
                    deferred.resolve(resolved);
                });
        }
        else if (type == "insplanid") {

            //Get the ins plan info
            planRepository.getPlan(entityId)
                .then(function (results) {

                    if (results.plan) {
                        resolved.title = results.plan.planId;
                        resolved.description = results.plan.insCoDesc;
                    }
                    deferred.resolve(resolved);
                },
                function (error) {
                    deferred.resolve(resolved);
                });
        }
        else if (type == "username") {

            //TODO: not sure if we will grab the user info from the database or not...
            resolved.title = id;
            resolved.description = "";
            deferred.resolve(resolved);
        }
        else {
            //The type is weird or something, so send as is.
            deferred.resolve(resolved);
        };

        return deferred.promise;
    };     

    /////////////////////////////
    // Public functions
    /////////////////////////////

    return {
        getTrans: function (entityId, entityType, startIndex, endIndex, sortColumn, sortDesc) {
            return getTrans(entityId, entityType, startIndex, endIndex, sortColumn, sortDesc);
        },
        getEntity: function(entityId, entityType){
            return getEntity(entityId, entityType);
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
        }
    }
}]);