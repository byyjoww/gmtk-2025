using GMTK2025.Environment;
using UnityEngine.Events;

namespace GMTK2025.UI
{
    public interface IInteractionModel
    {
        public event UnityAction<IInteractor, IInteractable> OnFocus;
        public event UnityAction OnDefocus;
    }
}