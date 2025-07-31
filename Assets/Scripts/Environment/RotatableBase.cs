using SLS.Core.Attributes;
using SLS.Core.Extensions;
using UnityEngine;

namespace GMTK2025.Environment
{
    public abstract class RotatableBase : InteractableObject
    {
        [System.Serializable]
        public class Config
        {
            public int Speed = 1;
            public Vector3 DefaultRotation = Vector3.zero;
        }

        protected enum EDirection
        {
            Undefined = 0,
            Clockwise = 1,
            Anticlockwise = -1,
        }

        private const float SPEED_CONSTANT = 30f;

        [SerializeField] protected Transform target = default;
        [SerializeField] protected Config config = default;
        [SerializeField, ReadOnly] protected bool isRotating = false;

        protected float lastKnownAngle = default;
        protected float targetAngle = default;
        protected EDirection targetDirection = default;
        protected Vector3 targetAxis = default;

        protected virtual float currentAngle => target.localEulerAngles.y;

        protected override void Awake()
        {
            base.Awake();
            target.transform.localRotation = Quaternion.Euler(config.DefaultRotation);
        }

        public override bool CanFocus(IInteractor interactor)
        {
            return base.CanFocus(interactor) && !isRotating;
        }

        protected override bool DoInteract(IInteractor interactor)
        {
            if (base.DoInteract(interactor))
            {
                isRotating = true;
                lastKnownAngle = GetCurrentAngle();
                targetAngle = GetNextTargetAngle();
                targetDirection = GetNextTargetDirection();
                targetAxis = GetNextTargetAxis();
                return true;
            }

            return false;
        }

        protected virtual float GetCurrentAngle()
        {
            return target.localEulerAngles.y;
        }

        protected abstract float GetNextTargetAngle();

        protected abstract EDirection GetNextTargetDirection();

        protected virtual Vector3 GetNextTargetAxis()
        {
            return target.up;
        }

        protected virtual Vector3 GetFinalRotation()
        {
            return target.localEulerAngles.SetY(targetAngle);
        }

        protected virtual void DoRotate(int angleDirection)
        {
            if (angleDirection == 0) { return; }
            var angle = angleDirection * SPEED_CONSTANT * Time.deltaTime;

            if (HasCompleted(angle))
            {
                OnComplete();
            }
            else
            {
                target.Rotate(targetAxis, angle);
                lastKnownAngle = GetCurrentAngle();
            }
        }

        protected virtual void OnComplete()
        {
            target.localRotation = Quaternion.Euler(GetFinalRotation());
            isRotating = false;
            lastKnownAngle = 0;
            targetDirection = EDirection.Undefined;
            targetAngle = 0;
            targetAxis = Vector3.zero;
        }

        protected virtual bool HasCompleted(float angleMoved)
        {
            var angleRemaining = Quaternion.Angle(target.localRotation, Quaternion.Euler(GetFinalRotation()));
            var nextAngleRotation = angleMoved + float.Epsilon;
            return Mathf.Abs(angleRemaining) <= Mathf.Abs(nextAngleRotation);

            //switch (targetDirection)
            //{
            //    case EDirection.Anticlockwise:
            //        return lastKnownAngle > targetAngle
            //            && currentAngle <= targetAngle;
            //    case EDirection.Clockwise: 
            //        return lastKnownAngle < targetAngle
            //            && currentAngle >= targetAngle;
            //    default: 
            //        return true;
            //}
        }

        protected virtual void Update()
        {
            DoRotate((int)targetDirection * config.Speed);
        }
    }
}
