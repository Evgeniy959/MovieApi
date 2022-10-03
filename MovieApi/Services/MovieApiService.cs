using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache memoryCache;

        public MovieApiService(IHttpClientFactory httpClientFactory, IOptions<MovieApiOptions> options, IMemoryCache memoryCache)
        {
            /*BaseUrl = "https://omdbapi.com/";
            ApiKey = "5b9b7798";*/

            BaseUrl = options.Value.BaseUrl;
            ApiKey = options.Value.ApiKey;
            httpClient = httpClientFactory.CreateClient();
            this.memoryCache = memoryCache;
        }

        public async Task<MovieApiResponse> SearchByTitle(string title)
        {
            MovieApiResponse result = null;
            if (!memoryCache.TryGetValue(title.ToLower(), out result))
            {
                var response = await httpClient.GetAsync($"{BaseUrl}?apikey={ApiKey}&s={title}");
                var json = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<MovieApiResponse>(json);
                if (result.Response == "False")
                {
                    throw new Exception(result.Error);
                }
                memoryCache.Set(title.ToLower(), result);
            }
            
            return result;
        }

        public async Task<Details> SearchById(string id)
        {
            //https://omdbapi.com/?i=tt0286716&apikey=5b9b7798&page=2
            Details result = null;
            if (!memoryCache.TryGetValue(id, out result))
            {
                var response = await httpClient.GetAsync($"{BaseUrl}?apikey={ApiKey}&i={id}");
                var json = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<Details>(json);
                if (result.Response == "False")
                {
                    throw new Exception(result.Error);
                }
                memoryCache.Set(id, result);
            }
            
            return result;
        }
    }
}
//https://omdbapi.com/?apikey=5b9b7798&s='{title}'
