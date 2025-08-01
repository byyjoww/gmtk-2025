using GMTK2025.App;
using GMTK2025.Characters;
using SLS.UI;
using System;

namespace GMTK2025.UI
{
    public class QuotaViewController : ViewController<QuotaView, Wallet>
    {
        private Wallet quota = default;
        private Wallet collected = default;
        private GameState gameState = default;

        public QuotaViewController(QuotaView view, Wallet model, Wallet quota, Wallet collected, GameState gameState) : base(view, model)
        {
            this.quota = quota;
            this.collected = collected;      
            this.gameState = gameState;
        }

        protected override void OnInit()
        {
            gameState.OnStart += Show;
            gameState.OnLose += Hide;
            model.OnValueChanged += OnValueChanged;
            quota.OnValueChanged += OnValueChanged;
            collected.OnValueChanged += OnValueChanged;
        }

        protected override void OnDispose()
        {
            gameState.OnStart -= Show;
            gameState.OnLose -= Hide;
            model.OnValueChanged -= OnValueChanged;
            quota.OnValueChanged -= OnValueChanged;
            collected.OnValueChanged -= OnValueChanged;
        }

        protected override void OnShow()
        {
            UpdateView(model.Current, quota.Current, collected.Current);
        }

        protected override void OnHide()
        {
            
        }

        private void OnValueChanged(int arg1, int arg2)
        {
            UpdateView(model.Current, quota.Current, collected.Current);
        }

        private void UpdateView(int walletAmount, int quotaAmount, int collectedAmount)
        {
            view.Setup(new QuotaView.PresenterModel
            {
                Text = $"Money: ${walletAmount}\nQuota: ${quotaAmount}\nCollected: ${collectedAmount}",
            });
        }
    }
}