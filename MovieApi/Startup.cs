using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieApi.Options;
using MovieApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddTransient<IMovieApiService, MovieApiService >();
            services.AddWebOptimizer(options =>
            { 
                options.CompileScssFiles(); 
            });
            services.Configure<MovieApiOptions>(options=> 
            {
                options.ApiKey = Configuration["MovieApiKey:ApiKey"];
                options.BaseUrl = Configuration["MovieApiKey:BaseUrl"];
            });
            services.AddHttpClient();
            /*Console.WriteLine(Configuration.GetSection("MovieApiKey").GetValue<string>("BaseUrl"));
            Console.WriteLine(Configuration.GetSection("MovieApiKey").GetValue<string>("ApiKey"));*/
            /*Console.WriteLine(Configuration["MovieApiKey:BaseUrl"]);
            Console.WriteLine(Configuration["MovieApiKey:ApiKey"]);*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseWebOptimizer();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{title?}");
            });
        }
    }
}
