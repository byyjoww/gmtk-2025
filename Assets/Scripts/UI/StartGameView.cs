using SLS.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.UI
{
    public class StartGameView : View
    {
        public struct PresenterModel
        {
            public string StartText { get; set; }
            public UnityAction OnStart { get; set; }
        }

        [SerializeField] private TMP_Text startText = default;
        [SerializeField] private SLSButton start = default;

        public void Setup(PresenterModel model)
        {
            startText.text = model.StartText;
            SetButtonAction(start, model.OnStart, true);
        }
    }
}