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
            var result = await movieApiService.SearchByTitle(title);
            ViewBag.Result = result.totalResults;
            ViewBag.MovieTitle = result?.Search[0]?.Title; 
            return View(result);
        }
        public async Task<IActionResult> Details(string id)
        {
            Details details = await movieApiService.SearchById(id);
            Console.WriteLine("Details - " + id);
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
