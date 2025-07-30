using GMTK2025.Utils;
using UnityEngine;

namespace GMTK2025.Environment
{
    public class Rotatable : RotatableBase
    {
        [SerializeField] private float rotationAmount = 90f;
        [SerializeField] private EDirection rotationDirection = EDirection.Anticlockwise;

        protected override EDirection GetNextTargetDirection()
        {
            return rotationDirection;
        }

        protected override float GetNextTargetAngle()
        {
            var amount = rotationDirection == EDirection.Clockwise
                ? rotationAmount
                : -rotationAmount;

            return Angle.FromValue(GetCurrentAngle() + amount);
        }

        private void Reset()
        {
            interaction = "Rotate";
        }
    }
}
