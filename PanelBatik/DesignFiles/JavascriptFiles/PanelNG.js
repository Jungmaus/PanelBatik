var app = angular.module('PanelApp', []);


app.controller('GetSiparisTable', ($scope, $http) => {

    function GetData() {
        $http.get("/Data/GetSiparisTableData").then((data) => {
            $scope.Siparisler = data.data;
        });
    }
    GetData();

    $scope.SiparisOnayla = (id) => {
        $http.post("/Operation/SiparisOnayla/" + id).then(() => {
            GetData();
        });
    };

    $scope.SiparisKargola = (id) => {
        $http.post("/Operation/SiparisKargola/" + id).then(() => {
            GetData();
        });
    };

    $scope.SiparisIptal = (id) => {
        $http.post("/Operation/SiparisIptal/" + id).then(() => {
            GetData();
        });
    };

});

app.controller('GetUrunListesiData',
    ($scope, $http) => {      

        function GetData() {
            $http.get("/Data/GetUrunListesiData/").then((data) => {
                $scope.Urunler = data.data;
            });
        }

        GetData();

    });

app.controller('SiparisListesiController',
    ($scope, $http) => {

       function GetAllSiparisData(){
            $http.get("/Data/GetSiparisListesiData/").then((data) => {
                $scope.TumSiparisler = data.data;
            });
        }
       GetAllSiparisData();
    });

app.controller('KategoriListesiController',($scope, $http) => {

        function GetKategoriList() {
            $http.get("/Data/GetKategoriList/").then((data) => {
                $scope.Kategoriler = data.data;
            });
        }

    $scope.KategoriSil = ((id) => {

        if (confirm(
            "Bu kategoriyi silerseniz, bu kategori ile ilgili olan tüm ürünler ve siparişler silinecektir. Onaylıyor musunuz ?")) {

            $http.post("/Operation/KategoriSil/" + id).then(() => {
                GetKategoriList();
                alert("Kategori ve ilgili ürünler başarıyla silinmiştir.");
            });

        }
        });

        GetKategoriList();

});