using GMTK2025.Characters;
using GMTK2025.Environment;
using GMTK2025.GameLoop;
using NUnit.Framework;
using SLS.Core.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.App
{
    public class GameState
    {
        private const int STARTING_UNITS = 10;
        private const int STARTING_QUOTA_UNITS = 3;
        private const int MAX_NPC_COUNT = 10;
        private const int QUOTA_UNIT_VALUE = 10;

        private PlayerCharacter character = default;
        private Wallet wallet = default;
        private Wallet collected = default;
        private Wallet quota = default;
        private LoopFactory loopFactory = default;
        private InteractableObject carriageEntrance = default;
        private Door carriageExit = default;

        private Loop Current { get; set; }
        private int LoopIndex { get; set; } = 0;
        private int NumOfNPCsInTrain { get; set; } = MAX_NPC_COUNT;        
        private HashSet<NPCProfile> KnownNpcs { get; set; } = new HashSet<NPCProfile>();

        public event UnityAction OnLose;

        public GameState(PlayerCharacter character, Wallet wallet, Wallet collected, Wallet quota, LoopFactory loopFactory, InteractableObject carriageEntrance, Door carriageExit)
        {
            this.character = character;
            this.wallet = wallet;
            this.collected = collected;
            this.quota = quota;
            this.loopFactory = loopFactory;
            this.carriageEntrance = carriageEntrance;
            this.carriageExit = carriageExit;

            carriageExit.AddOnConfirm(OnExitCarriage);
            carriageEntrance.OnInteract.AddListener(OnExitWaitingRoom);
            wallet.Add(STARTING_UNITS * QUOTA_UNIT_VALUE);
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
            int numOfKnownNPCsToUse = GetNumOfKnownNPCsToSpawn(LoopIndex);
            quota.Set(GetQuotaUnitsForLoop(LoopIndex, MAX_NPC_COUNT));
            Current = loopFactory.Create(NumOfNPCsInTrain, numOfKnownNPCsToUse, KnownNpcs);
            Current.NPCs.ForEach(x => KnownNpcs.Add(x));
        }

        private bool FinishLoop()
        {
            if (Current == null) { return true; }

            int debt = quota.Current - collected.Current;
            if (!wallet.Remove(debt))
            {
                OnLose?.Invoke();
                return false;
            }

            NumOfNPCsInTrain -= Current.Kicked;
            LoopIndex++;

            for (int i = Current.NPCs.Length; i-- > 0;)
            {
                var profile = Current.NPCs[i];
                GameObject.Destroy(profile.npc.gameObject);
            }

            quota.Clear();
            collected.Clear();
            Current = null;
            return true;
        }

        private static int GetQuotaUnitsForLoop(int loopIndex, int maxQuotaUnits)
        {
            int units = STARTING_QUOTA_UNITS + (Mathf.FloorToInt((float)loopIndex / 3f));
            return Mathf.Min(units, maxQuotaUnits);
        }

        private static int GetNumOfKnownNPCsToSpawn(int loopIndex)
        {
            if (loopIndex == 0)
            {
                return 0;
            }

            if (loopIndex >= 4)
            {
                return 10;
            }

            return 5;
        }
    }
}