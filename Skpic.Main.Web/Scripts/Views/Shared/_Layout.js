
var loginModule = angular.module("LoginModule", []);
loginModule.controller("LoginControler", ["$scope", "$http", function ($scope, $http) {
    $scope.loginModel = {
        loginName: "",
        loginPwd: ""
    };
    $scope.login = function () {
        $http({
            method: "post",
            data: {
                login: $scope.loginModel
            },
            url: "/Account/Login"
        }).success(function (data, status, headers, config) {
            alert(data);
        }).error(function (data, status, headers, config) {

        });
    };
    $scope.reset = function () {
        $scope.loginModel = {
            loginName: "",
            loginPwd: ""
        };
    }
}]);