using GMTK2025.Cameras;
using GMTK2025.Environment;
using GMTK2025.Inputs;
using GMTK2025.UI;
using KinematicCharacterController.Examples;
using SLS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.Characters
{
    public class PlayerCharacter : ExampleCharacterController, IInteractor, IInteractionModel, ITicker
    {
        [Header("Interaction")]
        [SerializeField] private InteractionChecker.Config interaction = new InteractionChecker.Config();

        private IPlayerInput input = default;
        private Transform camTransform = default;
        private InteractionChecker interactionChecker = default;

        private List<ITickable> tickables = new List<ITickable>();

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

        public void Setup(IPlayerInput input, PlayerCamera camera)
        {
            this.input = input;
            this.camTransform = camera.Transform;

            interactionChecker = new InteractionChecker(this, input, this, interaction);            
        }

        private void Update()
        {
            for (int i = tickables.Count; i-- > 0;)
            {
                tickables[i].OnTick();
            }

            if (input != null && camTransform != null) 
            {
                HandleCharacterInput();
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = input.Forward;
            characterInputs.MoveAxisRight = input.Right;
            characterInputs.CameraRotation = camTransform.rotation;
            characterInputs.JumpDown = input.Jump;
            characterInputs.CrouchDown = input.CrouchDown;
            characterInputs.CrouchUp = input.CrouchUp;

            SetInputs(ref characterInputs);
        }

        public void TeleportToPosition(Vector3 position)
        {
            Motor.MoveCharacter(position);
        }

        public void RegisterOnTick(ITickable tickable)
        {
            tickables.Add(tickable);
        }

        public void DeregisterOnTick(ITickable tickable)
        {
            tickables.Remove(tickable);
        }

        private void OnDestroy()
        {
            interactionChecker?.Dispose();
        }
    }
}