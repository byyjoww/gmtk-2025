using GMTK2025.Characters;
using GMTK2025.Environment;
using GMTK2025.GameLoop;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.App
{
    public class GameState
    {
        private const int STARTING_QUOTA_UNITS = 3;
        private const int MAX_NPC_COUNT = 10;
        private const int QUOTA_UNIT_VALUE = 10;

        private PlayerCharacter character = default;
        private LoopFactory loopFactory = default;
        private InteractableObject carriageEntrance = default;
        private Door carriageExit = default;

        private Loop Current { get; set; }
        private int LoopIndex { get; set; } = 0;
        private int NpcCount { get; set; } = MAX_NPC_COUNT;

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
            int quota = GetQuotaUnitsForLoop(LoopIndex, MAX_NPC_COUNT) * QUOTA_UNIT_VALUE;
            Current = loopFactory.Create(NpcCount, quota);
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

            NpcCount -= Current.Kicked;
            LoopIndex++;
            Current = null;
            return true;
        }

        private static int GetQuotaUnitsForLoop(int loopIndex, int maxQuotaUnits)
        {
            int units = STARTING_QUOTA_UNITS + (Mathf.FloorToInt((float)loopIndex / 3f));
            return Mathf.Min(units, maxQuotaUnits);
        }
    }
}