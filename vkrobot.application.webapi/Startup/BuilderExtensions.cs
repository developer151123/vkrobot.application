using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using vkrobot.application.data;
using vkrobot.application.interfaces;
using vkrobot.application.services;

namespace vkrobot.application.webapi.Startup
{
    internal static class BuilderExtensions
    {
        public static void RegisterContext<TContext>(this ContainerBuilder builder, string connectionString, string migrationAssembly) where TContext : DbContext
        {
            builder.Register(componentContext =>
            {
                var serviceProvider = componentContext.Resolve<IServiceProvider>();

                var dbContextOptions = new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>());
                var optionsBuilder = new DbContextOptionsBuilder<TContext>(dbContextOptions)
                    .UseApplicationServiceProvider(serviceProvider)
                    .UseNpgsql(connectionString,
                        serverOptions => serverOptions.MigrationsAssembly(migrationAssembly));

                return optionsBuilder.Options;
            }).As<DbContextOptions<TContext>>()
                .InstancePerLifetimeScope();
            

            builder.Register(context => context.Resolve<DbContextOptions<TContext>>())
                .As<DbContextOptions>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
