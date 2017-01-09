angular.module("plicd.common.directives")

//This directive sets up a loader on the target element.

.directive("loader", ["$timeout", function ($timeout) {
    return {
        restrict: 'A',
        scope: false,
        link: function (scope, element, attrs) {

            var loadText = attrs.loaderText ? attrs.loaderText : "Loading...";

            element.append("<div class='loader loader-bkgrd'></div>")
            element.append("<div class='loader loader-outer'><div class='loader-inner'>" + loadText + "</div></div>")          
            element.children("div.loader").hide();

            //Show/hide the loader div
            scope.$watch(attrs.loader, function (loading) {
                if (loading === true) {
                    element.children("div.loader").show();
                }
                else {
                    element.children("div.loader").hide();
                }
            });
        }
    };
}]);