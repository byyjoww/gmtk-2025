using GMTK2025.Cameras;
using GMTK2025.Characters;
using GMTK2025.Environment;
using GMTK2025.Inputs;
using GMTK2025.UI;
using SLS.Core;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK2025.App
{
    public class App : MonoBehaviour, ITicker
    {
        [Header("Components")]
        [SerializeField] private PlayerCharacter character = default;
        [SerializeField] private new PlayerCamera camera = default;
        [SerializeField] private PlayerInput input = default;
        [SerializeField] private Door exit = default;

        [Header("UI")]
        [SerializeField] private InteractionView interactionView = default;

        // view controllers
        private InteractionViewController interactionViewController = default;

        public void Start() => Init();

        public void OnApplicationQuit() => Terminate();

        private void Init()
        {
            character.Setup(input, camera);
            camera.Setup(input, character);

            interactionViewController = new InteractionViewController(new IInteractionModel[] { character }, interactionView, input, this);
        }

        private void Terminate()
        {

            interactionViewController?.Dispose();
        }

        private List<ITickable> tickables = new List<ITickable>();

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
    }
}