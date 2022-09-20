using Microsoft.Extensions.Options;
using MovieApi.Models;
using MovieApi.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieApi.Services
{
    public class MovieApiService : IMovieApiService
    {
        public string BaseUrl { get; }
        public string ApiKey { get; }
        private HttpClient httpClient;

        public MovieApiService(IHttpClientFactory httpClientFactory, IOptions<MovieApiOptions> options)
        {
            /*BaseUrl = "https://omdbapi.com/";
            ApiKey = "5b9b7798";*/

            BaseUrl = options.Value.BaseUrl;
            ApiKey = options.Value.ApiKey;
            httpClient = httpClientFactory.CreateClient();
            //httpClient = new HttpClient();
        }

        /*public async Task<string> SearchByTitle(string title)
        {
            var response = await httpClient.GetAsync($"{BaseUrl}?apikey={ApiKey}&s={title}");
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);

            return result;
        }*/
        public async Task<MovieApiResponse> SearchByTitle(string title)
        {
            var response = await httpClient.GetAsync($"{BaseUrl}?apikey={ApiKey}&s={title}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MovieApiResponse>(json);
            if (result.Response == "False") 
            {
                throw new Exception(result.Error);
            }
            return result;
        }

        public async Task<Details> SearchById(string id)
        {
        //https://omdbapi.com/?i=tt0286716&apikey=5b9b7798&page=2
            var response = await httpClient.GetAsync($"{BaseUrl}?apikey={ApiKey}&i={id}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Details>(json);
            if (result.Response == "False")
            {
                throw new Exception(result.Error);
            }
            return result;
        }
    }
}
//https://omdbapi.com/?apikey=5b9b7798&s='{title}'
