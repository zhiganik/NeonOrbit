using System;
using System.Collections;
using System.Collections.Generic;
using Core.MVVM.Bucket;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Core.MVVM
{
    [Serializable]
    public class ListView<T> : IViewLayer, IEnumerable<T> where T : ViewLayer
    {
        public T this[int i] => _items[i];
        public int Count => _items.Count;
        
        [SerializeField] protected Transform container;
        [SerializeField] [NotNull] private ViewBucket bucket;

        protected T Resource;
        
        private List<T> _items = new ();

        public virtual async UniTask Initialize()
        {
            Resource = await bucket.GetViewResource<T>();
        }
        
        public virtual T Create(IObjectResolver resolver)
        {
            if (Resource == null) 
                throw new Exception($"Resource of Type {typeof(T)} is null");
            
            return resolver.Instantiate(Resource, container);
        }

        public virtual void Add(T item, int? index = null)
        {
            _items.Add(item);

            index = index ?? _items.Count - 1;
            item.transform.SetSiblingIndex(index.Value);
        }

        public virtual void Remove(T item)
        {
            _items.Remove(item);
            Object.Destroy(item.gameObject);
        }

        public virtual void Clear()
        {
            foreach (var item in _items)
            {
                Object.Destroy(item.gameObject);
            }
            
            _items.Clear();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        
        public void Move(T view, int index) 
        {
            view.transform.SetSiblingIndex(index);
        }
    }
}