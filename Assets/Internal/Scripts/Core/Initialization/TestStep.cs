using Cysharp.Threading.Tasks;

namespace Core.Initialization
{
    public class TestStep : IInitializationStep
    {
        public async UniTask Run()
        {
            await UniTask.WaitForSeconds(3);
        }
    }
}