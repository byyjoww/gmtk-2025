using GMTK2025.Environment;
using System;

namespace GMTK2025.UI
{
    public class BaseInteractionViewController : IDisposable
    {
        protected IInteractionModel[] models = default;
        protected InteractionView view = default;
        protected IInteractionInput input = default;

        protected VisibleInteraction Current { get; set; }

        public BaseInteractionViewController(IInteractionModel[] models, InteractionView view, IInteractionInput input)
        {
            this.models = models;
            this.view = view;
            this.input = input;
            StartListening();
        }

        public void Show(string key, string interaction)
        {
            view.Show(key, interaction);
        }

        public void Show(string key, string interaction, float progress)
        {
            view.Show(key, interaction, progress);
        }

        public void Hide()
        {
            view.Hide();
        }

        protected virtual void SetFocus(IInteractor interactor, IInteractable interactible)
        {
            if (!view || view == null) { return; }

            Current = interactible
                .GetInteractions(interactor)
                .GetInteractionOfType<VisibleInteraction>();

            if (Current != null)
            {
                var key = input.GetInteractionInput(Current.Trigger);
                var interaction = Current.Interaction;
                Show(key, interaction);

                if (Current is VisibleHoldInteraction hold)
                {
                    hold.OnProgressUpdated.AddListener((progress) =>
                    {
                        Show(key, interaction, progress);
                    });
                }
            }
        }

        protected virtual void RemoveFocus()
        {
            if (!view || view == null) { return; }
            if (Current is VisibleHoldInteraction hold)
            {
                hold.OnProgressUpdated.RemoveAllListeners();
            }
            Hide();
        }

        private void Repaint()
        {
            if (view.isActiveAndEnabled)
            {
                var key = input.GetInteractionInput(Current.Trigger);
                view.UpdateKey(key);
            }
        }

        private void StartListening()
        {
            input.OnControlSchemeChanged += Repaint;
            foreach (var model in models)
            {
                model.OnFocus += SetFocus;
                model.OnDefocus += RemoveFocus;
            }
        }

        private void StopListening()
        {
            input.OnControlSchemeChanged -= Repaint;
            foreach (var model in models)
            {
                model.OnFocus -= SetFocus;
                model.OnDefocus -= RemoveFocus;
            }
        }

        public virtual void Dispose()
        {
            StopListening();
        }
    }
}