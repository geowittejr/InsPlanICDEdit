angular.module("plicd.common.filters")

.filter("shortDateTime", ["$filter", function ($filter) {
    return function (inputDate) {
        return $filter("date")(inputDate, "MM/dd/yyyy hh:mm:ss a")
    };
}])

.filter("mediumDateTime", ["$filter", function ($filter) {
    return function (inputDate) {
        return $filter("date")(inputDate, "MMM dd, yyyy hh:mm:ss a")
    };
}])

.filter("longDateTime", ["$filter", function ($filter) {
    return function (inputDate) {
        return $filter("date")(inputDate, "EEE, MMM dd, yyyy hh:mm:ss a")
    };
}]);