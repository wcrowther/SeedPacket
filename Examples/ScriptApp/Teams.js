// alert('teams.js loaded...');

const vueApp = new Vue({
    el: '#vueContent',
    data: {
    Settings:
        {
    CookieDuration: 7
        },
        ActiveTab: 0,
        Seed: 1234,
        FootballWeeks: [],
        Teams: [],
        ActiveTeam: { },
        Games: [],
        ElapsedTime: '',
        CustomSunday: null,
        Year: 2020,
        YearList: []
    },
    mounted: function ()
    {
        this.getFootballInfo();
        this.ActiveTab = getCookie('activeTab', 0);
    },
    components:
    {
        footballIcon
    },
    computed:
    {
        activeTab:
        {
            cache: false,
            get: function ()
            {
                try {
                    var tab = getCookie('activeTab', 0);
                    return tab;
                }
                catch (e) {
                    return 0;
                };
            },
            set: function (newValue)
            {
                this.ActiveTab = newValue;
                setCookie('activeTab', newValue, this.Settings.CookieDuration);
            }
        },
        yearList: function ()
        {
            var options = [];
            var currentYear = new Date().getFullYear()
            max = currentYear + 10

            for (var year = currentYear - 10; year <= max; year++) {
                var option = { value: year, text: year };
                options.push(option);
            }
            return options;
        },
        gamesByDate: function ()
        {
            let dateList = this.Games.groupBy(g => g.SeasonWeek);
            //console.log(JSON.stringify(dateList));

            return dateList;
        },
        gamesByWeek: function ()
        {
            return this.Games.groupBy(g => g.SeasonWeek);
        }
    },
    methods:
    {
        showTestButtonId: function ()
        {
            alert(event.currentTarget.id);
        },
        jDate: function (date, format)
        {
            format = typeof format === 'undefined' ? 'h:mm a MMM Do YYYY' : format;

            return jsonDate(date, format);
        },
        setActiveTab: function (tab)
        {
            this.ActiveTab = tab;
            //console.log('ActiveTab set to: ' + tab);

            setCookie('activeTab', tab, this.Settings.CookieDuration);
        },
        setActiveTeam: function (team)
        {
            if (typeof team === 'undefined' || team === null)
                this.ActiveTeam = null;
            else
                this.ActiveTeam = team;
        },
        getFootballInfo: function ()
        {
            console.log('GetFootballInfo for: ' + this.Year);
            let self = this;

            $.ajax({
                type: 'POST',
                url: '/Teams/GetFootballInfo',
                data: JSON.stringify({ Seed: this.Seed, Year: this.Year }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'JSON', //'html',
                success: function (result) {
                    self.Teams = result.Teams;
                    self.Games = result.Games;
                    self.OpeningSunday = result.OpeningSunday;
                    self.ElapsedTime = result.ElapsedTime;

                    if (self.Teams)
                        self.ActiveTeam = self.Teams[0];

                    for (var i = 0; i < self.Teams.length; i++) {
                        var team = self.Teams[i];

                        self.Teams[i].games = self.Games.filter(g => g.HomeTeam.Id === team.Id ||
                            g.AwayTeam.Id === team.Id);
                    }
                }
            });
        },
        gamesByTeam: function () {
            for (var i = 0; i < this.Teams.length; i++) {
                var team = Teams[i];

                this.Teams[i].games = this.Games.filter(g => g.HomeTeam.Id === team.Id ||
                    g.AwayTeam.Id === team.Id)
            }
        },
        teamsByConference: function (confId, divId) {
            return this.Teams.filter(g => g.ConfId === confId && g.DivId === divId);
        }
    }
})
