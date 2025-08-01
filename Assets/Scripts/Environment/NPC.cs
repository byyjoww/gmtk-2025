using UnityEngine;
using Yarn.Unity;

namespace GMTK2025.Environment
{
    public class NPC : InteractableDialogueObject 
    {
        [SerializeField] private NPCNameGender gender = default;

        public NPCNameGender Gender => gender;
    }
}
