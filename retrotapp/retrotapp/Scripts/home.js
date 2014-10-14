angular.module("home", ["ngResource"])
    .controller("homeController", [
        "$scope", "reviewService", function ($scope, reviewService) {
        $scope.recentItems = reviewService.getRecentReviews(3);
        }])
    .service("reviewService", ["$resource", function($resource) {
       return {
           getRecentReviews: function (howMany) {
               var reviewResource = $resource("/api/content/getrecentitems/:amount/reviews", {amount: howMany});
               return reviewResource.query();
           }
       } 
    }]);