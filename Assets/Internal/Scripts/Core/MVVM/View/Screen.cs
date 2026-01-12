using System;
using UnityEngine;

namespace Core.MVVM
{
    [RequireComponent(typeof(RuntimeBinder))]
    public abstract class Screen<T, TH> : ViewLayer 
        where T : IBindLayer
        where TH : IViewHolder
    {
        public override Type BinderType => typeof(T);
        public override Type HolderType => typeof(TH);
    }
}