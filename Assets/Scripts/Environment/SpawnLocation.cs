using TMPro;
using UnityEngine;

namespace GMTK2025.Environment
{
    public class SpawnLocation : MonoBehaviour
    {
        [SerializeField] private GameObject indicator = default;
        [SerializeField] private SpawnLocation[] associated = default;
        [SerializeField] private TMP_Text nametag = default;

        public Transform Location => transform;
        public SpawnLocation[] Associated => associated;

        private void Awake()
        {
            indicator.SetActive(false);
        }

        public void SetNametag(string name)
        {
            nametag.text = name;
        }
    }
}
