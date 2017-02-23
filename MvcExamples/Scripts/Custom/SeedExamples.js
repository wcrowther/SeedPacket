
(function (seedExamples, $, undefined) {

    var self = this;

    seedExamples.init = function () {
        self.$resultsNumber = $('#numberOfRowResults');
        self.$randomSeed = $('#randomSeed');
        self.$results = $('#rowResults');

        $resultsNumber.on('change', function () {
            seedExamples.refreshRowResults();
        })
        $randomSeed.on('change', function () {
            seedExamples.refreshRowResults();
        })
    };

    seedExamples.refreshRowResults = function (evt) {
        $results.load("/Home/GetResultRows/" + $resultsNumber.val() + "?seed=" + $randomSeed.val());
    }

})(window.seedExamples = window.seedExamples || {}, jQuery);


