using SLS.Core.Attributes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace GMTK2025.Environment
{
    public class Train : MonoBehaviour
    {
        [SerializeField, ReadOnly] private SpawnLocation[] spawnLocations = default;

        public SpawnLocation[] SpawnLocations => spawnLocations;

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
