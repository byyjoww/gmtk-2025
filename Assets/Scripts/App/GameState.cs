using GMTK2025.Cameras;
using GMTK2025.Characters;
using GMTK2025.Environment;
using GMTK2025.GameLoop;
using GMTK2025.Inputs;
using SLS.Core.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace GMTK2025.App
{
    public class GameState
    {
        public static NPC CURRENT_DIALOGUE = default;

        private const int STARTING_UNITS = 100;
        private const int STARTING_QUOTA_UNITS = 3;
        private const int MAX_NPC_COUNT = 10;
        private const int QUOTA_UNIT_VALUE = 10;

        private PlayerCharacter character = default;
        private PlayerCamera camera = default;
        private PlayerInput input = default;
        private Train train = default;
        private DialogueRunner dialogue = default;
        private Wallet wallet = default;
        private Wallet collected = default;
        private Wallet quota = default;
        private LoopFactory loopFactory = default;
        private Slidable carriageEntrance = default;
        private Door carriageExit = default;
        private Transform startingPos = default;

        private Loop Current { get; set; }
        private int LoopIndex { get; set; }
        private int NumOfNPCsInTrain { get; set; }
        private HashSet<NPCProfile> KnownNpcs { get; set; }

        public event UnityAction OnStart;
        public event UnityAction OnLose;

        public GameState(PlayerCharacter character, PlayerCamera camera, PlayerInput input, Train train, DialogueRunner dialogue, Wallet wallet, Wallet collected, Wallet quota,
            LoopFactory loopFactory, Slidable carriageEntrance, Door carriageExit, Transform startingPos)
        {
            this.character = character;
            this.camera = camera;
            this.input = input;
            this.train = train;
            this.dialogue = dialogue;
            this.wallet = wallet;
            this.collected = collected;
            this.quota = quota;
            this.loopFactory = loopFactory;
            this.carriageEntrance = carriageEntrance;
            this.carriageExit = carriageExit;
            this.startingPos = startingPos;

            carriageEntrance.OnInteract.AddListener(OnExitWaitingRoom);
            dialogue.onDialogueStart.AddListener(OnDialogueStarted);
            dialogue.onDialogueComplete.AddListener(OnDialogueEnded);
            dialogue.AddCommandHandler("collect", OnCollectMoneyFromNPC);
            dialogue.AddCommandHandler("kick", OnKickNPCFromTrain);
            dialogue.AddCommandHandler("exit", OnExitCarriage);
            EndGame();
        }

        public void StartGame()
        {
            Reset();
            camera.Lock();
            input.Enable();
            OnStart?.Invoke();
        }

        private void EndGame()
        {
            input.Disable();
            camera.Unlock();
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
            if (FinishLoop())
            {
                character.TeleportToPosition(carriageExit.Destination);
                StartLoop();
            }
        }

        private void OnCollectMoneyFromNPC()
        {
            CURRENT_DIALOGUE.IsEnabled = false;
            collected.Add(QUOTA_UNIT_VALUE);
        }

        private void OnKickNPCFromTrain()
        {
            CURRENT_DIALOGUE.IsEnabled = false;
            NumOfNPCsInTrain--;
        }

        private void OnDialogueStarted()
        {
            
        }

        private void OnDialogueEnded()
        {
            CURRENT_DIALOGUE = null;
        }                

        private void StartLoop()
        {
            int numOfKnownNPCsToUse = GetNumOfKnownNPCsToSpawn(LoopIndex);
            quota.Set(GetQuotaUnitsForLoop(LoopIndex, MAX_NPC_COUNT) * QUOTA_UNIT_VALUE);
            train.SetCarriageName(GetCarriageName(LoopIndex));
            Current = loopFactory.Create(NumOfNPCsInTrain, numOfKnownNPCsToUse, KnownNpcs);
            Current.NPCs.ForEach(x => KnownNpcs.Add(x));
        }

        private bool FinishLoop()
        {
            if (Current == null) { return true; }

            int debt = quota.Current - collected.Current;
            if (!wallet.Remove(debt))
            {
                EndGame();
                OnLose?.Invoke();
                return false;
            }

            NumOfNPCsInTrain -= Current.Kicked;
            LoopIndex++;

            for (int i = Current.NPCs.Length; i-- > 0;)
            {
                var profile = Current.NPCs[i];
                Debug.Log($"clearing NPC {profile.npc.name}");
                GameObject.Destroy(profile.npc.gameObject);
            }

            quota.Clear();
            collected.Clear();
            Current = null;
            return true;
        }

        private void Reset()
        {
            character.TeleportToPosition(startingPos.position, startingPos.rotation);
            camera.SetRotation(character.transform.forward);
            carriageEntrance.Close(100);

            Current = null;
            LoopIndex = 0;
            NumOfNPCsInTrain = MAX_NPC_COUNT;
            KnownNpcs = new HashSet<NPCProfile>();
            quota.Clear();
            collected.Clear();
            wallet.Set(STARTING_UNITS * QUOTA_UNIT_VALUE);
        }

        private static int GetQuotaUnitsForLoop(int loopIndex, int maxQuotaUnits)
        {
            int units = STARTING_QUOTA_UNITS + (Mathf.FloorToInt((float)loopIndex / 3f));
            return Mathf.Min(units, maxQuotaUnits);
        }

        private static string GetCarriageName(int loopIndex)
        {
            string identifier = string.Empty;
            var loopNumber = loopIndex + 1;

            while (loopNumber > 0)
            {
                int modulo = (loopNumber - 1) % 26;
                identifier = Convert.ToChar('A' + modulo) + identifier;
                loopNumber = (loopNumber - modulo) / 26;
            }

            return $"Carriage {identifier}";
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