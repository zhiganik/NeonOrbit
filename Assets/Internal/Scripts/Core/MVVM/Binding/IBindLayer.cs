namespace Core.MVVM
{
    public interface IBindLayer
    {
        void Bind(IViewLayer viewLayer);
        void Unbind(IViewLayer viewLayer);
    }
}