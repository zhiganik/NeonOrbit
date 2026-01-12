using Eflatun.SceneReference;
using UnityEngine;

namespace Core.SceneService
{
    [CreateAssetMenu(fileName = nameof(SceneLoaderConfig), menuName = "ScriptableObjects/" + nameof(SceneLoaderConfig), order = 0)]
    public class SceneLoaderConfig : ScriptableObject
    {
        [field: SerializeField] public SceneReference MainMenu { get; private set; }
        [field: SerializeField] public SceneReference Gameplay { get; private set; }

        [field: SerializeField] public float FadeDuration { get; private set; }
    }
}