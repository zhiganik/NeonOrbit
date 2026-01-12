using Core.MVVM;
using Core.MVVM.Bucket;
using Core.SceneService;
using Core.SceneService.SplashScreen;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Shared
{
    public class ProjectContext : LifetimeScope
    {
        [SerializeField] private ViewBucket viewBucket;
        [SerializeField] private SceneLoaderConfig sceneLoaderConfig;
        [SerializeField] private ViewHolder[] holders;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterMVVM(builder);
            RegisterServices(builder);
            RegisterModules(builder);
            
            builder.RegisterBuildCallback(container =>
            {
                viewBucket.Initialize();
            });
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.RegisterInstance(sceneLoaderConfig);
            builder.Register<SceneLoader>(Lifetime.Singleton).AsImplementedInterfaces();
        }
        
        private void RegisterModules(IContainerBuilder builder)
        {
            builder.RegisterScreenBindLayer<SplashScreenBindLayer>(Lifetime.Singleton);
        }

        private void RegisterMVVM(IContainerBuilder builder)
        {
            builder.RegisterInstance(viewBucket).AsImplementedInterfaces();
            builder.RegisterCollectionAsInterfaces(holders);
            
            builder.Register<ViewFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ViewRouter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.AutoInjectTypeOf<RuntimeBinder>();
        }
    }
}