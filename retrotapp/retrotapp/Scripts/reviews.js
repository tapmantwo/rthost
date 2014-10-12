angular.module("reviews", ["ngResource"])
    .controller("reviewController", [
        "$scope", "reviewService", function ($scope, reviewService) {
        $scope.reviews = reviewService.getReviews();
        }])
    .service("reviewService", ["$resource", function($resource) {
       return {
           getReviews: function() {
               var reviewResource = $resource("/api/content/reviews");
               return reviewResource.get();
           }
       } 
    }]);