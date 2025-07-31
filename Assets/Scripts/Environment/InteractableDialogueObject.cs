using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;
using Yarn.Unity.Attributes;

namespace GMTK2025.Environment
{
    public class InteractableDialogueObject : InteractableObject
    {
        private const string NO_DIALOGUE = "";

        [SerializeField] private YarnProject yarnProject = default;
        [SerializeField, YarnNode(nameof(yarnProject))]
        private string startNode = NO_DIALOGUE;

        private DialogueRunner dialogue = default;

        protected UnityEvent onConfirm = new UnityEvent();

        public void AddOnConfirm(UnityAction onExit)
        {
            onConfirm.RemoveAllListeners();
            onConfirm.AddListener(onExit);
        }

        public void Setup(DialogueRunner dialogue)
        {
            this.dialogue = dialogue;
        }

        protected override void OnInteracted(IInteractor interactor)
        {
            if (startNode == NO_DIALOGUE)
            {
                return;
            }

            dialogue.StartDialogue(startNode);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onConfirm.RemoveAllListeners();
        }
    }
}
