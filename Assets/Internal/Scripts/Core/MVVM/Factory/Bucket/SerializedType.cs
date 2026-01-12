using System;
using UnityEngine;

namespace Core.MVVM.Bucket
{
    [Serializable]
    public struct SerializedType
    {
        [SerializeField] private string assemblyQualifiedName;

        public SerializedType(Type t) => assemblyQualifiedName = t?.AssemblyQualifiedName;

        public Type Resolve() =>
            string.IsNullOrEmpty(assemblyQualifiedName) ? null : Type.GetType(assemblyQualifiedName, false);

        public void Set(Type t) => assemblyQualifiedName = t?.AssemblyQualifiedName;

        public override string ToString() => assemblyQualifiedName;
    }
}