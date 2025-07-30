using GMTK2025.Characters;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Attributes;

namespace GMTK2025.Environment
{
    public class Door : InteractableObject
    {
        [SerializeField] private DialogueRunner dialogue = default;
        [SerializeField] private Transform destination = default;
        [SerializeField] private YarnProject yarnProject = default;
        [SerializeField, YarnNode(nameof(yarnProject))]
        private string startNode = "Start";

        private IInteractor interactor = default;

        protected override void OnInteracted(IInteractor interactor)
        {
            this.interactor = interactor;
            dialogue.StartDialogue(startNode);
        }

        [YarnCommand("exit")]
        public void Exit()
        {
            if (interactor == null) { return; }

            interactor.transform.GetComponent<PlayerCharacter>().TeleportToPosition(destination.position);
            interactor = null;
        }
    }
}
