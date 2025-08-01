using SLS.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.UI
{
    public class EndGameView : View
    {
        public struct PresenterModel
        {
            public string DescriptionText { get; set; }
            public string RestartText { get; set; }
            public UnityAction OnRestart { get; set; }
        }

        [SerializeField] private TMP_Text descriptionText = default;
        [SerializeField] private TMP_Text restartText = default;
        [SerializeField] private SLSButton restart = default;

        public void Setup(PresenterModel model)
        {
            descriptionText.text = model.DescriptionText;
            restartText.text = model.RestartText;
            SetButtonAction(restart, model.OnRestart, true);
        }
    }
}