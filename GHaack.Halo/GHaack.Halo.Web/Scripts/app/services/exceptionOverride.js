(function () {
    'use strict';

    angular.module('ghaack.haloApp').factory("$exceptionHandler",
        ['$log', 'alerts', function ($log, alerts) {
            return function (exception, cause) {
                alerts.error("Something went wrong in Angular.");
                $log.error(exception, cause);
            };
        }]);
})();