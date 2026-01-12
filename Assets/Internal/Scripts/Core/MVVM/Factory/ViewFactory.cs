using System;
using System.Collections.Generic;
using Core.MVVM.Bucket;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Core.MVVM
{
    public class ViewFactory : IViewFactory
    {
        private readonly IObjectResolver _resolver;
        private readonly IViewBucket _viewBucket;
        private readonly Dictionary<Type, ViewLayer> _cache = new();
        
        public ViewFactory(IObjectResolver resolver, IViewBucket viewBucket)
        {
            _resolver = resolver;
            _viewBucket = viewBucket;
        }
        
        public async UniTask<T> GetInstance<T>() where T : ViewLayer
        {
            var resource = await _viewBucket.GetViewResource<T>();
            
            if (_cache.TryGetValue(typeof(T), out var cachedView))
            {
                return (T)cachedView;
            }
            
            var instance = _resolver.Instantiate(resource);
            _cache.Add(typeof(T), instance);
            return instance;
        }
    }
}