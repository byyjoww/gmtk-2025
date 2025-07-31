using UnityEngine;

namespace GMTK2025.Environment
{
    public class Transformable : InteractableObject
    {
        [SerializeField] private GameObject defaultState = default;
        [SerializeField] private GameObject transformedState = default;

        protected override void Awake()
        {
            base.Awake();
            SetDefaultState();
        }

        protected override bool DoInteract(IInteractor interactor)
        {
            if (base.DoInteract(interactor))
            {
                if (defaultState.activeSelf) { SetTransformedState(); }
                else { SetDefaultState(); }
                return true;
            }

            return false;
        }

        private void SetDefaultState()
        {
            defaultState.SetActive(true);
            transformedState.SetActive(false);
        }

        private void SetTransformedState()
        {
            defaultState.SetActive(false);
            transformedState.SetActive(true);
        }
    }
}
