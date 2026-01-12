using Core.MVVM;

namespace Core.SceneService.SplashScreen
{
    public struct FadeInSplashScreen : IEvent
    {
        public float Duration { get; }
        
        public string SplashText { get; }

        public FadeInSplashScreen(float duration, string splashText = null)
        {
            Duration = duration;
            SplashText = splashText;
        }
    }
}