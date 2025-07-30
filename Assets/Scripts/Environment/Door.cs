using GMTK2025.Characters;
using UnityEngine;

namespace GMTK2025.Environment
{
    public class Door : InteractableObject
    {
        [SerializeField] private Transform destination = default;

        protected override void OnInteracted(IInteractor interactor)
        {
            interactor.transform.GetComponent<PlayerCharacter>().TeleportToPosition(destination.position);
        }
    }
}
