using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;
using VContainer;

namespace Core.MVVM
{
    public class BindingList<TV, TM> : BindLayer<ListView<TV>, ObservableList<TM>> where TV : ViewLayer
    {
        private ISynchronizedView<TM, TV> _synchronizedView;
        private ListView<TV> _viewList;
        
        private readonly Dictionary<TV, IBindLayer> _binderMap = new ();
        private readonly IObjectResolver _resolver;
        
        public BindingList(ObservableList<TM> modelList, IObjectResolver resolver) : base(modelList)
        {
            _resolver = resolver;
        }

        protected override async void BindInternal(ListView<TV> view)
        {
            _viewList = view;
            await _viewList.Initialize();
            
            _synchronizedView = Model.CreateView(Create);
            _synchronizedView.ObserveAdd().Subscribe(Add);
            _synchronizedView.ObserveRemove().Subscribe(Remove);
            _synchronizedView.ObserveClear().Subscribe(Clear);
            _synchronizedView.ObserveMove().Subscribe(Move);
            _synchronizedView.ObserveReplace().Subscribe(Replace);
            
            foreach (var childView in _synchronizedView)
            {
                BindView(childView);
            }
        }
        
        protected override void UnbindInternal(ListView<TV> view)
        {
            foreach (var pair in _binderMap)
            {
                pair.Value.Unbind(pair.Key);
            }
            
            Clear(default);
            
            _synchronizedView.Dispose();
            _synchronizedView = null;
        }
        
        private TV Create(TM model)
        {
            if(_viewList == null)
                throw new Exception("View list is null!");

            var view = _viewList.Create(_resolver);
            var binder = Activator.CreateInstance(view.BinderType, args: model) as IBindLayer;
            _binderMap[view] = binder;
            return view;
        }
        
        private void Replace(CollectionReplaceEvent<(TM, TV)> replaceEvent)
        {
            RemoveView(replaceEvent.OldValue.Item2);
            BindView(replaceEvent.NewValue.Item2, replaceEvent.Index);
        }
        
        private void Remove(CollectionRemoveEvent<(TM, TV)> removeEvent)
        {
            var view = removeEvent.Value.Item2;
            if (!view) return;
            
            RemoveView(view);
        }
        
        private void Add(CollectionAddEvent<(TM,TV)> addEvent)
        {
            BindView(addEvent.Value.Item2);
        }
        
        private void Move(CollectionMoveEvent<(TM, TV)> moveEvent)
        {
            _viewList.Move(moveEvent.Value.Item2, moveEvent.NewIndex);
        }
        
        private void Clear(Unit unit)
        {
            _binderMap.Clear();
            _viewList.Clear(); 
        }
        
        private void BindView(TV view, int? index = null)
        {
            _binderMap[view].Bind(view);
            _viewList.Add(view, index);
        }
        
        private void RemoveView(TV view)
        {
            _binderMap[view].Unbind(view);
            _binderMap.Remove(view);
            _viewList.Remove(view);
        }
    }
}