var app = angular.module('HomeApp', []);

app.controller('LeftProducts',
    ($scope, $http) => {

        $scope.Products = "";

        var GetData = () => {
            $http.get("/Data/GetLeftProducts").then((data) => {
                $scope.Products = data.data;
            });
        }
        GetData();

    });

app.controller('NewProducts',
    ($scope, $http) => {

        $scope.Products1 = [];
        $scope.Products2 = [];

        var GetData = () => {
            $http.get("/Data/GetNewProducts").then((data) => {
                for (var i = 0; i < data.data.length; i++) {
                    if (i < 4) {
                        $scope.Products1.push(data.data[i]);
                    } else if(i >= 4)
                        $scope.Products2.push(data.data[i]);
                }
            });
        }
        GetData();

    });

app.controller('RandomProducts',
    ($scope, $http) => {

        $scope.Products = [];

        var GetData = () => {
            $http.get("/Data/GetRandomProducts").then((data) => {
                $scope.Products = data.data;
            });
        }
        GetData();

    });