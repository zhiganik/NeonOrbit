using Cysharp.Threading.Tasks;

namespace Core.MVVM
{
    public interface IViewFactory
    {
        UniTask<T> GetInstance<T>() where T : ViewLayer;
    }
}