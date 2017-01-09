angular.module("plicd.common.services")

.factory("utilities", ["$window", function (window) {

    return {
        isNumber: function (value) {
            return !isNaN(parseFloat(value)) && isFinite(value);
        },
        ensureInt: function (value, defaultValue) {
            var result = parseInt(value);
            return result != NaN ? result : defaultValue;
        },
        extractError: function (results) {
            var error = {};
            error.message = results.data && results.data.Message ? results.data.Message : results.message;
            error.messageDetail = results.data && results.data.MessageDetail ? results.data.MessageDetail : results.messageDetail;
            error.exceptionMessage = results.data && results.data.ExceptionMessage ? results.data.ExceptionMessage : results.exceptionMessage;
            error.exceptionType = results.data && results.data.ExceptionType ? results.data.ExceptionType : results.exceptionType;
            error.stackTrace = results.data && results.data.StackTrace ? results.data.StackTrace : results.stackTrace;
            return error;
        }
    }

}]);

