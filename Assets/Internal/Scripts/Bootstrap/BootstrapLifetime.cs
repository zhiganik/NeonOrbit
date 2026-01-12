using Core.Initialization;
using VContainer;
using VContainer.Unity;

namespace Bootstrap
{
    public class BootstrapLifetime : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TestStep>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterEntryPoint<Bootstrap>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}