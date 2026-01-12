using Cysharp.Threading.Tasks;

namespace Core.MVVM
{
    public interface IViewRouter
    {
        UniTask RouteView<T>() where T : ViewLayer;
    }
}