namespace GMTK2025.Inputs
{
    public interface IPlayerInput
    {
        float Forward { get; }
        float Right { get; }
        bool Jump { get; }
        bool CrouchDown { get; }
        bool CrouchUp { get; }

        float MouseLookUp { get; }
        float MouseLookRight { get; }
        float MouseScroll { get; }
        bool MouseSecondary { get; }
    }
}