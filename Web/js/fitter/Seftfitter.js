﻿var appfilter = angular.module('appfilter', []);
appfilter.filter('datechange', datefilter);
appfilter.filter('statuschange', statuschange);

function datefilter() {
    return function (input) {
        return input.substr(6, 13);
    };
}
function statuschange() {
    return function (input) {
        if (input == 1) {
            return "Pending Approval";
        }
        if (input == 2) {
            return "Approved";
        }
        if (input == 3) {
            return "Processed";
        }
        if (input == 4) {
            return "Collected";
        }
        if (input == 5) {
            return "Rejected";
        }
        if (input == 6) {
            return "Cancelled";
        }
    };
}


//function datefilter22() {
//    return function (input, param1) {
//        var args = Array.prototype.slice.call(arguments);
//        console.log("arguments=", args.length);
//        if (3 <= args.length) {
//            console.log("param2(string)=", args[2]);
//        }
//        if (4 <= args.length) {
//            console.log("param3(bool)=", args[3]);
//        }
//        console.log("------------------------------------------------- end dump of custom parameters");
//        // filter  
//        if (5 <= args.length) {
//            return window[args[4]](input);
//        }
//        return input;
//    };
//}