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

        [Header("Audio")]
        [SerializeField] private AudioClip[] trainTrackSfx = default;

        [Header("Components")]
        [SerializeField] private PlayerCharacter character = default;
        [SerializeField] private new PlayerCamera camera = default;
        [SerializeField] private PlayerInput input = default;
        [SerializeField] private DialogueRunner dialogue = default;
        [SerializeField] private Slidable carriageEntrance = default;
        [SerializeField] private Door carriageExit = default;
        [SerializeField] private Train train = default;
        [SerializeField] private Transform startingPos = default;
        [SerializeField] private AudioSource trainTrackAudioSource = default;
        [SerializeField] private InteractableDialogueObject[] dialogues = default;

        [Header("UI")]
        [SerializeField] private InteractionView interactionView = default;
        [SerializeField] private QuotaView quotaView = default;
        [SerializeField] private StartGameView startGameView = default;
        [SerializeField] private EndGameView endGameView = default;

        private LoopFactory loopFactory = default;
        private GameState gameState = default;
        private Wallet wallet = default;
        private Wallet collected = default;
        private Wallet quota = default;

        // view controllers
        private InteractionViewController interactionViewController = default;
        private QuotaViewController quotaViewController = default;
        private StartGameViewController startGameViewController = default;
        private EndGameViewController endGameViewController = default;

        private List<ITickable> tickables = new List<ITickable>();

        public void Start() => Init();

        public void OnApplicationQuit() => Terminate();

        private void Init()
        {
            dialogues.ForEach(x => x.Setup(dialogue));
            loopConfig.npcs.ForEach(x => x.Setup(dialogue));

            character.Setup(input, camera, dialogue);
            camera.Setup(input, character);

            loopFactory = new LoopFactory(character, dialogue, collected, train.SpawnLocations, loopConfig);
            wallet = new Wallet(0);
            collected = new Wallet(0);
            quota = new Wallet(0);
            gameState = new GameState(character, camera, input, train, dialogue, wallet, collected, quota, loopFactory, carriageEntrance, carriageExit, startingPos, trainTrackSfx, trainTrackAudioSource);

            interactionViewController = new InteractionViewController(new IInteractionModel[] { character }, interactionView, input, this);            
            
            quotaViewController = new QuotaViewController(quotaView, wallet, quota, collected, gameState);
            quotaViewController?.Init();

            startGameViewController = new StartGameViewController(startGameView, gameState);
            startGameViewController?.Init();

            endGameViewController = new EndGameViewController(endGameView, gameState);
            endGameViewController?.Init();
        }

        private void Terminate()
        {
            interactionViewController?.Dispose();
            quotaViewController?.Dispose();
            startGameViewController?.Dispose();
            endGameViewController?.Dispose();
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