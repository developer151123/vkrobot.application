using System.Reflection;
using Autofac;
using AutoMapper;

namespace vkrobot.application.webapi.Startup
{
    public class AutomapperModule : Autofac.Module
    {        
        private readonly AssemblyName[] _assembliesToScan;
        
        public AutomapperModule(AssemblyName[] assembliesToScan)
        {            
            _assembliesToScan = assembliesToScan;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            List<Assembly> assemblies = new List<Assembly>();
            foreach (var name in _assembliesToScan)
            {
                Assembly assembly = Assembly.Load(name);
                assemblies.Add(assembly);
            }

            var allTypes = assemblies
                 .Where(a => !a.IsDynamic && a.GetName().Name != nameof(AutoMapper))
                 .Distinct() // avoid AutoMapper.DuplicateTypeMapConfigurationException
                 .SelectMany(a => a.DefinedTypes)
                 .ToArray();

            var openTypes = new[] {
                            typeof(IValueResolver<,,>),
                            typeof(IMemberValueResolver<,,,>),
                            typeof(ITypeConverter<,>),
                            typeof(IValueConverter<,>),
                            typeof(IMappingAction<,>)
            };

            foreach (var type in openTypes.SelectMany(openType =>
             allTypes.Where(t => t.IsClass && !t.IsAbstract && ImplementsGenericInterface(t.AsType(), openType))))
            {
                builder.RegisterType(type.AsType()).InstancePerDependency();
            }

            builder.Register<AutoMapper.IConfigurationProvider>(ctx => new MapperConfiguration(cfg => cfg.AddMaps(assemblies))).SingleInstance();
            builder.Register<IMapper>(ctx => new Mapper(ctx.Resolve<AutoMapper.IConfigurationProvider>(), ctx.Resolve)).InstancePerDependency();
        }

        private static bool ImplementsGenericInterface(Type type, Type interfaceType)
          => IsGenericType(type, interfaceType) || type.GetTypeInfo().ImplementedInterfaces.Any(@interface => IsGenericType(@interface, interfaceType));

        private static bool IsGenericType(Type type, Type genericType)
                  => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }
}
