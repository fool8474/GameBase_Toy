using System;
using UniRx;

namespace Script.Event
{
    public static class IEventObservableExtension
    {
        public static IDisposable Subscribe(this IEventCommand eventCommand, Action action, bool allowSameEvent = false)
        {
            if (!(eventCommand is ISubscribable subscribable))
            {
                return Disposable.Empty;
            }
            
            // 이후 AddTo를 통해 dispoasable을 배치해주는 것이 안전
            return subscribable.Subscribe(action, allowSameEvent);
        }

        public static IDisposable Subscribe<T>(this IEventCommand<T> eventCommand, Action<T> action, bool allowSameEvent = false)
        {
            if (!(eventCommand is ISubscribable<T> subscribable))
            {
                return Disposable.Empty;
            }

            // 이후 AddTo를 통해 dispoasable을 배치해주는 것이 안전
            return subscribable.Subscribe(action, allowSameEvent);
        }
    }
}