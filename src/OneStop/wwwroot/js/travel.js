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
            position.push(response.Response.View[0].Result[0].Location.DisplayPosition.Latitude);
            position.push(response.Response.View[0].Result[0].Location.DisplayPosition.Longitude);
            position.push((response.Response.View[0].Result[0].Location.Address.Country).toLowerCase());
        }
    }).then(function () {
        //console.log(position);
    });
    return position;
};

Travel.prototype.getRestaurants= function () {
    $.ajax({
        url: "Home/Restaurants",
        type: 'POST',
        dataType: "json",
        data: { place: this.place },
        success: function (response) {
            var restaurants = response['businesses'];
            restaurants.forEach(function (item) {
                $("#restaurant").append("<div class='col-md-1 newRes'>" + "<img class='pics' src=" + item.image_url + ">" + "<br>" + "<a href=" + item.url + ">" + item.name + "</a>" + "<br>" + "<p>" + item.rating + "&#9733" + "</p>" + "</div>");
            });   
        }
    });
};

Travel.prototype.getHotels = function () {
    $.ajax({
        url: "Home/Hotels",
        type: 'POST',
        dataType: "json",
        data: { place: this.place },
        success: function (response) {
            var hotels = response['businesses'];
            hotels.forEach(function (item) {
                $("#hotel").append("<div class='col-md-1 newHotel'>" + "<img class='pics' src=" + item.image_url + ">" + "<br>" + "<a href=" + item.url + ">" + item.name + "</a>" + "<br>" + "<p>" + item.rating + "&#9733" + "</p>" + "</div>");
            });
        }
    });
};
exports.travelObject = Travel;