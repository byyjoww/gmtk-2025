using UnityEngine.Events;

namespace GMTK2025.UI
{
    public interface IInteractionInput
    {
        event UnityAction OnControlSchemeChanged;

        string GetInteractionInput(int trigger);
    }
}