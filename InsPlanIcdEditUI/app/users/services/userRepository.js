angular.module("plicd.users.services")

.factory("userRepository", ["$http", "$cookies", "$q", function ($http, $cookies, $q) {


    var self = this;

    /////////////////////////////
    // Private properties
    /////////////////////////////


    /////////////////////////////
    // Private functions
    /////////////////////////////

    var getCurrentUsername = function () {

        var deferred = $q.defer();        

        var resolved = {};        
        var username = $cookies.username;
        //username = "gwitte";
        resolved.username = username && username !== null ? username : null;
        deferred.resolve(resolved);

        return deferred.promise;
    };

    var isAuthorized = function (username) {

        var deferred = $q.defer();

        apiGetUser(username)
            .then(function (results) {

                var resolved = { isAuthorized: false };
                var user = results.data.user;
                if (user && user !== null) {
                    resolved.isAuthorized = user.isAuthorized ? user.isAuthorized : false;
                }
                deferred.resolve(resolved);
            });

        return deferred.promise;        
    };
    
    var apiGetUser = function (username) {
        return $http.get($http.apiUrl + "/users/" + username);
    };


    /////////////////////////////
    // Public functions
    /////////////////////////////

    return {
        getCurrentUsername: function () {
            return getCurrentUsername();
        },
        isAuthorized: function (username) {
            return isAuthorized(username);
        }
    }
}]);