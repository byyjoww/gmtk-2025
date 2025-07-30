namespace GMTK2025.Environment
{
    public class VisibleInteraction : IVisibleInteraction
    {
        private InteractFunc onExecute = default;

        public string Interaction { get; private set; }
        public int Trigger { get; }

        public VisibleInteraction(string interaction, int action, InteractFunc onExecute)
        {
            this.Interaction = interaction;
            this.Trigger = action;
            this.onExecute = onExecute;
        }

        public virtual bool Execute(IInteractor interactor)
        {
            return onExecute.Invoke(interactor);
        }
    }
}
