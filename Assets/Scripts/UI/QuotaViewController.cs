using GMTK2025.Characters;
using SLS.UI;
using System;

namespace GMTK2025.UI
{
    public class QuotaViewController : ViewController<QuotaView, Wallet>
    {
        private Wallet quota = default;
        private Wallet collected = default;        

        protected override bool showOnInit => true;

        public QuotaViewController(QuotaView view, Wallet model, Wallet quota, Wallet collected) : base(view, model)
        {
            this.quota = quota;
            this.collected = collected;            
        }

        protected override void OnInit()
        {
            model.OnValueChanged += OnValueChanged;
            quota.OnValueChanged += OnValueChanged;
            collected.OnValueChanged += OnValueChanged;
        }

        protected override void OnDispose()
        {
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