using GMTK2025.Environment;
using System;
using UnityEngine;

namespace GMTK2025.UI
{
    public class WorldInteractionViewController : BaseInteractionViewController, IDisposable
    {
        private Transform defaultViewParent = default;

        public WorldInteractionViewController(IInteractionModel[] models, InteractionView view, IInteractionInput input) : base(models, view, input)
        {
            defaultViewParent = view.transform.parent;
        }

        protected override void SetFocus(IInteractor interactor, IInteractable interactible)
        {
            base.SetFocus(interactor, interactible);
            view.transform.SetParent(interactible.transform);
            view.transform.localPosition = new Vector3(0f, 2f, 0f);
        }

        protected override void RemoveFocus()
        {
            base.RemoveFocus();
            view.transform.SetParent(defaultViewParent);
        }
    }
}