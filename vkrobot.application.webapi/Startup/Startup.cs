using System.Reflection;
using System.Text.Json.Serialization;
using Autofac;
using Carter;
using Carter.OpenApi;
using Copitos.Application.WebAPI;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using vkrobot.application.webapi.Helpers;

namespace vkrobot.application.webapi.Startup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string APICorsPolicy = "APICorsPolicy";
        
        public void ConfigureServices(IServiceCollection services)
        {
            // Add logging console switchable from application settings 
            services.AddLogging(options =>
            {
                options.AddSimpleConsole(o => o.TimestampFormat = "[HH:mm:ss dd.MM.yyyy]");
                options.AddJsonConsole(o => { });
                options.AddSystemdConsole(o => { });
            });

            // Add CORS            
            services.AddCors(options =>
            {
                options.AddPolicy(APICorsPolicy, new ApplicationCorsPolicy());
            });

            services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new DateTimeConverter());
            });
            
            
            // Add swagger support
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = "VK Robot Application",
                    Version = "v1",
                    Title = "VK Robot Application"
                });

                options.DocInclusionPredicate((s, description) =>
                {
                    foreach (var metaData in description.ActionDescriptor.EndpointMetadata)
                    {
                        if (metaData is IIncludeOpenApi)
                        {
                            return true;
                        }
                    }
                    return false;
                });
            });
            
            services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddCarter();
        }

        public void ConfigureContainer(ContainerBuilder builder, IServiceCollection services)
        {
            // Add any Autofac modules or registrations.
            // This is called AFTER ConfigureServices so things you
            // register here OVERRIDE things registered in ConfigureServices.
            var migrationAssembly = Assembly.GetExecutingAssembly().GetName();
            var assemblyNames = new List<AssemblyName>(Assembly.GetExecutingAssembly().GetReferencedAssemblies());
            var referenced = new List<AssemblyName>();
            referenced.AddRange(ServicesModule.GetReferencesAssemblies());
      

            builder.RegisterModule(new AutomapperModule(assemblyNames.Union(referenced).ToArray()));
            builder.RegisterModule(new ServicesModule(Configuration, migrationAssembly.Name!, Program.IsStartedFromTool));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionHandlerPathFeature != null)
                {
                    var exception = exceptionHandlerPathFeature.Error;

                    var result = JsonConvert.SerializeObject(new { error = exception.Message });
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                }
            }));
            
            app.UseCors(APICorsPolicy);
        }
    }
}
