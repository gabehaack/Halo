(function () {
    'use strict';

    window.onerror = function (msg) {
        if (window.alerts) {
            window.alerts.error("Something went wrong.");
        } else {
            alert("Something went really wrong.")
        }
    };

    var id = 'ghaack.haloApp';
    var haloApp = angular.module(id, []);
    haloApp.run([
        function () {

        }
    ]);
})();