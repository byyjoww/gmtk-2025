using GMTK2025.Characters;
using UnityEngine;

namespace GMTK2025.Environment
{
    public class Door : InteractableObject
    {
        [SerializeField] private Transform destination = default;

        protected override bool DoInteract(IInteractor interactor)
        {
            if (base.DoInteract(interactor))
            {
                interactor.transform.GetComponent<PlayerCharacter>().TeleportToPosition(destination.position);
                return true;
            }

            return false;
        }
    }
}
