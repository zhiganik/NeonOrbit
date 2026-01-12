using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.MVVM
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class ViewHolder : MonoBehaviour, IViewHolder
    {
        [SerializeField] private RectTransform rootTransform;
        
        private readonly Dictionary<Type, ViewLayer> _viewMap = new();

        public Type Type => GetType();

        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                
                if (child.TryGetComponent<ViewLayer>(out var viewLayer))
                {
                    _viewMap.Add(viewLayer.GetType(), viewLayer);

                    if (viewLayer.IsVisible.CurrentValue)
                    {
                        Show(viewLayer);
                    }
                }
            }
        }
        
        private void OnValidate()
        {
            rootTransform ??= GetComponent<RectTransform>();
        }

        public void AddView<T>(T view) where T : ViewLayer
        {
            if (_viewMap.TryGetValue(typeof(T), out var existingView))
            {
                Show(existingView);
                return;
            }
            
            var rectTransform = view.transform as RectTransform;
            
            if (rectTransform == null) return;
            
            rectTransform.SetParent(rootTransform);
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            
            _viewMap.Add(typeof(T), view);
            Show(view);
        }

        protected virtual void Show(ViewLayer view)
        {
            view.SetVisible(true);
        }

        protected virtual void Hide(ViewLayer view)
        {
            view.SetVisible(false);
        }

        public void HideView<T>() where T : ViewLayer
        {
            var type = typeof(T);
            
            if (_viewMap.TryGetValue(type, out var view))
            {
                Hide(view);
            }
        }
        
        public T RemoveView<T>() where T : ViewLayer
        {
            var type = typeof(T);

            if (!_viewMap.Remove(type, out var view)) return null;

            Hide(view);
            return (T)view;
        }
        
        public T GetView<T>() where T : ViewLayer
        {
            var type = typeof(T);
            
            if (!_viewMap.TryGetValue(type, out var view)) return null;
            
            return (T)view;
        }
    }
}