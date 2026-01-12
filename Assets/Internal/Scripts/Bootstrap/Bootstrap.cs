using System;
using System.Collections.Generic;
using System.Linq;
using Core.Initialization;
using Core.SceneService;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace Bootstrap
{
    public class Bootstrap : IInitializable
    {
        private readonly CompositeDisposable _disposable;
        private readonly ISceneLoader _sceneLoader;
        private readonly List<IInitializationStep> _initializationSteps;

        public Bootstrap(IEnumerable<IInitializationStep> initializationSteps, ISceneLoader sceneLoader)
        {
            _initializationSteps = initializationSteps.ToList();
            _sceneLoader = sceneLoader;

            _disposable = new CompositeDisposable();
        }
        
        public void Initialize()
        {
            InitializeLoading();
        }

        private async UniTask InitializeLoading()
        {
            try
            {
                foreach (var step in _initializationSteps)
                {
                    await step.Run();
                }
                
                await _sceneLoader.LoadMenu();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                _disposable.Dispose();
            }
        }
    }
}