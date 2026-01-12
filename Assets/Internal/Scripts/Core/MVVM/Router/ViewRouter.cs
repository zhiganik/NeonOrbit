using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Core.MVVM
{
    public class ViewRouter : IViewRouter
    {
        private readonly IViewFactory _viewFactory;
        
        private readonly Dictionary<Type, IViewHolder> _holdersMap;
        private readonly Dictionary<Type, IViewHolder> _viewToHoldersMap = new();

        public ViewRouter(IViewFactory viewFactory, IEnumerable<IViewHolder> viewHolders)
        {
            _viewFactory = viewFactory;
            _holdersMap = viewHolders.ToDictionary(h => h.Type);
        }
        
        public async UniTask RouteView<T>() where T : ViewLayer
        {
            var view = await _viewFactory.GetInstance<T>();
            
            if (!_holdersMap.TryGetValue(view.HolderType, out var viewHolder))
                throw new Exception($"Unable to find view holder of type {view.HolderType} for {view.GetType()}");

            if (!_viewToHoldersMap.ContainsKey(typeof(T)))
            {
                _viewToHoldersMap.Add(typeof(T), viewHolder);
            }
            
            viewHolder.AddView(view);
        }
    }
}