using GMTK2025.Environment;
using SLS.Core;
using UnityEngine;

namespace GMTK2025.UI
{
    public class InteractionViewController : BaseInteractionViewController, ITickable
    {
        private ITicker ticker = default;
        private Transform interactible = default;

        public InteractionViewController(IInteractionModel[] models, InteractionView view, IInteractionInput input, ITicker ticker) : base(models, view, input)
        {
            this.ticker = ticker;
        }

        protected override void SetFocus(IInteractor interactor, IInteractable interactible)
        {
            base.SetFocus(interactor, interactible);
            this.interactible = interactible.transform;
            UpdatePosition();
            ticker.RegisterOnTick(this);
        }

        protected override void RemoveFocus()
        {
            ticker.DeregisterOnTick(this);
            interactible = null;
            base.RemoveFocus();
        }

        private void UpdatePosition()
        {
            if (!interactible || interactible == null) { return; }
            var worldPos = interactible.position;
            var screenPos = Camera.main.WorldToScreenPoint(worldPos);
            view.transform.position = screenPos;
        }

        public override void Dispose()
        {
            ticker.DeregisterOnTick(this);
            base.Dispose();
        }

        void ITickable.OnTick()
        {
            UpdatePosition();
        }
    }
}