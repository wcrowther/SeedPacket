
(function (seedExamples, $, undefined) {

    var self = this;

    seedExamples.init = function ()
    {
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

    seedExamples.refreshRowResults = function (evt)
    {
		var seed = $randomSeed.val() || 1234;
		$results.load("/Data/GetResultRows/?rows=" + $resultsNumber.val() + "&seed=" + seed);

		console.log(`refreshRowResults ${$resultsNumber.val()} seed: ${ seed }`);
    }

})(window.seedExamples = window.seedExamples || {}, jQuery);


