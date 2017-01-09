angular.module("plicd.common.directives")

//This directive allows us to set up infinite scroll on the element passed in.

.directive("infiniteScroll", ["$timeout", "$window", function ($timeout, $window) {

    return {
        restrict: 'A',
        scope: {
            loadItems: "&",
            wasLoaded: "=",
            loadError: "=",
            minItems: "=",
            maxItems: "=",
            itemCount: "=",            
            scrollBuffer: "=",
            wasResized: "=",
            isLoading: "=",
            isLoadingMore: "="
        },
        link: function (scope, element, attrs) {

            var scrollBuffer = scope.scrollBuffer ? scope.scrollBuffer : 50;
            var minItems = scope.minItems ? scope.minItems : 20;
            var container = angular.element(element[0]);           
            var content = container.find("tbody:first");
            
            scope.scrolled = false;

            //Load upon scrolling if necessary
            element.on("scroll", function (e) {
                loadMoreItems();
            });

            //Load more items if the window was resized
            scope.$watch("wasResized", function (expression) {
                if (expression) {
                    loadMoreItems();
                }
            });            
                       
            //Items were just loaded, so check and see if we should keep going.
            scope.$watch("wasLoaded", function (expression) {               
                if (expression) {
                    loadMoreItems();
                }
            });

            //Handler for loading errors
            scope.$watch("loadError", function (expression) {
                if (expression) {
                    handleLoadError();
                }
            });

            //Check and see if we should load more items and then do it.
            var loadMoreItems = function () {
                if (loadingIsNeeded()) {
                    if (scope.loadItems) {                        
                        scope.loadItems();
                    }
                }
            };

            //Handle errors in loading
            var handleLoadError = function () {
                scope.isLoading = false;
                scope.isLoadingMore = false;
            };
            
            //Do we need to load more items?
            var loadingIsNeeded = function () {                 

                //Check if we need to load more items based on how many are loaded and how many total items exist
                if (scope.itemCount !== undefined && scope.maxItems !== undefined) {

                    //We loaded all the items, so no more loading is needed
                    if (scope.itemCount == scope.maxItems) {
                        scope.isLoading = false;
                        scope.isLoadingMore = false;
                        return false;
                    }

                    //We loaded less than the total items and didn't go over the minimum items yet, so more loading is needed
                    if (scope.itemCount < scope.maxItems && scope.itemCount <= minItems) {
                        container.scrollTop(0); //send user back to top of scroll content
                        scope.isLoading = true;
                        return true;
                    }
                }

                //Check scrolling
                var containerBottom = container.position().top + container.height();
                var contentBottom = content.position().top + content.height();                
                if (contentBottom - scrollBuffer <= containerBottom) {

                    //User scrolled close to end of content, so more loading is needed.

                    //We need to wrap the following update in a scope.$apply() call
                    //to make angularjs aware of the change.  If we don't, then
                    //a digest cycle will not kick off until something else triggers it.
                    //I believe this is because the window scroll ".on" jquery listener triggers
                    //the change outside of the angular environment, so angular doesn't 
                    //automatically kick off a digest.
                    scope.$apply(function () {
                        scope.isLoadingMore = true;
                    });
                    return true;
                }

                scope.isLoading = false;
                scope.isLoadingMore = false;
                return false;
            }
        }
    };

}]);