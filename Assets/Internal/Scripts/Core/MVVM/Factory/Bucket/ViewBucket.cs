using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.MVVM.Bucket
{
    [CreateAssetMenu(fileName = nameof(ViewBucket), menuName = "Buckets/" + nameof(ViewBucket))]
    public class ViewBucket : ScriptableObject, IViewBucket
    {
        [SerializeField] private List<BucketEntry> viewEntries;
        [SerializeField] private bool isValid;
        [NonSerialized] private bool _isInitialized = false;
        
        private Dictionary<Type, BucketEntry> _typeMap;
        private Dictionary<string, AssetReference> _addressableReferences;
        private Dictionary<AssetReference, ViewLayer> _loaded;

        public void Initialize()
        {
            if (_isInitialized) return;
            
            _typeMap = viewEntries.ToDictionary(v => v.Type, v => v);
            _addressableReferences = viewEntries.Where(v => v.IsAddressableAsset)
                .ToDictionary(v => v.Key, v => new AssetReference(v.Key));
            
            var keys = new HashSet<Type>();
            
            foreach (var pair in _typeMap)
            {
                if (string.IsNullOrEmpty(pair.Value.Key))
                {
                    Debug.LogError($"View [{pair.Key}] key is empty! Unable to proceed with that view!");
                    keys.Add(pair.Key);
                }
            }

            foreach (var key in keys)
            {
                _typeMap.Remove(key);
            }

            _loaded = new Dictionary<AssetReference, ViewLayer>();
            _isInitialized = true;
        }

        public async UniTask<TV> GetViewResource<TV>() where TV : ViewLayer
        {
            Validate();

            if (!GetEntryByType(typeof(TV), out var entry))
            {
                Debug.LogError($"View of type {typeof(TV)} must be placed in the bucket");
                return null;
            }
            
            if (_addressableReferences.TryGetValue(entry.Key, out var reference))
            {
                if (_loaded.TryGetValue(reference, out var view))
                {
                    return view as TV;
                }

                var go = await reference.LoadAssetAsync<GameObject>();
                var viewReference = go.GetComponent<TV>();
                _loaded.Add(reference, viewReference);
                return viewReference;
            }
            
            var resource = await Resources.LoadAsync<TV>(entry.Key);
            return resource as TV;
        }
        
        private bool GetEntryByType(Type type, out BucketEntry entry)
        {
            return _typeMap.TryGetValue(type, out entry);
        }
        
        private void Validate()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("View Bucket is not initialized!");

            if (!isValid)
                throw new InvalidOperationException("View Bucket is not valid!");
        }
    }
}