using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.StaticFilesEx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PCal.Filters;
using PCal.Startup_config;

namespace PCal
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Startup
    {
        private const string ExceptionsOnStartup = "Startup";
        private const string ExceptionsOnConfigureServices = "ConfigureServices";

        private const string SecretKey = "needtogetthisfromenvironment";

        private readonly Dictionary<string, List<Exception>> _exceptions;
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        // ReSharper disable once UnusedParameter.Local
        public Startup(IHostingEnvironment env, ILoggerFactory logger, string[] args = null)
        {
            _exceptions = new Dictionary<string, List<Exception>>
            {
                {ExceptionsOnStartup, new List<Exception>()},
                {ExceptionsOnConfigureServices, new List<Exception>()}
            };

            try
            {
                LoggerFactory = logger;

                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                    .AddEnvironmentVariables();

                Configuration = builder.Build();
            }
            catch (Exception e)
            {
                _exceptions[ExceptionsOnStartup].Add(e);
            }
        }

        private IConfigurationRoot Configuration { get; }
        private IContainer ApplicationContainer { get; set; }

        private ILoggerFactory LoggerFactory { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // ReSharper disable once UnusedMember.Global
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add custom application options
            ConfigureOptions(services);

            ConfigureAuthorization(services);

            // Add framework services.            
            ConfigureMvc(services);

            // Configure Autofac
            ConfigureAutofac(services);

            return new AutofacServiceProvider(ApplicationContainer);
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.AddOptions();

            services.Configure<ApplicationOptions>(Configuration);
            services.Configure<JwtIssuerOptions>(
                options =>
                {
                    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                    options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                }
            );
        }

        private void ConfigureAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            builder.Populate(services);
            ApplicationContainer = builder.Build();
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            var builder = services.AddMvc();

            ConfigureMvcJsonOptions(builder);

            ConfigureMvcFilters(builder);
        }

        private static void ConfigureMvcJsonOptions(IMvcBuilder builder)
        {
            builder.AddJsonOptions(
                o =>
                {
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    o.SerializerSettings.Converters.Add(new StringEnumConverter());
                    o.SerializerSettings.Formatting = Formatting.Indented;
                    o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    o.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                });
        }

        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy("DisneyUser", policy => policy.RequireClaim("DisneyCharacter", "IAmMickey"));
                }
            );
        }

        private void ConfigureMvcFilters(IMvcBuilder builder)
        {
            // Setup glogal action filters
            builder.AddMvcOptions(config => { config.Filters.Add(new GlobalActionFilter(LoggerFactory)); });

            // Setup global exception LoggerFactory
            builder.AddMvcOptions(config => { config.Filters.Add(new GlobalExceptionFilter(LoggerFactory)); });

            // Make authentication compulsory across the board (i.e. shut
            // down EVERYTHING unless explicitly opened up).
            builder.AddMvcOptions(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger,
            IApplicationLifetime appLifetime)
        {
            logger.AddConsole(Configuration.GetSection("Logging"));
            logger.AddDebug();


            ConfigureJwtToken(app);

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            var log = logger.CreateLogger<Startup>();

            if (_exceptions.Any(c => c.Value.Any()))
            {
                app.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/plain";

                        foreach (var ex in _exceptions)
                        foreach (var val in ex.Value)
                        {
                            log.LogError($"{ex.Key}:::{val.Message}");
                            await context.Response.WriteAsync($"Error on {ex.Key}: {ex.Value}")
                                .ConfigureAwait(false);
                        }
                    });
                return;
            }

            try
            {
                app.UseExceptionHandler(
                    builder =>
                    {
                        builder.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                                context.Response.ContentType = "text/html";

                                var error = context.Features.Get<IExceptionHandlerFeature>();

                                if (error != null)
                                    await context.Response.WriteAsync($"<h1>Error: {error.Error.Message}</h1>")
                                        .ConfigureAwait(false);
                            });
                    });

                //app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseFileServer(new FileServerOptions
                {
                    EnableDefaultFiles = true,
                    EnableDirectoryBrowsing = false
                });

                app.UseMvc();

                appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
            }
            catch (Exception e)
            {
                app.Run(
                    async context =>
                    {
                        log.LogError($"{e.Message}");
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync(e.Message).ConfigureAwait(false);
                        await context.Response.WriteAsync(e.StackTrace).ConfigureAwait(false);
                    });
            }
        }

        private void ConfigureJwtToken(IApplicationBuilder app)
        {
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });
        }
    }
}