using GMTK2025.UI;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.Inputs
{
    public class PlayerInput : MonoBehaviour, IPlayerInput, IInteractionInput
    {
        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        public float Forward => Input.GetAxisRaw(VerticalInput);
        public float Right => Input.GetAxisRaw(HorizontalInput);
        public bool Jump => Input.GetKeyDown(KeyCode.Space);
        public bool CrouchDown => Input.GetKeyDown(KeyCode.C);
        public bool CrouchUp => Input.GetKeyUp(KeyCode.C);
        public bool InteractDown => Input.GetKeyDown(KeyCode.F);
        public bool InteractUp => Input.GetKeyUp(KeyCode.F);

        public float MouseLookUp => Input.GetAxisRaw(MouseYInput);
        public float MouseLookRight => Input.GetAxisRaw(MouseXInput);
        public float MouseScroll => -Input.GetAxis(MouseScrollInput);
        public bool MousePrimary => Input.GetMouseButtonDown(0);
        public bool MouseSecondary => Input.GetMouseButtonDown(1);        

        public event UnityAction<int> OnInteractDown;
        public event UnityAction<int> OnInteractUp;
        public event UnityAction OnControlSchemeChanged;

        public string GetInteractionInput(int trigger)
        {
            return "F";
        }

        private void Update()
        {
            if (MousePrimary)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (InteractDown)
            {
                OnInteractDown?.Invoke(1);
            }

            if (InteractUp)
            {
                OnInteractUp?.Invoke(1);
            }
        }
    }
}