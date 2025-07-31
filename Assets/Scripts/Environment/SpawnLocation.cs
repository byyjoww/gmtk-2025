using UnityEngine;

namespace GMTK2025.Environment
{
    public class SpawnLocation : MonoBehaviour
    {
        [SerializeField] private GameObject indicator = default;
        [SerializeField] private SpawnLocation[] associated = default;

        public Transform Location => transform;
        public SpawnLocation[] Associated => associated;

        private void Awake()
        {
            indicator.SetActive(false);
        }
    }
}
