using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Core.MVVM
{
    public static class InjectExtensions
    {
        public static void AutoInjectTypeOf<T>(this IContainerBuilder builder) where T : Object
        {
            builder.RegisterBuildCallback(container =>
            {
                var injectBinders = Object.FindObjectsByType<T>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

                foreach (var binder in injectBinders)
                {
                    container.Inject(binder);
                }
            });
        }

        public static void RegisterCollectionAsInterfaces<T>(this IContainerBuilder builder, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                builder.RegisterInstance(item).AsImplementedInterfaces();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterScreenBindLayer<TB>(this IContainerBuilder builder, Lifetime lifetime = Lifetime.Scoped) 
            where TB : IBindLayer
        {
            var binderType = typeof(TB);
            var modelType = GetGenericArgumentTypeFromBinder(binderType, BinderGenericTypes.Model);
            
            if (modelType == null)
            {
                throw new InvalidOperationException(
                    $"Type {binderType.FullName} must inherit {typeof(BindLayer<,>).FullName} to infer the model type (TM).");
            }

            if (!builder.Exists(modelType))
            {
                builder.Register(modelType, lifetime)
                    .As(modelType)
                    .AsImplementedInterfaces();
            }
            
            builder.Register(binderType, lifetime)
                .As(binderType);
        }
        
        private static Type GetGenericArgumentTypeFromBinder(Type type, BinderGenericTypes argument)
        {
            for (var t = type; t != null && t != typeof(object); t = t.BaseType)
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(BindLayer<,>))
                {
                    return t.GetGenericArguments()[(int)argument];
                }
            }

            var concrete = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(BindLayer<,>));
            return concrete?.GetGenericArguments()[1];
        }
        
        private enum BinderGenericTypes
        {
            View = 0,
            Model = 1,
        }
    }
}