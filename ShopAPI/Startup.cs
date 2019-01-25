﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                                OnAuthenticationFailed = context =>
                                {
                                    if (context.Exception.GetType() == typeof(Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException))
                                    {
                                        context.Response.Headers.Add("Token-Expired", "true");

                                        //string accessToken = context.Request.Headers["Authorization"];
                                        //accessToken = accessToken.Split(' ')[1];

                                        //var handler = new HttpClientHandler();
                                        //handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                                        //var client = new HttpClient(handler);
                                        //client.PostAsJsonAsync(SERVICES_URLS.UserService.RefreshAccessToken, accessToken);

                                    }
                                    return Task.CompletedTask;
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
