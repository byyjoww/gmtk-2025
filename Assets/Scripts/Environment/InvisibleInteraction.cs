namespace GMTK2025.Environment
{
    public delegate bool InteractFunc(IInteractor interactor);

    public class InvisibleInteraction : IInteraction
    {
        private InteractFunc onExecute = default;

        public InvisibleInteraction(InteractFunc onExecute)
        {
            this.onExecute = onExecute;
        }

        public virtual bool Execute(IInteractor interactor)
        {
            return onExecute.Invoke(interactor);
        }
    }
}
