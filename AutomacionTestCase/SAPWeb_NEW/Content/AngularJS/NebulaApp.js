var app = angular.module('SAPApp', ['ui.router', 'ui.bootstrap', 'ngAnimate', 'isteven-multi-select', 'angular-popups']);

app.config(function ($stateProvider, $urlRouterProvider, $httpProvider) {
    //$httpProvider.defaults.useXDomain = true;
    //$httpProvider.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
    //$httpProvider.defaults.withCredentials = false;
    //delete $httpProvider.defaults.headers.common['X-Requested-With'];

    // Use x-www-form-urlencoded Content-Type
    $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';

    // Override $http service's default transformRequest
    $httpProvider.defaults.transformRequest = [function (data) {
        /**
         * The workhorse; converts an object to x-www-form-urlencoded serialization.
         * @param {Object} obj
         * @return {String}
         */
        var param = function (obj) {
            var query = '';
            var name, value, fullSubName, subName, subValue, innerObj, i;

            for (name in obj) {
                value = obj[name];

                if (value instanceof Array) {
                    for (i = 0; i < value.length; ++i) {
                        subValue = value[i];
                        fullSubName = name + '[' + i + ']';
                        innerObj = {};
                        innerObj[fullSubName] = subValue;
                        query += param(innerObj) + '&';
                    }
                }
                else if (value instanceof Object) {
                    for (subName in value) {
                        subValue = value[subName];
                        fullSubName = name + '[' + subName + ']';
                        innerObj = {};
                        innerObj[fullSubName] = subValue;
                        query += param(innerObj) + '&';
                    }
                }
                else if (value !== undefined && value !== null) {
                    query += encodeURIComponent(name) + '=' + encodeURIComponent(value) + '&';
                }
            }

            return query.length ? query.substr(0, query.length - 1) : query;
        };

        return angular.isObject(data) && String(data) !== '[object File]' ? param(data) : data;
    }];

    $urlRouterProvider.when("", "/web");

    $stateProvider
        .state('web', {
            url: "/web",
            templateUrl: "Content/AutomationPage/WebAutomation.html"
        })
        .state('client', {
            url: "/client",
            templateUrl: "Content/AutomationPage/ClientAutomation.html"
        })
        .state('android', {
            url: "/android",
            templateUrl: "Content/AutomationPage/AndroidAutomation.html"
        })
        .state('ios', {
            url: "/ios",
            templateUrl: "Content/AutomationPage/IOSAutomation.html"
        });
});

app.directive('onFinishRenderFilters', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            if (scope.$last === true) {
                $timeout(function () {
                    scope.$emit('ngRepeatFinished');
                });
            }
        }
    };
});


