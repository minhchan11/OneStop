﻿var Travel = require('../js/travel.js').travelObject;
$(document).ready(function () {
    var newTravel = new Travel();
    $("#customer").submit(function (event) {
        event.preventDefault();
        $("#hide").removeClass("hidden");
        $('span[class=details]').text("");
        $("#weather").text("");
        $("#budgetConvert").text("");
        newTravel.place = $("#destination").val().replace(" ", "_").toLowerCase();
        newTravel.getInfo();
        var newBudget = parseFloat($("#budget").val());
        var newPosition = newTravel.getCoordinate(newBudget);
        newTravel.getRestaurants();
        newTravel.getHotels();
        newTravel.getWeather();
        //setTimeout(function () {
        //    newTravel.getAttractions(newPosition[0], newPosition[1]);
        //}, 50);
        //setTimeout(function () {
        //    newTravel.getAirport(newPosition[0], newPosition[1]);
        //}, 60);


    });
});