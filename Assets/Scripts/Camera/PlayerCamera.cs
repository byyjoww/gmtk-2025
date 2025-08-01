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
        private int frameCount = 0;

        public void Setup(IPlayerInput input, PlayerCharacter character)
        {
            this.input = input;
            this.motor = character.Motor;            
            SetFollowTransform(character.CameraFollowPoint);
            IgnoredColliders.Clear();
            IgnoredColliders.AddRange(character.GetComponentsInChildren<Collider>());            
        }

        public void Lock()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Unlock()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        public void SetRotation(Vector3 eulerProjectedOnPlane)
        {
            PlanarDirection = eulerProjectedOnPlane;
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (RotateWithPhysicsMover && motor != null)
            {
                PlanarDirection = motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * PlanarDirection;
                PlanarDirection = Vector3.ProjectOnPlane(PlanarDirection, motor.CharacterUp).normalized;
            }

            if (frameCount < 5)
            {
                frameCount++;
                return;
            }

            var input = HandleCameraInput();
            UpdateWithInput(Time.deltaTime, input.scroll, input.look);
        }

        private (float scroll, Vector3 look) HandleCameraInput()
        {
            if (input == null || !input.IsEnabled) 
            {
                return (0f, Vector3.zero); 
            }
            
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
            // Handle toggling zoom level
            if (input.MouseSecondary)
            {
                TargetDistance = (TargetDistance == 0f) ? DefaultDistance : 0f;
            }

            return (scrollInput, lookInputVector);
        }
    }
}