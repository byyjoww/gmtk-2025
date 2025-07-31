using GMTK2025.Characters;
using SLS.UI;
using System;

namespace GMTK2025.UI
{
    public class QuotaViewController : ViewController<QuotaView, Wallet>
    {
        protected override bool showOnInit => true;

        public QuotaViewController(QuotaView view, Wallet model) : base(view, model)
        {

        }

        protected override void OnInit()
        {
            model.OnValueChanged += OnValueChanged;
        }

        protected override void OnDispose()
        {
            model.OnValueChanged -= OnValueChanged;
        }

        protected override void OnShow()
        {
            UpdateView(model.Current);
        }

        protected override void OnHide()
        {
            
        }

        private void OnValueChanged(int prev, int current)
        {
            UpdateView(current);
        }

        private void UpdateView(int current)
        {
            view.Setup(new QuotaView.PresenterModel
            {
                Text = $"Money: ${current}\nQuota: ${100}",
            });
        }
    }
}