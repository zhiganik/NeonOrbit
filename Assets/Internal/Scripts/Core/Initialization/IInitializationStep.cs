using Cysharp.Threading.Tasks;

namespace Core.Initialization
{
    public interface IInitializationStep
    {
        public UniTask Run();
    }
}