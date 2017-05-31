
(function (Examples, $, undefined) {

    var self = this;

    Examples.init = function () {
        self.$resultsNumber = $('#numberOfRowResults');
        self.$randomSeed = $('#randomSeed');
        self.$results = $('#rowResults');

        $resultsNumber.on('change', function () {
            Examples.refreshRowResults();
        })
        $randomSeed.on('change', function () {
            Examples.refreshRowResults();
        })
    };

    Examples.refreshRowResults = function (evt) {
        var seed = $randomSeed.val() || 1234;
        $results.load("/Ajax/GetResultRows/" + $resultsNumber.val() + "?seed=" + seed );
    }

})(window.Examples = window.Examples || {}, jQuery);


