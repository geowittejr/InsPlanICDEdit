angular.module("plicd.common.directives")

// This directive fixes an issue that occurs when the browser is in IE8 mode (or possibly quirks mode)
// and pressing the Backspace key in a text input control does not call the method set by the ng-change directive.

.directive("filterBackspaceFix", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, elem, attrs, controller) {
            if (attrs.type === "radio" || attrs.type === "checkbox") return;

            elem.bind("keydown keyup keypress", function (event) {
                if (event.which === 8) {
                    scope.$apply(function () {
                        controller.$setViewValue(elem.val());
                    });
                }
            });
        }
    };
});