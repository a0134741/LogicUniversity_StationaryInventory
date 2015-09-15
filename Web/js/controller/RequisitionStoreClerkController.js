﻿define(['app'], function (app) {
    app.controller('RequisitionSCControllers', ['$rootScope', '$scope', 'BaseService', RequisitionSCControllers]);

    function RequisitionSCControllers($rootScope, $scope, BaseService) {
        //sidebar highlight
        $rootScope.changehighlight(8);

        var myBaseService = BaseService;
        $scope.search = function () {
            var status = $scope.statusSelect.selectedOption.id;
            var ReqID = $scope.RequisitionNo;
            if (ReqID == null || ReqID == "") { ReqID = "null"; }
            //console.log(ReqID);
            BaseService.getRequisitionList(status, ReqID, "null", "null")
            .then(function (data) {
                console.log(data);
                $scope.Requisitions = data;
                $.each($scope.Requisitions, function (index, value) {
                    console.log(value.EmpID);
                    myBaseService.getEmployee(value.EmpID)
                        .then(function (empdata) {
                            value.EmpName = empdata.EmpName;
                        }, function (data) {
                            alert(data);
                        })
                })
            }, function (data) {
                alert(data);
            })
        }

        //requisition list
        BaseService.getRequisitionApprovedList()
                .then(function (data) {
                    console.log(data);
                    $scope.Requisitions = data;
                    $.each($scope.Requisitions, function (index, value) {
                        console.log(value.EmpID);
                        myBaseService.getEmployee(value.EmpID)
                            .then(function (empdata) {
                                value.EmpName = empdata.EmpName;
                            }, function (data) {
                                alert(data);
                            })
                    })

                }, function (data) {
                    alert(data);
                }
                )

        //status combobox
        $scope.statusSelect = {
            availableOptions: [{ id: '2', name: 'Approved' },
                                { id: '3', name: 'Processed' },
                                { id: '4', name: 'Collected' }],
            selectedOption: { id: '2', name: 'Approved' }
        };

    }
});