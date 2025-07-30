using GMTK2025.Characters;
using GMTK2025.Inputs;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine;

namespace GMTK2025.Cameras
{
    public class PlayerCamera : ExampleCharacterCamera
    {
        private IPlayerInput input = default;
        private KinematicCharacterMotor motor = default;     

        public void Setup(IPlayerInput input, PlayerCharacter character)
        {
            this.input = input;
            this.motor = character.Motor;

            Cursor.lockState = CursorLockMode.Locked;
            SetFollowTransform(character.CameraFollowPoint);
            IgnoredColliders.Clear();
            IgnoredColliders.AddRange(character.GetComponentsInChildren<Collider>());
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (RotateWithPhysicsMover && motor != null)
            {
                PlanarDirection = motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * PlanarDirection;
                PlanarDirection = Vector3.ProjectOnPlane(PlanarDirection, motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            if (input == null) { return; }

            // Create the look input vector for the camera
            float mouseLookAxisUp = input.MouseLookUp;
            float mouseLookAxisRight = input.MouseLookRight;
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = input.MouseScroll;
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (input.MouseSecondary)
            {
                TargetDistance = (TargetDistance == 0f) ? DefaultDistance : 0f;
            }
        }
    }
}