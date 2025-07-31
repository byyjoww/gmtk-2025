using GMTK2025.Environment;
using UnityEngine;

namespace GMTK2025.GameLoop
{
    public class LoopFactory
    {
        public struct Config
        {
            public int NumOfNPCPerCarriage;
            public NPCPreset[] presets;
        }

        private NPC[] npcs = default;
        private Transform[] spawnPositions = default;
        private Config config = default;

        public LoopFactory(NPC[] npcs, Transform[] spawnPositions, Config config)
        {
            this.npcs = npcs;
            this.spawnPositions = spawnPositions;
            this.config = config;
        }

        public Loop Create()
        {
            var profiles = CreateNPCProfilesForLoop();
            int quota = profiles.Length * 10;
            return new Loop(profiles, quota);
        }

        private NPCProfile[] CreateNPCProfilesForLoop()
        {
            return new NPCProfile[0];
        }
    }
}