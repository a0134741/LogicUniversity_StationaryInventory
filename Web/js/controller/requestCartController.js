﻿define(['app'], function (app) {
    app.controller('RequestCartControllers', ['$rootScope', '$scope', 'BaseService', RequestCartControllers]);
    app.controller('RequestCartListCtrl', ['$rootScope', '$scope', 'BaseService', RequestCartList]);

    function RequestCartControllers($rootScope, $scope,BaseService) {
        var selfBaseService = BaseService;
        //set mean highlight
        $rootScope.changehighlight(1);
        $('#create-switch').bootstrapSwitch();
        console.log("enter RequestCartControllers")
        $scope.back = function () {
            location.href = '#/requisition';
        }
        $scope.submit = function () {
            console.log("enter");
            var msg = [];
            $.each($rootScope.RequestCarts, function (index, value) {
                var each = {
                    EmpID: $rootScope.UserInfo.EmpId,
                    ItemID: value.ItemID,
                    Qty: value.Qty,

                };
                msg.push(each);
            });
            var req_id="";
            console.log(angular.toJson(msg));
            selfBaseService.createRequisition(angular.toJson(msg))
                .then(function (data) {
                    req_id=data;
                }, function (data) {
                    alert(data);
                })
            var priority = 2;
            if ($scope.PRIORITY) { priority = 1 }
            
            selfBaseService.setReqPriority(req_id, priority, $scope.remoarks)
                .then(function (data) {
                    alert("success!");
                    location.href = '#/requisition';
                })
        }
    }
    function RequestCartList($rootScope, $scope, BaseService) {
        //get EmpId from session
        var EmpId = $rootScope.UserInfo.EmpId;
        var selfBaseService = BaseService;
        console.log("enter requestCartListCtrls");
        BaseService.getRequestCart(EmpId)
            .then(function (data) {
                console.log(data);
                $rootScope.RequestCarts = data;
            }, function (data) {
                alert(data);
            })
        $scope.delect = function (RequestCart) {
            var msg = '{"ItemID":"'+RequestCart.ItemID+'" ,"EmpID":'+EmpId+'}';
            console.log(msg);
            BaseService.removeRequestCart(msg)
                    .then(function (data) {
                        selfBaseService.getRequestCart(EmpId)
                        .then(function (data) {
                            console.log(data);
                            $rootScope.RequestCarts = data;
                            if (!$scope.$$phase) {
                                $rootScope.$apply();
                            }
                        }, function (data) {
                            alert(data);
                        })
                    }, function (data) {

                    })
        }
    }
})
