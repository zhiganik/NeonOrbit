using System;
using R3;
using UnityEngine;

namespace Core.MVVM
{
    public interface IEvent
    {
        
    }
    
    public static class EventBus
    {
        private static DisposableBag _disposables;
        
        public static IDisposable AddEvent(IDisposable disposable) => disposable.AddTo(ref _disposables);
        
        public static void Clear() => _disposables.Dispose();
    }

    public static class EventBus<T> where T : IEvent
    {
        private static Subject<T> _subject;

        public delegate void EventAction(in T @event);

        static EventBus()
        {
            _subject = new Subject<T>();
        }

        public static IDisposable On(EventAction action)
        {
            return EventBus.AddEvent(_subject.Subscribe(a => action(a)));
        }

        public static IDisposable On(Action action)
        {
            return EventBus.AddEvent(_subject.Subscribe(_ => action()));
        }
        
        public static IDisposable On(Action<T> action)
        {
            return EventBus.AddEvent(_subject.Subscribe(action));
        }

        public static void Emit(T value)
        {
            Debug.Log($"Emit event: {typeof(T).Name}");
            _subject.OnNext(value);
        }
    }
}