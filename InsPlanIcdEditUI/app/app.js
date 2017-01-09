

//Bootstrap the main app
angular.element(document).ready(function () {
    angular.bootstrap(document, ["mainApp"]);
});

//Allow logging even if dev tools aren't open
if (!window.console) { window.console = { log: function () { } }; }

//Create all modules and inject their dependencies

//Directives
angular.module("plicd.common.directives", []);
angular.module("plicd.history.directives", []);

//Filters
angular.module("plicd.common.filters", []);

//Services
angular.module("plicd.common.services", [
    "plicd.users.services"
]);
angular.module("plicd.icd9s.services", []);
angular.module("plicd.plans.services", []);
angular.module("plicd.history.services", []);
angular.module("plicd.users.services", [
    "ngCookies"
]);

//Controllers
angular.module("plicd.home.controllers", [
    "plicd.common.services"
]);
angular.module("plicd.icd9s.controllers", [
    "plicd.common.directives",
    "plicd.common.filters",
    "plicd.common.services",
    "plicd.icd9s.services"
]);
angular.module("plicd.plans.controllers", [
    "plicd.common.directives",
    "plicd.common.filters",
    "plicd.common.services",
    "plicd.plans.services"
]);
angular.module("plicd.history.controllers", [
    "plicd.common.directives",
    "plicd.common.filters",
    "plicd.common.services",
    "plicd.history.directives",
    "plicd.history.services",
    "plicd.icd9s.services",
    "plicd.plans.services"
]);

//Create the main app module
angular.module("mainApp", [
    "ui.router",
    "plicd.common.services",
    "plicd.home.controllers",
    "plicd.plans.controllers",
    "plicd.icd9s.controllers",
    "plicd.history.controllers"
])
    
//**************************************************************************
// The config function allows us to configure any of our service providers. 
// You can only inject providers in this function, not service instances.
//**************************************************************************
.config(["$locationProvider", "$urlRouterProvider", "$stateProvider", function ($locationProvider, $urlRouterProvider, $stateProvider) {

    //Configure location provider
    $locationProvider.html5Mode(false);

    //Configure url route provider
    $urlRouterProvider
        .when("/", "/plans")
        .otherwise("/plans");
    
    //Configure state provider
    var homeState = {
        name: "home",
        abstract: true,
        templateUrl: "app/home/views/home.html"
    };

    var authenticatingState = {
        name: "home.authenticating",
        url: "/authenticating",
        templateUrl: "app/home/views/authenticating.html",
        allowAnonymous: true
    };

    var unauthenticatedState = {
        name: "home.unauthenticated",
        url: "/unauthenticated",
        templateUrl: "app/home/views/unauthenticated.html",
        allowAnonymous: true
    };

    var unauthorizedState = {
        name: "home.unauthorized",
        url: "/unauthorized",
        templateUrl: "app/home/views/unauthorized.html",
        allowAnonymous: true
    };

    var plansState = {
        name: "home.plans",
        abstract: true,
        template: "<div ui-view></div>"
    };

    var planListState = {
        name: "home.plans.list",
        url: "/plans",
        templateUrl: "app/plans/views/planList.html"
    };

    var planDetailState = {
        name: "home.plans.detail",
        url: "/plans/:planId",
        templateUrl: "app/plans/views/planDetail.html"
    };

    var icd9sState = {
        name: "home.icd9s",
        abstract: true,
        template: "<div ui-view></div>"
    };

    var icd9ListState = {
        name: "home.icd9s.list",
        url: "/icd9s",
        templateUrl: "app/icd9s/views/icd9List.html"
    };

    var icd9DetailState = {
        name: "home.icd9s.detail",
        url: "/icd9s/:code",
        templateUrl: "app/icd9s/views/icd9Detail.html"
    };

    var historyState = {
        name: "home.history",
        abstract: true,
        template: "<div ui-view></div>"
    };

    var historyListState = {
        name: "home.history.list",
        url: "/history?id&type",
        templateUrl: "app/history/views/historyList.html"
    };

    $stateProvider
        .state(homeState)
        .state(authenticatingState)
        .state(unauthenticatedState)
        .state(unauthorizedState)
        .state(plansState)
        .state(planListState)
        .state(planDetailState)
        .state(icd9sState)
        .state(icd9ListState)
        .state(icd9DetailState)
        .state(historyState)
        .state(historyListState);
}])

//**************************************************************************
// The run function is where the application starts up and links to the DOM. 
// Service instances can be injected here to configure them.
//**************************************************************************
.run(["$rootScope", "$state", "$stateParams", "$location", "$http", "authService", "$timeout", function ($rootScope, $state, $stateParams, $location, $http, authService, $timeout) {
    
    //Check the current URL for the environment we're running in.
    //Set the api url on the $http service according to the environmnent.
    //Also, set a test username if we are running on localhost.    
    var localApiUrl = "http://localhost:55000";
    var testUsername = "george";
    var devApiUrl = "https://devserver.com/icd";
    var productionApiUrl = "https://prodserver.com/icd";
    var host = $location.host().toLowerCase();
    $http.apiUrl = host.indexOf("devserver.com") >= 0 ? devApiUrl : host.indexOf("prodserver.com") >= 0 ? productionApiUrl : localApiUrl;
    if($http.apiUrl === localApiUrl)
        authService.setTestUsername(testUsername);
    
    var path = $location.$$path; //get the intially requested page
    $location.path("/authenticating"); //redirect temporarily to the authenticating page

    //Authenticate and then redirect
    authService.authenticateCurrentUser()
        .then(function (results) {

            var isAuthenticated = results.isAuthenticated;
            var isAuthorized = results.isAuthorized;
            var username = results.username;

            //Set the default authorization header for all API calls.                          
            if (isAuthenticated) {

                //Add the username to the default Authorization header to all API calls.
                //Keep in mind that we normally would send a password and username or a token, 
                //but this is what I have to work with until we have a better security workflow.
                $http.defaults.headers.common["Authorization"] = username;
            };

            if (!results.isAuthenticated) {
                //Transition user to unauthenticated state
                $state.transitionTo("home.unauthenticated");
                return;
            }

            if (!results.isAuthorized) {
                //Transition user to unauthorized state
                $state.transitionTo("home.unauthorized");
                return;
            }

            //Redirect after authenticating to the initially requested path
            $location.path(path);
        },
        function (results) {
            //Transition to unauthenticated state because an error happened
            $state.transitionTo("home.unauthenticated");
        });
       
    //We will use this event to check for authentication/authorization on each state object that requires it.
    //FYI - the $stateChangeStart event doesn't work in this situation...I think it's a bug in ui-router. It doesn't redraw the correct UI of the redirected state.
    $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
        
        if (!toState.allowAnonymous) {

            if (!authService.isCurrentUserAuthenticated()) {
                //Transition user to unauthenticated state
                $state.transitionTo("home.unauthenticated");
                return;
            }

            if (!authService.isCurrentUserAuthorized()) {
                //Transition user to unauthorized state
                $state.transitionTo("home.unauthorized");
                return;
            }
        }
    });

}]);