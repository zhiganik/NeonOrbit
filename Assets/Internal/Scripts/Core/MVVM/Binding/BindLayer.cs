namespace Core.MVVM
{
    public abstract class BindLayer<TV, TM> : IBindLayer where TV : IViewLayer
    {
        protected readonly TM Model;
        
        public BindLayer(TM model)
        {
            Model = model;
        }

        protected abstract void BindInternal(TV view);
        protected abstract void UnbindInternal(TV view);
        
        public void Bind(IViewLayer viewLayer)
        {
            BindInternal((TV)viewLayer);
        }

        public void Unbind(IViewLayer viewLayer)
        {
            UnbindInternal((TV)viewLayer);
        }
    }
}