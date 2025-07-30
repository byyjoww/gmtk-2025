using GMTK2025.Cameras;
using GMTK2025.Characters;
using GMTK2025.Inputs;
using KinematicCharacterController;
using UnityEngine;

namespace GMTK2025.App
{
    public class App : MonoBehaviour
    {
        public PlayerCharacter character = default;
        public new PlayerCamera camera = default;
        public PlayerInput input = default;

        public void Start() => Init();

        public void Init()
        {
            character.Input = input;
            character.CameraTransform = camera.Transform;

            camera.Motor = character.Motor;
            camera.Up = character.Motor.CharacterUp;
            camera.Input = input;

            Cursor.lockState = CursorLockMode.Locked;
            camera.SetFollowTransform(character.CameraFollowPoint);
            camera.IgnoredColliders.Clear();
            camera.IgnoredColliders.AddRange(character.GetComponentsInChildren<Collider>());
        }
    }
}