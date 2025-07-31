using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.Environment
{
    public class Openable : RotatableBase
    {
        [SerializeField] private bool startClosed = true;
        [SerializeField] private bool invertRotation = false;
        [SerializeField] private float openMinRotation = 50f;
        [SerializeField] private float closedMaxRotation = 90f;

        [SerializeField, Space] private UnityEvent OnOpen = default;
        [SerializeField] private UnityEvent OnClose = default;

        private bool isOpen = false;

        private InteractionCollection openInteraction = default;
        private InteractionCollection closeInteraction = default;

        protected override void Awake()
        {
            openInteraction = new InteractionCollection(new VisibleInteraction("Close", 1, DoInteract));
            closeInteraction = new InteractionCollection(new VisibleInteraction("Open", 1, DoInteract));
            isOpen = !startClosed;
            base.Awake();
        }

        public override InteractionCollection GetInteractions(IInteractor interactor)
        {
            return isOpen
                ? closeInteraction
                : openInteraction;
        }

        protected override EDirection GetNextTargetDirection()
        {
            return isOpen
                ? !invertRotation ? EDirection.Clockwise : EDirection.Anticlockwise
                : !invertRotation ? EDirection.Anticlockwise : EDirection.Clockwise;
        }

        protected override float GetNextTargetAngle()
        {
            return isOpen ? closedMaxRotation : openMinRotation;
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            isOpen = !isOpen;

            if (isOpen) { OnOpen?.Invoke(); }
            else { OnClose?.Invoke(); }
        }
    }
}
