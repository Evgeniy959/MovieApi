using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApi.Models;
using MovieApi.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieApiService movieApiService;

        public HomeController(ILogger<HomeController> logger, IMovieApiService movieApiService)
        {
            _logger = logger;
            this.movieApiService = movieApiService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Search(string title)
        {
            /*HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://omdbapi.com/?apikey=5b9b7798&s={title}");
            var result = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(result);*/
            //Console.WriteLine(title +"-"+age);
            //ViewBag.Result = result;
            var result = await movieApiService.SearchByTitle(title);
            ViewBag.MovieTitle = title;
            return View(result);
        }
        public async Task<IActionResult> Details(string id)
        {
            Details details = await movieApiService.SearchById(id);
            Console.WriteLine("999999999 - " + id);
            return View(details);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
