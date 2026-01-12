using VContainer;
using VContainer.Unity;

namespace MainMenu
{
    public class MainMenuLifetime : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
           builder.RegisterEntryPoint<MainMenuEntryPoint>();
        }
    }
}