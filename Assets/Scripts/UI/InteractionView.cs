using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK2025.UI
{
    public class InteractionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text textComponent = default;
        [SerializeField] private GameObject progressBarPanel = default;
        [SerializeField] private Image progressBarComponent = default;

        private string key = string.Empty;
        private string interaction = string.Empty;

        public void Show(string key, string interaction)
        {
            Show(key, interaction, 0f);
        }

        public void Show(string key, string interaction, float progress)
        {
            this.interaction = interaction;
            UpdateKey(key);
            SetProgress(progress);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void UpdateKey(string key)
        {
            this.key = key;
            UpdateText();
        }

        private void UpdateText()
        {
            if (textComponent == null) { return; }
            textComponent.text = $"{key}"; // $"Press {key} to {interaction}";
        }

        private void SetProgress(float progress)
        {
            progressBarComponent.fillAmount = progress;
            progressBarPanel.SetActive(progress > 0 && progress < 1);
        }

        private void OnValidate()
        {
            if (textComponent == null) { textComponent = GetComponentInChildren<TMP_Text>(); }
        }
    }
}