using Core.MVVM;
using Core.SceneService.SplashScreen;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.SceneService
{
    public interface ISceneLoader
    {
        UniTask LoadMenu();
        UniTask UnloadAll();
        UniTask ShowSplashScreen();
        UniTask HideSplashScreen();
    }
    
    public class SceneLoader : ISceneLoader
    {
        private readonly SceneLoaderConfig _config;
        
        public SceneLoader(SceneLoaderConfig config)
        {
            _config = config;
        }

        public UniTask LoadGameplay(bool fadeIn = false, bool fadeOut = false)
        {
            return LoadScene(_config.Gameplay.BuildIndex, fadeIn, fadeOut);
        }
        
        public async UniTask LoadMenu()
        {
            await LoadScene(_config.MainMenu.BuildIndex);
        }

        public async UniTask UnloadAll()
        {
            await HideSplashScreen();
        }
        
        public UniTask ShowSplashScreen()
        {
            EventBus<FadeInSplashScreen>.Emit(new FadeInSplashScreen(_config.FadeDuration));
            return UniTask.WaitForSeconds(_config.FadeDuration);
        }

        public UniTask HideSplashScreen()
        {
            EventBus<FadeOutSplashScreen>.Emit(new FadeOutSplashScreen(_config.FadeDuration));
            return UniTask.WaitForSeconds(_config.FadeDuration);
        }

        private async UniTask LoadScene(int buildIndex, bool fadeIn = true, bool fadeOut = true)
        {
            if (fadeIn) await ShowSplashScreen();
            await SceneManager.LoadSceneAsync(buildIndex).ToUniTask();
            if (fadeOut) await HideSplashScreen();
        }
    }
}