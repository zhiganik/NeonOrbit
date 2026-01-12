using DG.Tweening;
using MPUIKIT;
using UnityEngine;
using UnityEngine.UI;

namespace Core.SceneService.SplashScreen
{
    public class LoadingCircle : MonoBehaviour
    {
        [SerializeField] private MPImage image;
        [SerializeField] private float duration = 2f;
        
        private Sequence _sequence;

        private void Awake()
        {
            enabled = false;
        }

        private void OnValidate()
        {
            image ??= GetComponent<MPImage>();
        }

        public void OnEnable()
        {
            _sequence ??= GetSequence();
            _sequence.Play();
        }

        public void OnDisable()
        {
            _sequence.Pause();
        }

        private Sequence GetSequence()
        {
            return DOTween.Sequence()
                .Append(transform.DORotate(Vector3.forward * 360f, duration, RotateMode.FastBeyond360))
                .SetLoops(-1, LoopType.Incremental);
        }
    }
}