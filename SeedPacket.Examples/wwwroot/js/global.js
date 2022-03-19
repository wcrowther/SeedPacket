

function jsonDate(jDate) {
    return (jDate !== null) ? moment(jDate).format("MMMM Do YYYY, h:mm:ss") : "";
}

// alert('global.js');

function jsonDate(jDate, format)
{
    return (jDate !== null) ? moment(jDate).format(format) : "";
}

function dateOnly(jDate)
{
    return (jDate !== null) ? moment(jDate).format('MM/DD/YYYY') : "";
}

function setCookie(key, value, days)
{
    //console.log('setCookie ' + key + ' to ' + value);

    var expires = new Date();
    expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
    document.cookie = key + '=' + value + ';expires=' + expires.toUTCString();
}

function getCookie(name, _default)
{
    _default = typeof _default === 'undefined' ? null : _default;

    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++)
    {
        var c = ca[i];
        while (c.charAt(0) == ' ')
            c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0)
        {
            var val = c.substring(nameEQ.length, c.length);
            return val;
        }
    }
    return _default;
}


//function getCookie(key, _default)
//{
//    _default = typeof _default === 'undefined' ? null : _default;
//    var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');

//    console.log( key = ' : ' + keyValue + ' : ' + (keyValue ? keyValue[2] : '') );

//    return keyValue ? keyValue[2] : _default;
//}

function getCookieBoolean(name)
{
    var cookie = getCookie(name);
    var defaultValue = (cookie !== null && cookie === 'false') ? false : true;
    // console.log(name + ' : ' + defaultValue);

    return defaultValue;
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
