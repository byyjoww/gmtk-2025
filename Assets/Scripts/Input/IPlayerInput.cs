using System;
using UnityEngine.Events;

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
        bool IsEnabled { get; }

        event UnityAction<int> OnInteractDown;
        event UnityAction<int> OnInteractUp;
    }
}