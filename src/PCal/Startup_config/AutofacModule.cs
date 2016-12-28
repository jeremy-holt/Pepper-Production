using Autofac;
using Microsoft.Extensions.Options;
using Raven.Client;

namespace PCal.Startup_config
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new[]
            {
                typeof(IMarkerWebApi).Assembly
            };


            builder.RegisterAssemblyTypes(assemblies).AsImplementedInterfaces().InstancePerDependency();        

            builder.Register(c => new ConfigRavenDb(c.Resolve<IOptions<ApplicationOptions>>())
                    .InitializeRavenDb(assemblies))
                .As<IDocumentStore>()
                .InstancePerLifetimeScope();

            builder.Register(c => c.Resolve<IDocumentStore>().OpenAsyncSession())
                .As<IAsyncDocumentSession>();
        }
    }
}