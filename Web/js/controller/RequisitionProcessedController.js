﻿define(['app'], function (app) {
    app.controller('ReqProcController', ['$scope', '$rootScope', "$routeParams", 'BaseService', ReqProcController]);

    function ReqProcController($scope, $rootScope, $routeParams, BaseService) {
        $rootScope.changehighlight(8);
        var retid = $routeParams.retid;
        $scope.retid = retid;
        var myBaseService = BaseService;
        console.log($scope.retid);

        //BaseService.getRetrievalListBySC("null", "null", $scope.retid)
        //    .then(function (data) {
        //        console.log(data);
        //        $scope.RetrievalData = data[0];
                
        //    }, function (data) {
        //        alert(data);
        //    })

        BaseService.getRequisitionListByRetID($scope.retid)
            .then(function (data) {
                console.log(data);
                $scope.RequisitionData = data;
                $scope.reqForms = [];
                $.each($scope.RequisitionData, function (index, value) {
                    var each = value.ReqID + ", ";
                    msg.push(each);
                })
            }, function (data) {
                alert(data);
            })

        BaseService.getRetrievalDetail(retid)
            .then(function (data) {
                console.log(data);
                $scope.RetrievalDetails = data;
                $.each($scope.RetrievalDetails, function (index, value) {
                    console.log(value.ItemID);
                    myBaseService.getItemDetail(value.ItemID)
                        .then(function (data) {
                            value.Bin = data.Bin;
                            value.ItemName = data.ItemName;
                        }, function (data) {
                            alert(data);
                        }
                        )
                });
            }, function (data) {
                alert(data);
            }
            )
        $scope.back = function () {
            location.href = "#/requisitionStoreClerk";
        }
    }
})