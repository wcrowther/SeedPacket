
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
    mounted: function () {
    this.getFootballInfo();
        this.ActiveTab = getCookie('activeTab', 0);
    },
    components:
    {
        footballIcon:
        {
    template: `<span :class="classObject" :style="styleObject" @@click="showTeam(team)" :title="teamName"></span>`,
            props:
            {
                team: { type: Object, required: true, default: null },
                size: {
                    type: String,
                    default: 'small',
                    validator: (prop) => [
                        'small',
                        'medium',
                        'large',
                    ].includes(prop)
                }
            },
            methods:
            {
                showTeam: function (team) {
                    this.$emit('show-team', team);
                }
            },
            computed:
            {
                classObject: function () {
                    return `football-icon-${ this.size } `;
                },
                teamName: function () {
                    return `${ this.team.Location } ${ this.team.Name } - ${ this.team.Conference } ${ this.team.Division } `
                },
                styleObject: function () {
                    var width = (this.team.DivId - 1) * this.icon.width;
                    var height = (this.team.TeamId - 1) * this.icon.height;

                    width = (this.team.ConfId === 2) ? width + (4 * this.icon.width) : width;

                    return { backgroundPosition: `-${ width }px -${ height }px` };
                },
                icon: function () {
                    if (this.size === 'large')
                        return { width: 120, height: 100 };
                    else if (this.size === 'medium')
                        return { width: 60, height: 50 };
                    else                  // small
                        return { width: 30, height: 25 };
                }
            }
        }
    },
    computed:
    {
        activeTab:
        {
            cache: false,
            get: function () {
                try {
                    var tab = getCookie('activeTab', 0);
                    return tab;
                }
                catch (e) {
                    return 0;
                };
            },
            set: function (newValue) {
                this.ActiveTab = newValue;
                setCookie('activeTab', newValue, this.Settings.CookieDuration);
            }
        },
        yearList: function () {
            var options = [];
            var currentYear = new Date().getFullYear()
            max = currentYear + 10

            for (var year = currentYear - 10; year <= max; year++) {
                var option = { value: year, text: year };
                options.push(option);
            }
            return options;
        },
        gamesByDate: function () {
            let dateList = this.Games.groupBy(g => g.SeasonWeek);
            //console.log(JSON.stringify(dateList));

            return dateList;
        },
        gamesByWeek: function () {
            return this.Games.groupBy(g => g.SeasonWeek);
        }
    },
    methods:
    {
        showTestButtonId: function () {
            alert(event.currentTarget.id);
        },
        jDate: function (date, format) {
            format = typeof format === 'undefined' ? 'h:mm a MMM Do YYYY' : format;

            return jsonDate(date, format);
        },
        setActiveTab: function (tab) {
            this.ActiveTab = tab;
            // console.log('ActiveTab set to: ' + tab);

            setCookie('activeTab', tab, this.Settings.CookieDuration);
        },
        setActiveTeam: function (team) {
            if (typeof team === 'undefined' || team === null)
                this.ActiveTeam = null;
            else
                this.ActiveTeam = team;
        },
        getFootballInfo: function () {
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
