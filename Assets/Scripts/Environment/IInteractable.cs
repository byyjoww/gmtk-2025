using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.Environment
{
    public interface IInteractable
    {
        Transform transform { get; }

        event UnityAction OnForcedUnfocus;

        InteractionCollection GetInteractions(IInteractor interactor);
        bool CanFocus(IInteractor interactor);
        void OnSetFocus(IInteractor interactor);
        void OnRemoveFocus();
    }
}
