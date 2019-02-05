using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShopAPI.Infrastructure;

namespace ShopAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                        {
                            options.RequireHttpsMetadata = true;
                            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidIssuer = AuthOptions.ISSUER,

                                ValidateAudience = true,
                                ValidAudience = AuthOptions.AUDIENCE,

                                RequireExpirationTime = true,
                                ValidateLifetime = true,

                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = AuthOptions.sharedSymmetricSecurityKey,

                                // Clock skew compensates for server time drift.
                                ClockSkew = TimeSpan.FromMinutes(AuthOptions.CLOCK_SKEW),
                            };

                            options.Events = new JwtBearerEvents
                            {
                                OnTokenValidated = async context =>
                                {
                                    var timeLeft = context.SecurityToken.ValidTo - DateTime.Now;

                                    if (timeLeft < TimeSpan.FromMinutes(AuthOptions.LIFETIME * 0.5) && timeLeft > TimeSpan.FromSeconds(0))
                                    {
                                        context.Response.Headers.Add(AuthOptions.NEW_ACCESS_TOKEN_HEADER,
                                                                     await TokenService.GenerateToken(context.Principal.Identity.Name));
                                    }
                                }
                                //OnMessageReceived = async context =>
                                //{
                                //    string accessToken = context.Request.Headers["Authorization"];
                                //    if (string.IsNullOrEmpty(accessToken) || !TokenService.IsExpiredToken(accessToken.Split(' ')[1]))
                                //        return;

                                //    string newAccessToken = await TokenService.TryRefreshAccesTokenAsync(accessToken.Split(' ')[1]);

                                //    if (!string.IsNullOrEmpty(newAccessToken))
                                //    {
                                //        context.Response.Headers.Add(SERVICES_URLS.UserService.AccessTokenHEADER, newAccessToken);
                                //        context.Request.Headers["Authorization"] = "Bearer " + newAccessToken;
                                //    }
                                //}
                            };
                        });

            services.AddHttpClient();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Shop API", Version = "v1", Description = "The first swagger doc" });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // SPA stay on https://localhost:44305/ or https://clientspa:443/
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithExposedHeaders(AuthOptions.NEW_ACCESS_TOKEN_HEADER));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
