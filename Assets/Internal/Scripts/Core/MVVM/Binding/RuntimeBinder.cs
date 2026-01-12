using System;
using UnityEngine;
using VContainer;

namespace Core.MVVM
{
    [DisallowMultipleComponent]
    public sealed class RuntimeBinder : MonoBehaviour, IDisposable
    {
        private ViewLayer _view;
        private IDisposable _bag;
        private IBindLayer _bindLayer;

        private Type ViewModelType => _view.BinderType;
        
        [Inject]
        private void Construct(IObjectResolver container)
        {
            _view = GetComponent<ViewLayer>();

            if (_view == null)
            {
                Debug.LogWarning($"Unused RuntimeBinder for {gameObject.name}");
                return;
            }
            
            _bindLayer = container.Resolve(ViewModelType) as IBindLayer;
        }

        private void OnEnable()
        {
            _bindLayer?.Bind(_view);
        }
        
        private void OnDisable()
        {
            _bindLayer?.Unbind(_view);
        }

        public void Dispose()
        {
            _bag?.Dispose();
        }
    }
}