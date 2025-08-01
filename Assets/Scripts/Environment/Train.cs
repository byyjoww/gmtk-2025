using SLS.Core.Attributes;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace GMTK2025.Environment
{
    public class Train : MonoBehaviour
    {
        [SerializeField] private TMP_Text carriageName = default;
        [SerializeField, ReadOnly] private SpawnLocation[] spawnLocations = default;

        public SpawnLocation[] SpawnLocations => spawnLocations;

        public void SetCarriageName(string name)
        {
            carriageName.text = name;
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (PrefabUtility.IsPartOfNonAssetPrefabInstance(gameObject) && PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                spawnLocations = FindObjectsByType<SpawnLocation>(FindObjectsSortMode.None);
                EditorUtility.SetDirty(this);
            }
#endif
        }
    }
}
