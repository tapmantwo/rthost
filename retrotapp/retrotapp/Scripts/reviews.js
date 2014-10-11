angular.module("reviews", ["ngResource"])
    .controller("reviewController", [
        "$scope", "reviewService", function ($scope, reviewService) {
        $scope.reviews = reviewService.getReviews();

            //    [
            //{
            //    name: "C64",
            //    thumbnailUrl: "/reviews/C64.png",
            //    folders: [],
            //    items: [
            //        {
            //            name: "Manic Miner",
            //            url: "/reviews/C64/manic miner.html",
            //            thumbnailUrl: "/reviews/C64/manic miner.png",
            //            lastModifiedDate: "11/10/2014"
            //        },
            //        {
            //            name: "Short Circuit",
            //            url: "/reviews/C64/short circuit.html",
            //            thumbnailUrl: "/reviews/C64/short circuit.png",
            //            lastModifiedDate: "11/10/2014"
            //        }
            //    ]
            //},
            //{
            //    name: "Atari 2600",
            //    thumbnailUrl: "/reviews/atari 2600.png",
            //    folders: [],
            //    items: [
            //        {
            //            name: "Crystal Castles",
            //            url: "/reviews/atari 2600/crystal castles.html",
            //            thumbnailUrl: "/reviews/atari 2600/crystal castles.png",
            //            lastModifiedDate: "11/10/2014"
            //        },
            //        {
            //            name: "Yars Revenge",
            //            url: "/reviews/atari 2600/yars revenge.html",
            //            thumbnailUrl: "/reviews/atari 2600/yars revenge.png",
            //            lastModifiedDate: "11/10/2014"
            //        }
            //    ]
            //}
        //];
        }])
    .service("reviewService", ["$resource", function($resource) {
       return {
           getReviews: function() {
               var reviewResource = $resource("/api/content/reviews");
               return reviewResource.get();
           }
       } 
    }]);