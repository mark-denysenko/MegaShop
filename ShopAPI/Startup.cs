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
                                ClockSkew = TimeSpan.FromMinutes(5),
                            };

                            options.Events = new JwtBearerEvents
                            {
                                // OnAuthenticationFailed = async context =>
                                OnMessageReceived = async context =>
                                {
                                    string accessToken = context.Request.Headers["Authorization"];
                                    if (string.IsNullOrEmpty(accessToken) || !TokenService.IsExpiredToken(accessToken.Split(' ')[1]))
                                        return;

                                    string newAccessToken = await TokenService.TryRefreshAccesTokenAsync(accessToken.Split(' ')[1]);

                                    if (!string.IsNullOrEmpty(newAccessToken))
                                    {
                                        context.Response.Headers.Add(SERVICES_URLS.UserService.AccessTokenHEADER, newAccessToken);
                                        context.Request.Headers["Authorization"] = "Bearer " + newAccessToken;
                                    }

                                    //string login = TokenService.GetLoginFromToken(accessToken.Split(' ')[1]);
                                    //var response = await CustomHttpClientFactory.CreateHttpClientWithoutSslValidation()
                                    //                                            .PostAsync(SERVICES_URLS.UserService.CheckRefreshToken, new StringContent($"\"{login}\"", Encoding.UTF8, "application/json"));

                                    //if (response.IsSuccessStatusCode)
                                    //{
                                    //    var newAccessToken = await TokenService.GenerateToken(login);
                                    //    context.Response.Headers.Add(SERVICES_URLS.UserService.AccessTokenHEADER, newAccessToken);
                                    //    context.Request.Headers["Authorization"] = "Bearer " + newAccessToken;
                                    //}
                                    //else
                                    //{
                                    //    context.Response.Headers.Add("Token-Expired", "true");
                                    //}
                                }
                            };
                        });

            services.AddHttpClient();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

            // middleware for checking expiration of token and refreshing 

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
