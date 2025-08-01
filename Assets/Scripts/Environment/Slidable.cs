using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.Environment
{
    public class Slidable : InteractableObject
    {
        [System.Serializable]
        public class Config
        {
            public float Speed = 1;
            public bool StartClosed = true;
        }

        private const float SPEED_CONSTANT = 10f;

        [SerializeField] protected Transform target = default;
        [SerializeField] protected Transform startPos = default;
        [SerializeField] protected Transform endPos = default;
        [SerializeField] protected Config config = default;

        [SerializeField, Space] private UnityEvent OnOpen = default;
        [SerializeField] private UnityEvent OnClose = default;

        private bool isOpen = false;
        private Vector3? targetPosition = default;
        private float? speed = default;

        private InteractionCollection openInteraction = default;
        private InteractionCollection closeInteraction = default;

        protected override void Awake()
        {
            openInteraction = new InteractionCollection(new VisibleInteraction("Close", 1, DoInteract));
            closeInteraction = new InteractionCollection(new VisibleInteraction("Open", 1, DoInteract));

            isOpen = !config.StartClosed;
            target.position = GetNextPosition();

            base.Awake();
        }

        public override InteractionCollection GetInteractions(IInteractor interactor)
        {
            return isOpen
                ? closeInteraction
                : openInteraction;
        }

        public void Open(float speed)
        {
            ChangeState(true, speed);
        }

        public void Close(float speed)
        {
            ChangeState(false, speed);
        }

        public void ChangeState(bool newState, float speed)
        {
            this.speed = speed;
            isOpen = newState;
            targetPosition = GetNextPosition();
        }

        private Vector3 GetNextPosition()
        {
            return isOpen
                ? startPos.position
                : endPos.position;
        }

        protected override void OnInteracted(IInteractor interactor)
        {
            if (!targetPosition.HasValue)
            {
                ChangeState(!isOpen, config.Speed);
            }
        }

        protected virtual void DoSlide()
        {
            float step = speed.Value * SPEED_CONSTANT * Time.deltaTime;
            target.position = Vector3.MoveTowards(target.position, targetPosition.Value, step);

            if (Vector3.Distance(target.position, targetPosition.Value) < 0.001f)
            {
                OnComplete();
            }
        }

        protected void OnComplete()
        {
            target.position = targetPosition.Value;            
            targetPosition = null;
            speed = null;

            if (isOpen) { OnOpen?.Invoke(); }
            else { OnClose?.Invoke(); }
        }

        protected virtual bool HasCompleted(float angleMoved)
        {
            return Vector3.Distance(target.position, targetPosition.Value) < 0.001f;
        }

        protected virtual void Update()
        {
            if (targetPosition.HasValue)
            {
                DoSlide();
            }
        }
    }
}
