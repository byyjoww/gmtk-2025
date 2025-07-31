using NUnit.Framework;
using SLS.Core.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK2025.Environment
{
    public class Train : MonoBehaviour
    {
        [SerializeField] private Transform spawnPosRoot = default;
        [SerializeField, ReadOnly] private Transform[] spawnPositions = default;

        public Transform[] SpawnPositions => spawnPositions;

        private void OnValidate()
        {
            var spawns = new List<Transform>();
            foreach (Transform transform in spawnPosRoot.transform)
            {
                spawns.Add(transform);
            }
            spawnPositions = spawns.ToArray();
        }
    }
}
