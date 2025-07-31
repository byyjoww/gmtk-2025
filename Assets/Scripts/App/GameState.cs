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
        private Door waitingRoomExit = default;
        private Door carriageExit = default;

        private Loop Current { get; set; }

        public event UnityAction OnLose;

        public GameState(PlayerCharacter character, LoopFactory loopFactory, Door waitingRoomExit, Door carriageExit)
        {
            this.character = character;
            this.loopFactory = loopFactory;
            this.waitingRoomExit = waitingRoomExit;
            this.carriageExit = carriageExit;

            carriageExit.AddOnConfirm(OnExitCarriage);
            waitingRoomExit.AddOnConfirm(OnExitWaitingRoom);
        }

        private void OnExitWaitingRoom()
        {
            character.TeleportToPosition(waitingRoomExit.Destination);
            StartLoop();
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