using Core.MVVM;
using Core.Shared;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.SceneService.SplashScreen
{
    public class SplashScreenView : Screen<SplashScreenBindLayer, OverlayViewHolder>
    {
        [SerializeField] private float fadeDuration;
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private LoadingCircle loadingCircle;

        public void SetDuration(float value)
        {
            fadeDuration = value;
        }
        
        public void SetSplashText(string text)
        {
            loadingText.SetText(text);
        }

        public override void SetVisible(bool state)
        {
            loadingCircle.enabled = state;
            CanvasGroup.DOFade(state ? 1f : 0f, fadeDuration).onComplete += () => loadingCircle.enabled = state;
            CanvasGroup.blocksRaycasts = state;
            CanvasGroup.interactable = state;
        }
    }
}