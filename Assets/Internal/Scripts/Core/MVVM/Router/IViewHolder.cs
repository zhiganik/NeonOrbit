using System;

namespace Core.MVVM
{
    public interface IViewHolder
    {
        Type Type { get; }
        
        void AddView<T>(T view) where T : ViewLayer;
    }
}