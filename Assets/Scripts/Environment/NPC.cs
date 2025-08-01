using GMTK2025.App;
using GMTK2025.GameLoop;
using UnityEngine;
using Yarn.Unity;

namespace GMTK2025.Environment
{
    public class NPC : InteractableDialogueObject
    {
        [SerializeField] private NPCNameGender gender = default;

        public NPCNameGender Gender => gender;
        public NPCProfile Profile { get; set; }

        protected override void OnDialogueStarted()
        {
            dialogue.VariableStorage.SetValue("$npcName", Profile.name);
            GameState.CURRENT_DIALOGUE = this;
        }
    }
}
