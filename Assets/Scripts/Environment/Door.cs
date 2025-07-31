using UnityEngine;
using Yarn.Unity;

namespace GMTK2025.Environment
{
    public class Door : InteractableDialogueObject
    {
        [SerializeField] private Transform destination = default;
        
        public Vector2 Destination => destination.position;

        [YarnCommand("exit")]
        public void Exit()
        {
            onConfirm?.Invoke();
        }
    }
}
