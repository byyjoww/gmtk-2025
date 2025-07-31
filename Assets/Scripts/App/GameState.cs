using GMTK2025.Characters;
using GMTK2025.Environment;
using GMTK2025.GameLoop;
using UnityEngine.Events;

namespace GMTK2025.App
{
    public class GameState
    {
        private PlayerCharacter character = default;
        private LoopFactory loopFactory = default;
        private InteractableObject carriageEntrance = default;
        private Door carriageExit = default;

        private Loop Current { get; set; }

        public event UnityAction OnLose;

        public GameState(PlayerCharacter character, LoopFactory loopFactory, InteractableObject carriageEntrance, Door carriageExit)
        {
            this.character = character;
            this.loopFactory = loopFactory;
            this.carriageEntrance = carriageEntrance;
            this.carriageExit = carriageExit;

            carriageExit.AddOnConfirm(OnExitCarriage);
            carriageEntrance.OnInteract.AddListener(OnExitWaitingRoom);
        }

        private void OnExitWaitingRoom()
        {
            if (Current == null)
            {
                StartLoop();
            }            
        }

        private void OnExitCarriage()
        {
            character.TeleportToPosition(carriageExit.Destination);
            if (FinishLoop())
            {
                StartLoop();
            }
        }

        private void StartLoop()
        {
            Current = loopFactory.Create();
        }

        private bool FinishLoop()
        {
            if (Current == null) { return true; }

            int debt = Current.Quota - Current.Accumulated;
            if (!character.Wallet.Remove(debt))
            {
                OnLose?.Invoke();
                return false;
            }

            Current = null;
            return true;
        }
    }
}