using SLS.UI;
using TMPro;
using UnityEngine;

namespace GMTK2025.UI
{
    public class QuotaView : View
    {
        public struct PresenterModel
        {
            public string Text { get; set; }
        }

        [SerializeField] private TMP_Text text = default;

        public void Setup(PresenterModel model)
        {
            text.text = model.Text;
        }
    }
}