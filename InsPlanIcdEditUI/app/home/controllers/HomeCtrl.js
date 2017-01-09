angular.module("plicd.home.controllers")

.controller("HomeCtrl", ["$location", "authService", "$state", function ($location, authService, $state) {
    
    var self = this;
    
    /////////////////////////////
    // Public properties
    /////////////////////////////


    /////////////////////////////
    // Public functions
    /////////////////////////////

    self.isActive = function (path) {
        return $location.path().indexOf(path) == 0;
    };
    
    /////////////////////////////
    // Private functions
    /////////////////////////////

    var init = function () {
        //
    };

    /////////////////////////////
    // Initial data load
    /////////////////////////////

    init();

}]);