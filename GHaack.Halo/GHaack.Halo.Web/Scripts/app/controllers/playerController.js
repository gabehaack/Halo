(function () {
    'use strict';

    function PlayerController($http) {
        var vm = this;

        vm.stats = {};
        vm.init = init;

        function init(stats) {
            vm.stats = stats;
        }

        // exports

    }

    angular
        .module('app')
        .controller('PlayerController', PlayerController)
    PlayerController.$inject = ['$http'];
}());