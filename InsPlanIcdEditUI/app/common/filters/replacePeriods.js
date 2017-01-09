angular.module("plicd.common.filters")

.filter("replacePeriods", function () {
    return function (input) {
        return input.replace(".", "-");
    };
});