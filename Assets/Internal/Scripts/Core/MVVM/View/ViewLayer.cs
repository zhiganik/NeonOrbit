using System;
using R3;
using UnityEngine;

namespace Core.MVVM
{
    public abstract class ViewLayer<T> : ViewLayer
        where T : IBindLayer
    {
        public override Type BinderType => typeof(T);
        
        public override Type HolderType => null;
    }
    
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public abstract class ViewLayer : MonoBehaviour, IViewLayer
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;

        private ReactiveProperty<bool> _isVisible;
        private IDisposable _bag;
        
        public abstract Type BinderType { get; }
        public abstract Type HolderType { get; }
        
        public ReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        public CanvasGroup CanvasGroup => canvasGroup;
        public RectTransform RectTransform => rectTransform;

        private void Awake()
        {
            _isVisible = new ReactiveProperty<bool>(canvasGroup.alpha >= 1);
        }
        
        private void OnDestroy()
        {
            _bag?.Dispose();
        }

        private void OnValidate()
        {
            canvasGroup ??= GetComponent<CanvasGroup>();
            rectTransform ??= GetComponent<RectTransform>();
        }

        public virtual void SetVisible(bool visible)
        {
            canvasGroup.SetVisible(visible);
            _isVisible.Value = visible;
        }
    }
}