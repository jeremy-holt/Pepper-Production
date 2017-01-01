using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFilesEx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PCal.Startup_config;
using Newtonsoft.Json.Serialization;
using PCal.Filters;

namespace PCal
{
    public class Startup
    {
        private const string ExceptionsOnStartup = "Startup";
        private const string ExceptionsOnConfigureServices = "ConfigureServices";
        private readonly Dictionary<string, List<Exception>> _exceptions;

        public Startup(IHostingEnvironment env, ILoggerFactory logger, string[] args = null)
        {
            _exceptions = new Dictionary<string, List<Exception>>
            {
                {ExceptionsOnStartup, new List<Exception>()},
                {ExceptionsOnConfigureServices, new List<Exception>()}
            };

            try
            {
                HostingEnvironment = env;
                LoggerFactory = logger;

                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                    .AddEnvironmentVariables();

                //if (args != null && args.Length > 0)
                //{
                //    builder.AddCommandLine(args);

                //}
                Configuration = builder.Build();
            }
            catch (Exception e)
            {
                _exceptions[ExceptionsOnStartup].Add(e);       
            }
        }

        public ILoggerFactory LoggerFactory { get; }

        private IConfigurationRoot Configuration { get; }
        private IContainer ApplicationContainer { get; set; }

        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            // Add custom application options
            services.AddOptions();
            services.Configure<ApplicationOptions>(Configuration);

            // Add framework services.
            //ConfigureMvc(services, logger);
            ConfigureMvc(services, LoggerFactory);
            

            // Configure Autofac
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        private void ConfigureMvc(IServiceCollection services, ILoggerFactory logger)
        {
            var builder = services.AddMvc();

            builder.AddJsonOptions(
                o =>
                {
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    o.SerializerSettings.Converters.Add(new StringEnumConverter());
                    o.SerializerSettings.Formatting = Formatting.Indented;
                    o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    o.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                });

            // Setup glogal action filters
            builder.AddMvcOptions(o => { o.Filters.Add(new GlobalActionFilter(logger)); });

            // Setup global exception filters
            builder.AddMvcOptions(o => { o.Filters.Add(new GlobalExceptionFilter(logger)); });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger,
            IApplicationLifetime appLifetime)
        {
            logger.AddConsole(Configuration.GetSection("Logging"));
            logger.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();           
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var log = logger.CreateLogger<Startup>();
            if (_exceptions.Any(c => c.Value.Any()))
            {
                app.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/plain";

                        foreach (var ex in _exceptions)
                        {
                            foreach (var val in ex.Value)                            
                            {
                                log.LogError($"{ex.Key}:::{val.Message}");
                                await context.Response.WriteAsync($"Error on {ex.Key}: {ex.Value}")
                                    .ConfigureAwait(false);
                            }
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
                                {
                                    await context.Response.WriteAsync($"<h1>Error: {error.Error.Message}</h1>")
                                        .ConfigureAwait(false);
                                }
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
    }

}