

function jsonDate(jDate) {
    return (jDate !== null) ? moment(jDate).format("MMMM Do YYYY, h:mm:ss") : "";
}

//alert('global.js');

function jsonDate(jDate, format) {
    return (jDate !== null) ? moment(jDate).format(format) : "";
}

function dateOnly(jDate) {
    return (jDate !== null) ? moment(jDate).format('MM/DD/YYYY') : "";
}


Array.prototype.groupBy = function (keyFunction)
{
    var groups = {};
    this.forEach(function (el) {
        var key = keyFunction(el);
        if (key in groups == false) {
            groups[key] = [];
        }
        groups[key].push(el);
    });
    return Object.keys(groups).map(function (key) {
        return {
            key: key,
            group: groups[key]
        };
    });
};
