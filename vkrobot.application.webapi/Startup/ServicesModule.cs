using System.Reflection;
using Autofac;
using AutoMapper;
using vkrobot.application.data;
using vkrobot.application.interfaces;
using vkrobot.application.services;
using vkrobot.application.webapi.Configuration;

namespace vkrobot.application.webapi.Startup
{
    public class ServicesModule : Autofac.Module
    {

        private readonly string _migrationAssembly;
        private readonly bool _isStartedFromTool;
        private readonly IConfiguration _configuration;
        public ServicesModule(IConfiguration configuration, string migrationAssembly, bool isStartedFromTool)
        {
            _configuration = configuration;
            _migrationAssembly = migrationAssembly;
            _isStartedFromTool= isStartedFromTool;
        }

        public static AssemblyName[] GetReferencesAssemblies()
        {
            return Assembly.GetExecutingAssembly().GetReferencedAssemblies();
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            var appConfig = _configuration.GetSection("Application").Get<ApplicationConfiguration>();

            // Register database context
            builder.RegisterContext<ApplicationData>( appConfig!.ConnectionString!, _migrationAssembly);
            
            builder.Register(c => new GroupService(c.Resolve<ApplicationData>(),
                    c.Resolve<IMapper>(),
                    c.Resolve<ILogger<GroupService>>()
                ))
                .As<IGroupService>()
                .InstancePerLifetimeScope();
            
            builder.Register(c => new MessageService(c.Resolve<ApplicationData>(),
                    c.Resolve<IMapper>(),
                    c.Resolve<ILogger<MessageService>>()
                ))
                .As<IMessageService>()
                .InstancePerLifetimeScope();
            

            // Migrate database startable
            builder.Register(c => new MigrateDatabase(                
                   c.Resolve<ApplicationData>(),      
                   appConfig,
                   c.Resolve<IGroupService>(),
                   c.Resolve<ILogger<MigrateDatabase>>(),                   
                   _isStartedFromTool))
                   .As<IStartable>()
                   .SingleInstance();

        }
    }
}
