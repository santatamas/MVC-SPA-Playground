// create our main angular app
var mainApp = angular.module('mainApp', ['ngRoute', 'chieffancypants.loadingBar', 'nvd3', 'xeditable']);

// configure our routes
mainApp.config(function ($routeProvider, cfpLoadingBarProvider) {
    $routeProvider

        // route for the home page
        .when('/', {
            templateUrl: 'Content/Templates/home.html',
            controller: 'mainController'
        })

        // route for the about page
        .when('/charts', {
            templateUrl: 'Content/Templates/charts.html',
            controller: 'chartsController'
        })

        // route for the about page
        .when('/editable', {
            templateUrl: 'Content/Templates/editable.html',
            controller: 'editableController'
        });

    cfpLoadingBarProvider.includeSpinner = true;
});

// create the controller and inject Angular's $scope
mainApp.controller('mainController', function ($scope, $http) {
    // create a message to display in our view
    $scope.message = 'List of sample polling Data:';

    $http.get('http://pollprototypeapi.azurewebsites.net/api/PollSampleEntities').success(function(data) {
        $scope.pollSamples = data;
    });


});

mainApp.controller('chartsController', function ($scope, $http) {
    $scope.message = 'Average ages by first names (LinqJS).';

    $http.get('http://pollprototypeapi.azurewebsites.net/api/PollSampleEntities').success(function(data) {

        var aggregatedSamples = Enumerable.From(data)
            .GroupBy("$.Name.split(' ')[0]", null, function(key, g) {
                var result = {
                    name: key,
                    averageAge: g.Sum("$.Age") / g.Count()
                }
                return result;
            }).Select().ToArray();

        $scope.pollSamples = aggregatedSamples;
        $scope.bardata = [
            {
                key: "Average ages by first names (LinqJS)",
                values: aggregatedSamples
            }
        ];
    });

    $scope.baroptions = {
        chart: {
            type: 'discreteBarChart',
            height: 450,
            margin: {
                top: 20,
                right: 20,
                bottom: 60,
                left: 55
            },
            x: function (d) { return d.name; },
            y: function (d) { return d.averageAge; },
            showValues: true,
            valueFormat: function (d) {
                return d3.format(',.0f')(d);
            },
            transitionDuration: 500,
            xAxis: {
                axisLabel: 'First Names'
            },
            yAxis: {
                axisLabel: 'Average Age',
                axisLabelDistance: 30
            }
        }
    };
});

// create the controller and inject Angular's $scope
mainApp.controller('editableController', function ($scope, $http) {
    // create a message to display in our view
    $scope.message = 'Editable entities (xEditable)';

    $http.get('http://pollprototypeapi.azurewebsites.net/api/PollSampleEntities').success(function (data) {
        $scope.pollSamples = data;
    });

    $scope.checkName = function (data, id) {
        if (id === 2 && data !== 'awesome') {
            return "Username 2 should be `awesome`";
        }
    };

    $scope.saveUser = function (data, sample) {
        //$scope.user not updated yet
        //angular.extend(data, { id: id });
        sample.Name = data.name;
        sample.Age = data.age;
        sample.CountryId = data.countryId;

        return $http.put('http://pollprototypeapi.azurewebsites.net/api/PollSampleEntities/' + sample.PollSampleEntityId, sample);
    };

    // remove user
    $scope.removeUser = function (sampleId, index) {
        $scope.pollSamples.splice(index, 1);
        $http.delete('http://pollprototypeapi.azurewebsites.net/api/PollSampleEntities/' + sampleId);
    };

    // add user
    $scope.addUser = function () {
        $scope.inserted = {
            Name: '',
            Age: 0,
            CountryId: '--',
            TrueFalseAnswer1: true,
            TrueFalseAnswer2: true,
            NumericAnswer1: 0,
            NumericAnswer2: 0
        };
        $scope.users.push($scope.inserted);
        $http.post('http://pollprototypeapi.azurewebsites.net/api/PollSampleEntities/', $scope.inserted);
    };
});
