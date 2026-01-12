using System;
using Core.MVVM;
using R3;

namespace Core.SceneService.SplashScreen
{
    public class SplashScreenBindLayer : BindLayer<SplashScreenView, SplashScreen>
    {
        private IDisposable _disposable;
        
        public SplashScreenBindLayer(SplashScreen model) : base(model)
        {
        }

        protected override void BindInternal(SplashScreenView view)
        {
            var bag = new DisposableBag();
            Model.Duration.Subscribe(view.SetDuration).AddTo(ref bag);
            Model.Text.Subscribe(view.SetSplashText).AddTo(ref bag);
            Model.IsVisible.Subscribe(view.SetVisible).AddTo(ref bag);
            _disposable = bag;
        }

        protected override void UnbindInternal(SplashScreenView view)
        {
            _disposable?.Dispose();
        }
    }
}