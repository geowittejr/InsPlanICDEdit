angular.module("plicd.common.services")

.factory("authService", ["$q", "userRepository", "utilities", function ($q, userRepository, utilities) {

    var _testUsername = null; //store username to use in testing (null by default)
    var _currentUsername = null;
    var _isCurrentUserAuthorized = false;
    
    var authenticateCurrentUser = function () {
        
        var deferred = $q.defer();
        var resolved = { username: null, isAuthenticated: false, isAuthorized: false };

        getCurrentUsername()
            .then(function (results) {

                _currentUsername = results.username && results.username !== null ? results.username : null;

                userRepository.isAuthorized(_currentUsername)
                    .then(function (results) {

                        _isCurrentUserAuthorized = results.isAuthorized ? results.isAuthorized : false;
                        
                        resolved.username = _currentUsername;
                        resolved.isAuthenticated = _currentUsername !== null;
                        resolved.isAuthorized = _isCurrentUserAuthorized;

                        deferred.resolve(resolved);
                    });
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };

    var getCurrentUsername = function () {

        var resolved = { username: null };

        var deferred = $q.defer();

        if(_testUsername !== null){
            resolved.username = _testUsername;
            deferred.resolve(resolved);
            return deferred.promise;
        }

        userRepository.getCurrentUsername()
            .then(function (results) {
                resolved.username = results.username;
                deferred.resolve(resolved);
            },
            function (results) {
                var error = utilities.extractError(results);
                deferred.reject(error);
            });

        return deferred.promise;
    };


    return {
        authenticateCurrentUser: function(){
            return authenticateCurrentUser();
        },
        getCurrentUsername: function(){
            return getCurrentUsername();
        },
        isCurrentUserAuthenticated: function(){
            return _currentUsername !== null;
        },
        isCurrentUserAuthorized: function () {
            return _isCurrentUserAuthorized;
        },
        setTestUsername: function(username){
            _testUsername = username;
        }
    }
}]);