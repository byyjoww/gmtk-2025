using GMTK2025.Inputs;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine;

namespace GMTK2025.Cameras
{
    public class PlayerCamera : ExampleCharacterCamera
    {
        public IPlayerInput Input { get; set; }
        public KinematicCharacterMotor Motor { get; set; }
        public Vector3 Up { get; set; }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (RotateWithPhysicsMover && Motor != null)
            {
                PlanarDirection = Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * PlanarDirection;
                PlanarDirection = Vector3.ProjectOnPlane(PlanarDirection, Up).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            if (Input == null) { return; }

            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.MouseLookUp;
            float mouseLookAxisRight = Input.MouseLookRight;
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = Input.MouseScroll;
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.MouseSecondary)
            {
                TargetDistance = (TargetDistance == 0f) ? DefaultDistance : 0f;
            }
        }
    }
}