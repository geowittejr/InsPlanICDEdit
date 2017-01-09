angular.module("plicd.common.directives")

//This directive allows us to resize an element so that its height doesn't cause the element to expand past the end of the "resizeParent" element.
//We use it mainly to keep the table data expanded to fill the remaining screen height, but not past the main parent div container.
//The element is resized upon window.resize and also whenever the passed in "resizeElementIf" expression changes to true.

.directive("dynamicResize", ["$timeout", "$window", function ($timeout, $window) {
    return {
        restrict: "A",
        scope: {
            resizeWhen: "=",
            onWindowResize: "&"
        },
        link: function (scope, element, attrs) {

            var resizeParent = attrs.resizeParent;
            var resizeBottomMargin = attrs.resizeBottomMargin;

            //Load upon window resize if necessary
            //Alert the scope that window resized because other directives may need this
            //and for some reason it seems that the window.onresize event can only be bound to once.
            $window.onresize = function () {
                resizeElement();
                if (scope.onWindowResize) {
                    scope.$apply(function () {
                        scope.onWindowResize();
                    });
                }
            };

            //Load upon an expression eval
            scope.$watch("resizeWhen", function (expression) {
                if (expression) {
                    $timeout(function () {
                        resizeElement();
                    });
                }
            });

            //Resize the element
            var resizeElement = function () {
                var parent = angular.element("#" + resizeParent);
                var margin = parseInt(resizeBottomMargin);
                var bottomLine = parent.position().top + parent.outerHeight() - margin; //element should not go past this line
                var newElHeight = bottomLine - element.position().top;
                element.height(newElHeight);
            };
        }
    };

}]);