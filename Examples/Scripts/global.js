

function jsonDate(jDate) {
    return (jDate !== null) ? moment(jDate).format("MMMM Do YYYY, h:mm:ss") : "";
}

function jsonDate(jDate,format) {
    return (jDate !== null) ? moment(jDate).format(format) : "";
}

function dateOnly(jDate) {
    return (jDate !== null) ? moment(jDate).format('MM/DD/YYYY') : "";
}
