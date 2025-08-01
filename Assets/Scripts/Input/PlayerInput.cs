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

        private bool isEnabled = default;

        public float Forward => isEnabled ? Input.GetAxisRaw(VerticalInput) : 0f;
        public float Right => isEnabled ? Input.GetAxisRaw(HorizontalInput) : 0f;
        public bool Jump => isEnabled && Input.GetKeyDown(KeyCode.Space);
        public bool CrouchDown => isEnabled && Input.GetKeyDown(KeyCode.C);
        public bool CrouchUp => isEnabled && Input.GetKeyUp(KeyCode.C);
        public bool InteractDown => isEnabled && Input.GetKeyDown(KeyCode.F);
        public bool InteractUp => isEnabled && Input.GetKeyUp(KeyCode.F);
        public float MouseLookUp => isEnabled ? Input.GetAxisRaw(MouseYInput) : 0f;
        public float MouseLookRight => isEnabled ? Input.GetAxisRaw(MouseXInput) : 0f;
        public float MouseScroll => isEnabled ? -Input.GetAxis(MouseScrollInput) : 0f;
        public bool MousePrimary => isEnabled && Input.GetMouseButtonDown(0);
        public bool MouseSecondary => isEnabled && Input.GetMouseButtonDown(1);

        public event UnityAction<int> OnInteractDown;
        public event UnityAction<int> OnInteractUp;
        public event UnityAction OnControlSchemeChanged;

        public string GetInteractionInput(int trigger)
        {
            return "F";
        }

        public void Enable()
        {
            isEnabled = true;
        }

        public void Disable()
        {
            isEnabled = false;
        }

        private void Update()
        {
            if (!isEnabled) { return; }

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