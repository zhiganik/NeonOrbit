using System;
using Core.MVVM;
using R3;

namespace Core.SceneService.SplashScreen
{
    public interface ISplashScreen
    {
        void FadeIn(in FadeInSplashScreen args);
        void FadeOut(in FadeOutSplashScreen args);
    }
    
    public class SplashScreen : ISplashScreen, IDisposable
    {
        private readonly ReactiveProperty<float> _duration = new (DEFAULT_FADE_DURATION);
        private readonly ReactiveProperty<bool> _isVisible = new (false);
        private readonly ReactiveProperty<string> _text = new (DEFAULT_SPLASH_TEXT);

        private readonly IDisposable _disposable;
        
        public ReadOnlyReactiveProperty<float> Duration => _duration;
        public ReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        public ReadOnlyReactiveProperty<string> Text => _text;
        
        private const float DEFAULT_FADE_DURATION = 1f;        
        private const string DEFAULT_SPLASH_TEXT = "Loading...";
        
        public SplashScreen()
        {
            var bag = new DisposableBag();
            EventBus<FadeInSplashScreen>.On(OnFadeIn).AddTo(ref bag);
            EventBus<FadeOutSplashScreen>.On(OnFadeOut).AddTo(ref bag);
            _disposable = bag;
        }

        public void FadeIn(in FadeInSplashScreen args)
        {
            _duration.Value = args.Duration;
            _text.Value = string.IsNullOrEmpty(args.SplashText) ? DEFAULT_SPLASH_TEXT : args.SplashText;
            _isVisible.Value = true;
        }

        public void FadeOut(in FadeOutSplashScreen args)
        {
            _duration.Value = args.Duration;
            _isVisible.Value = false;
        }
        
        private void OnFadeIn(in FadeInSplashScreen @event) => FadeIn(@event);
        private void OnFadeOut(in FadeOutSplashScreen @event) => FadeOut(@event);

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}