
(function (Teams, $, undefined) {

    var self = this;

    Teams.init = function ()
    {
        self.$firstSunday = $('#firstSunday');
        self.$randomSeed = $('#randomSeed');
        self.$results = $('#games');

        $firstSunday.on('change', function () {
            Teams.getRefreshedGames();
        })
        $randomSeed.on('change', function () {
            Teams.getRefreshedGames();
        })
    };

    Teams.getRefreshedGames = function (evt)
    {
        var seed = $randomSeed.val() || 1234;
        var firstSunday;
        $results.load("/Teams/GetRefreshedGames/" + seed  );
    }

})(window.Teams = window.Teams || {}, jQuery);


