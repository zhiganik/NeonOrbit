using Core.MVVM;

namespace Core.SceneService.SplashScreen
{
    public struct FadeOutSplashScreen : IEvent
    {
        public float Duration { get; }

        public FadeOutSplashScreen(float duration)
        {
            Duration = duration;
        }
    }
}