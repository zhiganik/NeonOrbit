using Cysharp.Threading.Tasks;

namespace Core.MVVM.Bucket
{
    public interface IViewBucket
    {
        UniTask<TV> GetViewResource<TV>() where TV : ViewLayer;
    }
}