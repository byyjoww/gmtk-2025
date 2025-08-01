using GMTK2025.Environment;
using UnityEngine;
using Yarn.Unity.Attributes;

namespace GMTK2025.GameLoop
{
    [System.Serializable]
    public struct NPCPreset
    {
        public bool isKnown;
        public TextAsset dialogue;
        public NPCGender gender;
    }
}