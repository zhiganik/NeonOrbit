using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.MVVM
{
    public static class ViewExtensions
    {
        public static void SetVisible(this CanvasGroup canvasGroup, bool state)
        {
            canvasGroup.alpha = state ? 1f : 0;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
        
        public static async UniTask SetVisible(this CanvasGroup canvasGroup, bool state, AnimationCurve curve, 
            float duration = 1f, CancellationToken token = default)
        {
            var startAlpha = canvasGroup.alpha;
            var targetAlpha = state ? 1f : 0f;
            var elapsedTime = 0f;

            while (elapsedTime < duration && !token.IsCancellationRequested)
            {
                elapsedTime += Time.deltaTime;
                var t = elapsedTime / duration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, curve.Evaluate(t));
                await UniTask.NextFrame();
            }

            canvasGroup.alpha = targetAlpha;
            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;
        }
    }
}