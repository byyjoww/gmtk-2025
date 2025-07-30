using GMTK2025.Inputs;
using GMTK2025.UI;
using GMTK2025.Utils;
using SLS.Core;
using SLS.Core.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.Environment
{
    public class InteractionChecker : GMTK2025.Utils.Tickable, IInteractionModel
    {
        [System.Serializable]
        public class Config
        {
            public float InteractRadius = 5f;
            public float InteractFieldOfView = 90f;
        }

        private IDictionary<int, IHoldResolver> holdResolvers = default;
        private IInteractor interactor = default;
        private IPlayerInput input = default;
        private Config config = default;
        private bool isEnabled = true;

        private IInteractable Focus { get; set; }
        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public event UnityAction<IInteractor, IInteractable> OnFocus;
        public event UnityAction OnDefocus;
        public event UnityAction<IInteractable> OnInteractionSuccess;
        public event UnityAction<IInteractable> OnInteractionFail;

        public InteractionChecker(IInteractor interactor, IPlayerInput input, ITicker ticker, Config config) : base(ticker)
        {
            this.interactor = interactor;
            this.input = input;
            this.config = config;
            this.holdResolvers = new Dictionary<int, IHoldResolver>();
            input.OnInteractDown += HandleInteractDown;
            input.OnInteractUp += HandleInteractUp;
        }

        private void HandleInteractDown(int action)
        {
            if (Focus != null)
            {
                var interactions = Focus.GetInteractions(interactor).GetVisibleInteractions(action);
                if (interactions.Length <= 0) { return; }

                void ExecuteInteraction(IInteraction interaction)
                {
                    if (interaction == null) { return; }

                    if (interaction.Execute(interactor))
                    {
                        OnInteractionSuccess?.Invoke(Focus);
                    }
                    else
                    {
                        OnInteractionFail?.Invoke(Focus);
                    }
                }

                var holdInteraction = interactions.FirstOrDefault(x => x is VisibleHoldInteraction) as VisibleHoldInteraction;
                var tapInteraction = interactions.FirstOrDefault(x => !(x is VisibleHoldInteraction)) as VisibleInteraction;
                if (holdInteraction != null)
                {
                    float baseHoldTime = 0.4f;
                    float totalHoldTime = holdInteraction.HoldTime;
                    float interactionHoldTime = totalHoldTime - baseHoldTime;

                    var holdResolver = new HoldResolver(new HoldResolver.Config { HoldThresholdSeconds = baseHoldTime });
                    var holdTimer = Timer.CreateScaledTimer(TimeSpan.FromSeconds(interactionHoldTime));

                    holdTimer.OnEnd.AddListener(delegate
                    {
                        ExecuteInteraction(holdInteraction);
                        Unbind();
                    });

                    holdTimer.OnTick.AddListener(delegate
                    {
                        var progress = ((float)(baseHoldTime + holdTimer.Elapsed.TotalSeconds) / totalHoldTime);
                        holdInteraction.SetProgress(progress);
                    });

                    void Unbind()
                    {
                        holdTimer.Stop();
                        holdTimer.Dispose();
                        holdInteraction.SetProgress(0f);
                        holdResolver.OnHoldStart -= OnHoldStart;
                        holdResolver.OnHoldEnd -= OnHoldEnd;
                        holdResolver.OnClick -= OnClick;
                        holdResolver.OnHoldProgressChanged -= UpdateBaseHoldProgress;
                        holdResolvers.Remove(action);
                        holdResolver.Dispose();
                    }

                    void UpdateBaseHoldProgress(float progress)
                    {
                        float basePercentage = baseHoldTime / totalHoldTime;
                        holdInteraction.SetProgress(progress * basePercentage);
                    }

                    void OnClick()
                    {
                        Unbind();
                        ExecuteInteraction(tapInteraction);
                    }

                    void OnHoldStart()
                    {
                        holdTimer.Start();
                    }

                    void OnHoldEnd()
                    {
                        Unbind();
                    }

                    holdResolver.OnHoldStart += OnHoldStart;
                    holdResolver.OnHoldEnd += OnHoldEnd;
                    holdResolver.OnClick += OnClick;
                    holdResolver.OnHoldProgressChanged += UpdateBaseHoldProgress;
                    holdResolvers.Add(action, holdResolver);
                    holdResolver.ReportHoldInputDown();
                }
                else
                {
                    ExecuteInteraction(tapInteraction);
                }
            }
        }

        private void HandleInteractUp(int action)
        {
            if (holdResolvers.TryGetValue(action, out IHoldResolver resolver))
            {
                resolver.ReportHoldInputUp();
            }
        }

        private void CheckForFocusableInteractibles()
        {
            var interactible = Physics.OverlapSphere(interactor.transform.position, config.InteractRadius)
                .Select(x => x.GetComponent<IInteractable>())
                .Where(x => x != null && InTargetFieldOfView(interactor.transform, x.transform, config.InteractFieldOfView) && x.CanFocus(interactor))
                .OrderBy(x => Vector3.Distance(interactor.transform.position, x.transform.position))
                .FirstOrDefault();

            if (interactible != null && interactible != Focus) { SetFocus(interactible); }
            else if (interactible == null) { RemoveFocus(); }
        }

        public void SetFocus(IInteractable interactible)
        {
            if (Focus != null)
            {
                RemoveFocus();
            }

            Focus = interactible;
            Focus.OnForcedUnfocus += RemoveFocus;
            Focus.OnSetFocus(interactor);
            OnFocus?.Invoke(interactor, Focus);
        }

        public void RemoveFocus()
        {
            if (Focus == null) { return; }
            Focus.OnForcedUnfocus -= RemoveFocus;
            Focus.OnRemoveFocus();
            Focus = null;
            OnDefocus?.Invoke();
        }

        private static bool InTargetFieldOfView(Transform agent, Transform target, float fieldOfViewAngle = 60)
        {
            Vector3 targetDirection = target.position - agent.position;
            float lookingAngle = Vector3.Angle(targetDirection, agent.forward);
            return lookingAngle < fieldOfViewAngle;
        }

        protected override void OnTick()
        {
            if (!isEnabled) { return; }

            CheckForFocusableInteractibles();
        }

        protected override void OnDispose()
        {
            input.OnInteractDown -= HandleInteractDown;
            input.OnInteractUp -= HandleInteractUp;
        }
    }
}
