
var footballIcon = Vue.extend(
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
            classObject: function ()
            {
                return `football-icon-${this.size} `;
            },
            teamName: function ()
            {
                var name = `${this.team.Location} ${this.team.Name} - ${this.team.Conference} ${this.team.Division} `

                return name;
            },
            styleObject: function () {
                var width = (this.team.DivId - 1) * this.icon.width;
                var height = (this.team.TeamId - 1) * this.icon.height;

                width = (this.team.ConfId === 2) ? width + (4 * this.icon.width) : width;

                return { backgroundPosition: `-${width}px -${height}px` };
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
    })
