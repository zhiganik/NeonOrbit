using System;
using UnityEngine;

namespace Core.MVVM.Bucket
{
    [Serializable]
    public class BucketEntry
    {
#if UNITY_EDITOR
        [SerializeField] private ViewLayer prefab;
#endif
        [SerializeField] private bool isAddressableAsset;
        [SerializeField] private string key;
        [SerializeField] private SerializedType type;
        
        public Type Type => type.Resolve();
        public string Key => key;
        public bool IsAddressableAsset => isAddressableAsset;
    }
}