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

        protected DialogueRunner dialogue = default;

        protected virtual string StartNode => startNode;

        public void Setup(DialogueRunner dialogue)
        {
            this.dialogue = dialogue;
        }

        protected override void OnInteracted(IInteractor interactor)
        {
            if (StartNode == NO_DIALOGUE)
            {
                return;
            }

            OnDialogueStarted();
            dialogue.StartDialogue(StartNode);
        }

        protected virtual void OnDialogueStarted()
        {

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
