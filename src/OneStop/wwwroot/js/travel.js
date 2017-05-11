function Travel() {
    this.place = "";
}

Travel.prototype.getInfo = function () {
    console.log(this.place);
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

exports.travelObject = Travel;