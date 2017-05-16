﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneStop.Models;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OneStop.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Wiki(string place)
        {
            var client = new RestClient("https://en.wikipedia.org/w/api.php");

            var request = new RestRequest(Method.GET);
            request.AddParameter("format", "json");
            request.AddParameter("action","parse");
            request.AddParameter("page", place);
            request.AddParameter("prop", "text");
            request.AddParameter("section", 0);
            request.AddHeader("Api-User-Agent", "Travel-App");

            var response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            return Json(jsonResponse);
        }

        [HttpPost]
        public IActionResult Coord(string place)
        {
            var client = new RestClient("https://geocoder.cit.api.here.com/6.2/");

            var request = new RestRequest("geocode.json?",Method.GET);
            request.AddParameter("searchtext", place);
            request.AddParameter("app_id", env.HereKey);
            request.AddParameter("app_code", env.HereSecret);
            request.AddParameter("gen", "8");

            var response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            return Json(jsonResponse);
        }

        [HttpPost]
        public IActionResult Restaurants(string place)
        {
            var client = new RestClient("http://api.yelp.com/v3");
            var request = new RestRequest("/businesses/search", Method.GET);
            request.AddParameter("term", "restaurant");
            request.AddParameter("location", place);
            request.AddHeader("Authorization", "Bearer " + env.YelpToken);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            return Json(jsonResponse);
        }

        [HttpPost]
        public IActionResult Hotels(string place)
        {
            var client = new RestClient("http://api.yelp.com/v3");
            var request = new RestRequest("/businesses/search", Method.GET);
            request.AddParameter("term", "hotel");
            request.AddParameter("location", place);
            request.AddHeader("Authorization", "Bearer " + env.YelpToken);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            return Json(jsonResponse);
        }

        [HttpPost]
        public IActionResult Weather(string place)
        {
            var client = new RestClient("http://api.openweathermap.org/data/2.5/");
            var request = new RestRequest("forecast?q=" + place, Method.GET);
            request.AddParameter("appid", env.apiWeatherKey);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            return Json(jsonResponse);
        }

        [HttpPost]
        public IActionResult CurrencyCode(string countryCode)
        {
            var client = new RestClient("https://restcountries.eu/rest/v2/alpha/" + countryCode);
            var request = new RestRequest(Method.GET);
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            return Json(jsonResponse);
        }


        [HttpPost]
        public IActionResult Exchange(string currencyCode)
        {
            var client = new RestClient("http://apilayer.net/api/live?");
            var request = new RestRequest(Method.GET);
            request.AddParameter("access_key", env.currencyKey);
            request.AddParameter("currencies", "USD," + currencyCode);
            request.AddParameter("format", "1");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            return Json(jsonResponse);
        }

        [HttpPost]
        public IActionResult Attractions(string latitude, string longitude)
        {
            var client = new RestClient("https://places.demo.api.here.com/places/v1/discover/here?");
            var request = new RestRequest(Method.GET);
            string location = latitude + ',' + longitude;
            request.AddParameter("at", location);
            request.AddParameter("app_id", env.HereKey);
            request.AddParameter("app_code", env.HereSecret);
            request.AddParameter("gen", "8");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            return Json(jsonResponse);
        }

        [HttpPost]
        public IActionResult Airport(string latitude, string longitude)
        {
            var client = new RestClient("https://places.cit.api.here.com/places/v1/discover/around");
            var request = new RestRequest(Method.GET);
            string location = latitude + ',' + longitude;
            request.AddParameter("at", location);
            request.AddParameter("app_id", env.HereKey);
            request.AddParameter("app_code", env.HereSecret);
            request.AddParameter("cat", "airport");
            request.AddParameter("gen", "8");
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            return Json(jsonResponse);
        }


        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();

            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }


        //Save to databases actions
        [HttpPost]
        public IActionResult SaveAttractions(string attractionName)
        {
            Attraction newAttraction = new Attraction(attractionName);
            _db.Attractions.Add(newAttraction);
            _db.SaveChanges();
            return Json(newAttraction);
        }

    }
}
