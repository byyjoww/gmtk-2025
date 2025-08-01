using GMTK2025.Cameras;
using GMTK2025.Characters;
using GMTK2025.Environment;
using GMTK2025.GameLoop;
using GMTK2025.Inputs;
using GMTK2025.UI;
using SLS.Core;
using SLS.Core.Extensions;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace GMTK2025.App
{
    public class App : MonoBehaviour, ITicker
    {
        [Header("Config")]
        [SerializeField] private LoopFactory.Config loopConfig = new LoopFactory.Config();

        [Header("Components")]
        [SerializeField] private PlayerCharacter character = default;
        [SerializeField] private new PlayerCamera camera = default;
        [SerializeField] private PlayerInput input = default;
        [SerializeField] private DialogueRunner dialogue = default;
        [SerializeField] private InteractableObject carriageEntrance = default;
        [SerializeField] private Door carriageExit = default;
        [SerializeField] private Train train = default;
        [SerializeField] private InteractableDialogueObject[] dialogues = default;
        [SerializeField] private NPC[] npcs = default;                

        [Header("UI")]
        [SerializeField] private InteractionView interactionView = default;
        [SerializeField] private QuotaView quotaView = default;

        private LoopFactory loopFactory = default;
        private GameState gameState = default;
        private Wallet wallet = default;
        private Wallet collected = default;
        private Wallet quota = default;

        // view controllers
        private InteractionViewController interactionViewController = default;
        private QuotaViewController quotaViewController = default;

        private List<ITickable> tickables = new List<ITickable>();

        public void Start() => Init();

        public void OnApplicationQuit() => Terminate();

        private void Init()
        {
            dialogues.ForEach(x => x.Setup(dialogue));
            npcs.ForEach(x => x.Setup(dialogue));

            character.Setup(input, camera, dialogue);
            camera.Setup(input, character);

            loopFactory = new LoopFactory(character, dialogue, collected, npcs, train.SpawnLocations, loopConfig);
            wallet = new Wallet(0);
            collected = new Wallet(0);
            quota = new Wallet(0);
            gameState = new GameState(character, train, dialogue, wallet, collected, quota, loopFactory, carriageEntrance, carriageExit);

            interactionViewController = new InteractionViewController(new IInteractionModel[] { character }, interactionView, input, this);
            
            quotaViewController = new QuotaViewController(quotaView, wallet, collected, quota);
            quotaViewController?.Init();
        }

        private void Terminate()
        {
            interactionViewController?.Dispose();
            quotaViewController?.Dispose();
        }

        public void RegisterOnTick(ITickable tickable)
        {
            tickables.Add(tickable);
        }

        public void DeregisterOnTick(ITickable tickable)
        {
            tickables.Remove(tickable);
        }

        protected virtual void Update()
        {
            for (int i = tickables.Count; i-- > 0;)
            {
                tickables[i].OnTick();
            }
        }

        private void OnValidate()
        {
            dialogues = FindObjectsByType<InteractableDialogueObject>(FindObjectsSortMode.None);
            // npcs = FindObjectsByType<NPC>(FindObjectsSortMode.None);
        }
    }
}