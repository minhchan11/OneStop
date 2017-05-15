﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OneStop.Model;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OneStop.Controllers
{
    public class HomeController : Controller
    {
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
            request.AddHeader("Authorization", "Bearer Ao9wyNEJi_JdysZZ2BQGr0sG2CgKnj1Pe3QEEjbSa6__mbKWQYitQne4wAJAmWamYagq7-P-4iZb4mzAH6ZIs_bjomtWwuU4XeFck6RhJmuQihOFdWMZqGIWhAUSWXYx");
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
            request.AddHeader("Authorization", "Bearer Ao9wyNEJi_JdysZZ2BQGr0sG2CgKnj1Pe3QEEjbSa6__mbKWQYitQne4wAJAmWamYagq7-P-4iZb4mzAH6ZIs_bjomtWwuU4XeFck6RhJmuQihOFdWMZqGIWhAUSWXYx");
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
            var client = new RestClient("http://api.openweathermap.org/data/2.5");
            var request = new RestRequest(place, Method.GET);
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

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();

            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }

    }
}
