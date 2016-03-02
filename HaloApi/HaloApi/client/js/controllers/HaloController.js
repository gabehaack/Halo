(function () {
    var app = angular.module("gabeHalo", []);
    
    var haloController = function ($scope, $http) {
        
        var haloApiGetRequest = function (url) {
            return {
                method: 'GET',
                url: url,
                headers: {
                    'Content-Type' : 'text/plain',
                    'Ocp-Apim-Subscription-Key' : 'ef273017b79a4cbeb8646d1de7d1f598'
                }
            }
        }
        
        var createResult = function (k, d, a) {
            var kd = k / d;
            var kda = k + a / 3 - d;
            return {
                k: k,
                d: d,
                a: a,
                kd: kd,
                kda: kda
            };
        }
        
        var onHttpError = function (reason) {
            $scope.error = "Error: " + reason;
        };
        
        // Get Service Record
        var onGetServiceRecord = function(response) {
            //$scope.apiData = response.data;
        };
        
        var getServiceRecord = function (gamertag) {
            var url = "https://www.haloapi.com/stats/h5/servicerecords/arena?players=" + gamertag;
            var request = haloApiGetRequest(url);
            $http(request)
                .then(onGetServiceRecord, onHttpError);
        }
        
        // Get Games
        var onGetGames = function (response) {
            var results = response.Results.Players[0];
            $scope.result = createResult(results.TotalKills, results.TotalDeaths, results.TotalAssists);
        };
        
        $scope.getGames = function (gamertag) {
            var url = "https://www.haloapi.com/stats/h5/players/" + gamertag + "/matches?modes=arena&count=1";
            var request = haloApiGetRequest(url);
            $http(request)
                .then(onGetGames, onHttpError);
        }
        
        $scope.footerImageSource = "http://mammothgamers.com/wp-content/uploads/2015/10/6987008-art-halo.jpg"
    };
    
    app.controller("haloController", haloController);
}());