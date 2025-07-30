namespace GMTK2025.Environment
{
    public interface IVisibleInteraction : IInteraction
    {
        int Trigger { get; }
        string Interaction { get; }
    }
}
