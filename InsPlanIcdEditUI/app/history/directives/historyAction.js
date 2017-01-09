angular.module("plicd.history.directives")

//This directive outputs the history action html from a history item's properties.

.directive("historyAction", function ($timeout, $q, $compile) {

    //Define the various html templates for our action types
    var planTemplate = "<a href='#/plans/{{item.insPlanId}}' class='text-link-bold'>{{item.insPlanId}}</a> {{getAction()}}";
    var icd9Template = "<a href='#/icd9s/{{item.icd9 | replacePeriods}}' class='text-link-bold'>{{item.icd9}}</a> {{getAction()}}";
    var planIcd9Template = "<a href='#/plans/{{item.insPlanId}}' class='text-link-bold'>{{item.insPlanId}}</a> and <a href='#/icd9s/{{item.icd9 | replacePeriods}}' class='text-link-bold'>{{item.icd9}}</a> {{getAction()}}";

    return {
        restrict: "E",
        scope: {
            item: "=",
            entityType: "=",
            entityId: "="
        },
        link: function (scope, element, attrs) {

            //Add function to the scope so our templates can use it to get the correct action text.            
            scope.getAction = function () {
                var actionType = scope.item.actionType;
                return actionType == 1 ? "added" : actionType == 2 ? "removed" : "updated";
            };

            //Add the appropriate template html to our element
            var isAll = scope.entityId == null && scope.entityType == null;            
            var type = scope.entityType && scope.entityType !== null ? scope.entityType.toLowerCase() : "";
            var template = isAll ? planIcd9Template : type == "insplanid" ? icd9Template : type == "icd9" ? planTemplate : planIcd9Template;
            element.html(template);

            //Compile the template against the scope values
            $compile(element.contents())(scope);
        }
    };
});