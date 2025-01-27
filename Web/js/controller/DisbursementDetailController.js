﻿define(['app'], function (app) {
    app.controller('DisbursementDetailController', ['$scope', '$rootScope', "$routeParams", 'BaseService', DisbursementDetailController]);

    function DisbursementDetailController($scope, $rootScope, $routeParams, BaseService) {

        $rootScope.changehighlight(5);

        if ($rootScope.disbBackTo == 0) {
            $scope.viewReqBtn = true;
        }
        else {
            $scope.viewReqBtn = false;
        }

        var disid = $routeParams.disid;
        $rootScope.disid = disid;
        var myBaseService = BaseService;
        //console.log($scope.disid);
        BaseService.getDisbursementList("null", "null", disid, "null", "null")
            .then(function (data) {
                //console.log(data);
                $scope.DisbursementData = data[0];
                //console.log($scope.DisbursementData);
                myBaseService.getEmployee($scope.DisbursementData.EmpID)
                       .then(function (Empdata) {
                           ////console.log("getEmployee");
                           $scope.DisbursedByName = Empdata.EmpName;
                       })
                ////console.log("++++++" + $scope.DisbursementData.ReceivedBy);

                if ($scope.DisbursementData.ReceivedBy != null) {
                    myBaseService.getEmployee($scope.DisbursementData.ReceivedBy)
                       .then(function (Empldata) {
                           ////console.log("getEmployee");
                           $scope.ReceivedByName = Empldata.EmpName;
                       })
                } else {
                    $scope.RequisitionData.ReceivedBy = "";
                }
            }, function (data) {
                alert(data);
            }
            )
        BaseService.getDisbursementDetail(disid)
            .then(function (data) {
                //console.log(data);
                $scope.DisbursementDetailLists = data;
                $.each($scope.DisbursementDetailLists, function (index, value) {
                    //console.log(value.ItemID);
                    myBaseService.getItemDetail(value.ItemID)
                        .then(function (data) {
                            value.Description = data.ItemName;
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
            if ($rootScope.disbBackTo == 0) {
                location.href = "#/disbursement";
            }
            else
            {
                location.href = "#/disbursementStoreClerk";
            }
        }
        $scope.viewRequisition = function () {
            location.href = "#/disbursementRequisition/" + disid;
        }
    }
})