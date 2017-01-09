angular.module("plicd.common.directives")

//This directive shows a status alert on the specified element whenever the statusAlert.show value is changed to true

.directive("statusAlert", ["$timeout", function ($timeout) {

    return {
        restrict: 'A',
        scope: {
            //statusAlert is a javascript object that contains the following properties:
            //  show:       true/false to determine when to show the alert
            //  updating:   true/false lets our UI know that the item is updating currently
            //  cssClass:   the class name to add for the alert
            //  text:       the text to display in the alert

            statusAlert: "="
        },
        link: function (scope, element, attrs) {

            //Create the status alert div and hide it initially
            element.append("<div class='status-alert'></div>")
            var alertDiv = element.children("div.status-alert");
            alertDiv.hide();

            //Watch for a change in cssClass, which lets us know that the alert is ready to display            
            scope.$watch("statusAlert.show", function (show) {
                if (show) { showStatusAlert(); }
            });

            //Process the new status
            var showStatusAlert = function () {

                //Update the alert text and css
                alertDiv.html(scope.statusAlert.text).attr("class", "status-alert " + scope.statusAlert.cssClass);

                //Fade in and out, then reset the status
                alertDiv.fadeIn(500).delay(0).fadeOut(1500, function () {
                    //reset the status alert after showing it
                    scope.statusAlert.cssClass = "";
                    scope.statusAlert.text = "";
                    scope.statusAlert.updating = false;
                    scope.statusAlert.show = false;
                });
            };
        }
    };

}]);