
(function (Teams, $, undefined) {

    var self = this;

    Teams.init = function ()
    {
        self.$openingSunday = $('#openingSunday');
        self.$randomSeed = $('#randomSeed');
        self.$results = $('#games');

        $openingSunday.on('change', function () {
            Teams.getRefreshedGames();
        })
        $randomSeed.on('change', function () {
            Teams.getRefreshedGames();
        })
    };

    Teams.getRefreshedGames = function (evt)
    {
        var seed = $randomSeed.val() || 1234;
        var openingSunday = $openingSunday.val(); //.replace(/\s/g, '_').replace(/\//g, '-');

        $.ajax({
            type: 'POST',
            url: '/Teams/GetRefreshedGames',
            data: JSON.stringify({ seed: seed, openingSunday: openingSunday }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            success: function (result)
            {
                $results.html(result);
            }
        });

       // OLD: $results.load("/Teams/GetRefreshedGames/" + seed + "/" + openingSunday);
    }

})(window.Teams = window.Teams || {}, jQuery);


