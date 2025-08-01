using GMTK2025.Cameras;
using GMTK2025.Environment;
using GMTK2025.Inputs;
using GMTK2025.UI;
using KinematicCharacterController.Examples;
using SLS.Core;
using SLS.Core.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace GMTK2025.Characters
{
    public class PlayerCharacter : ExampleCharacterController, IInteractor, IInteractionModel, ITicker
    {
        [Header("Interaction")]
        [SerializeField] private InteractionChecker.Config interaction = new InteractionChecker.Config();

        private IPlayerInput input = default;
        private Transform camTransform = default;
        private InteractionChecker interactionChecker = default;
        private DialogueRunner dialogue = default;        

        private List<ITickable> tickables = new List<ITickable>();
        private bool movementEnabled = true;

        public event UnityAction<IInteractor, IInteractable> OnFocus
        {
            add => interactionChecker.OnFocus += value;
            remove => interactionChecker.OnFocus -= value;
        }

        public event UnityAction OnDefocus
        {
            add => interactionChecker.OnDefocus += value;
            remove => interactionChecker.OnDefocus -= value;
        }

        public void Setup(IPlayerInput input, PlayerCamera camera, DialogueRunner dialogue)
        {
            this.input = input;
            this.camTransform = camera.Transform;
            this.dialogue = dialogue;

            interactionChecker = new InteractionChecker(this, input, this, interaction);
            dialogue.onDialogueStart.AddListener(OnDialogueStarted);
            dialogue.onDialogueComplete.AddListener(OnDialogueEnded);
        }

        public void TeleportToPosition(Vector3 position)
        {
            TeleportToPosition(position, transform.rotation);
        }

        public void TeleportToPosition(Vector3 position, Quaternion rotation)
        {
            Motor.SetPositionAndRotation(position, rotation);
        }

        public void RegisterOnTick(ITickable tickable)
        {
            tickables.Add(tickable);
        }

        public void DeregisterOnTick(ITickable tickable)
        {
            tickables.Remove(tickable);
        }

        private void Update()
        {
            for (int i = tickables.Count; i-- > 0;)
            {
                tickables[i].OnTick();
            }

            if (input != null && input.IsEnabled && camTransform != null && movementEnabled) 
            {
                HandleCharacterInput();
            }
        }

        private void HandleCharacterInput()
        {
            Vector3 rot = transform.eulerAngles.SetY(camTransform.eulerAngles.y);
            Quaternion quat = Quaternion.Euler(rot);
            Motor.RotateCharacter(quat);

            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs
            {
                // Build the CharacterInputs struct
                MoveAxisForward = input.Forward,
                MoveAxisRight = input.Right,
                CameraRotation = camTransform.rotation,
                JumpDown = input.Jump,
                CrouchDown = input.CrouchDown,
                CrouchUp = input.CrouchUp,
            };

            SetInputs(ref characterInputs);
        }        

        private void OnDialogueStarted()
        {
            interactionChecker.Disable();
            movementEnabled = false;

            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs
            {
                MoveAxisForward = 0f,
                MoveAxisRight = 0f,
                CameraRotation = camTransform.rotation,
                JumpDown = false,
                CrouchDown = false,
                CrouchUp = false,
            };

            SetInputs(ref characterInputs);
        }

        private void OnDialogueEnded()
        {
            interactionChecker.Enable();
            movementEnabled = true;
        }

        private void OnDestroy()
        {
            interactionChecker?.Dispose();
            dialogue.onDialogueStart?.RemoveListener(OnDialogueStarted);
            dialogue.onDialogueComplete?.RemoveListener(OnDialogueEnded);
        }
    }
}