using Autofac;
using Autofac.Extensions.DependencyInjection;
using Carter;

namespace Copitos.Application.WebAPI
{ 
    public class Program
    {
        public static bool IsStartedFromTool { get; private set; } = true;

        private static bool CheckStartedFromTool()
        {
            var arg = Environment.GetCommandLineArgs();
            return arg[0].Contains("ef.dll");
        }

        public static void Main(string[] args)
        {
            IsStartedFromTool = CheckStartedFromTool();
            var applicationBuilder = WebApplication.CreateBuilder(args);
            var startup = new vkrobot.application.webapi.Startup.Startup(applicationBuilder.Configuration);
            startup.ConfigureServices(applicationBuilder.Services);
            applicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(containerBuilder =>
                {
                    startup.ConfigureContainer(containerBuilder, applicationBuilder.Services);
                });

            var app = applicationBuilder.Build();
            startup.Configure(app, app.Environment);
            app.MapCarter();
            app.Run();
        }
    }
}