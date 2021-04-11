using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using bloggr_csharp.Repositories;
using bloggr_csharp.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MySqlConnector;


namespace bloggr_csharp
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "latewinter_artcollective", Version = "v1" });
            });


            // REVIEW[epic=Authentication] creates functionality for authentication
            services.AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              }).AddJwtBearer(options =>
              {
                  options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
                  options.Audience = Configuration["Auth0:Audience"];
              });

            // REVIEW[epic=Authentication] creates functionality for hitting server from client
            //make sure to put app.UseCors("CorsDevPolicy"); on under if env.isdevelopment
            services.AddCors(options =>
              {
                  options.AddPolicy("CorsDevPolicy", builder =>
                {
                    builder
                        .WithOrigins(new string[]{
                            "http://localhost:8080",
                            "http://localhost:8081"
                            })
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
              });



            services.AddScoped<IDbConnection>(x => CreateDbConnection());

            services.AddTransient<ProfilesService>();
            services.AddTransient<ProfilesRepository>();
            services.AddTransient<BlogsService>();
            services.AddTransient<BlogsRepository>();
            services.AddTransient<CommentsService>();
            services.AddTransient<CommentsRepository>();
        }

        private IDbConnection CreateDbConnection()
        {
            string connectionString = Configuration["db:gearhost"];
            return new MySqlConnection(connectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "latewinter_artcollective v1"));
                app.UseCors("CorsDevPolicy");
                //^ to enable client - server interaction
            }

            app.UseHttpsRedirection();

            Auth0ProviderExtension.ConfigureKeyMap(new List<string>() { "id" });

            app.UseRouting();

            //NOTE Runs authentication process
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
