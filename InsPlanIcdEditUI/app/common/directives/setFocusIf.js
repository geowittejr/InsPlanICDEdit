angular.module("plicd.common.directives")

//This directive allows us to set focus to the element upon evaluation of the "setFocusIf" expression to true.
.directive("setFocusIf", ["$timeout", function ($timeout) {
    return function (scope, element, attrs) {
        scope.$watch(attrs.setFocusIf, function (expression) {
            if (expression) {
                $timeout(function () {
                    element.focus();
                });
            }
        });
    };
}]);