using GMTK2025.Inputs;
using KinematicCharacterController.Examples;
using UnityEngine;

namespace GMTK2025.Characters
{
    public class PlayerCharacter : ExampleCharacterController
    {
        public IPlayerInput Input { get; set; }
        public Transform CameraTransform { get; set; }

        private void Update()
        {
            if (Input == null || CameraTransform == null) { return; }

            HandleCharacterInput();
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.Forward;
            characterInputs.MoveAxisRight = Input.Right;
            characterInputs.CameraRotation = CameraTransform.rotation;
            characterInputs.JumpDown = Input.Jump;
            characterInputs.CrouchDown = Input.CrouchDown;
            characterInputs.CrouchUp = Input.CrouchUp;

            SetInputs(ref characterInputs);
        }
    }
}