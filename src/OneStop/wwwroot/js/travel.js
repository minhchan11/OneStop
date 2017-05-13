function Travel() {
    this.place = "";
}

Travel.prototype.getInfo = function () {
    $.ajax({
        url: "Home/Wiki",
        type: 'POST',
        dataType: "json",
        data: { place: this.place },
        success: function (result) {
            var markup = result.parse.text["*"];
            var i = $('<div></div>').html(markup);
            i.find('a').each(function () { $(this).replaceWith($(this).html()); });
            i.find('sup').remove();
            i.find('.mw-ext-cite-error').remove();
            $('#info').html($(i).find('p').has('b'));
            $('#info-picture').html($(i).find('img').first());
        }
    });
};

Travel.prototype.getCoordinate = function () {
    var position = [];
    $.ajax({
        url: "Home/Coord",
        type: 'POST',
        dataType: "json",
        data: { place: this.place },
        success: function (response) {
            console.log(response);
            position.push(response.Response.View[0].Result[0].Location.DisplayPosition.Latitude);
            position.push(response.Response.View[0].Result[0].Location.DisplayPosition.Longitude);
            position.push((response.Response.View[0].Result[0].Location.Address.Country).toLowerCase());
            console.log(response);
        }
    }).then(function () {
        console.log(position);
    });
    return position;
};

exports.travelObject = Travel;