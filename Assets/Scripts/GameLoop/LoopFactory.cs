using GMTK2025.Environment;
using NUnit.Framework;
using SLS.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GMTK2025.GameLoop
{
    public class LoopFactory
    {
        public struct Config
        {
            public NPCPreset[] presets;
        }

        private NPC[] npcs = default;
        private SpawnLocation[] spawnPositions = default;
        private Config config = default;

        public LoopFactory(NPC[] npcs, SpawnLocation[] spawnPositions, Config config)
        {
            this.npcs = npcs;
            this.spawnPositions = spawnPositions;
            this.config = config;
        }

        public Loop Create(int numOfNpcs, int quota)
        {
            var profiles = CreateNPCProfilesForLoop(numOfNpcs);            
            return new Loop(profiles, quota);
        }

        private NPCProfile[] CreateNPCProfilesForLoop(int numOfNpcs)
        {
            var profiles = new NPCProfile[numOfNpcs];
            var availableNPCs = npcs.ToList();
            var availablePresets = config.presets.ToList();
            var availableSpawns = spawnPositions.ToList();

            for (int i = 0; i < numOfNpcs; i++)
            {
                var profile = new NPCProfile
                {
                    npc = availableNPCs.Random(),
                    preset = availablePresets.Random(),
                    spawn = availableSpawns.Random(),
                };

                availableNPCs.Remove(profile.npc);
                availablePresets.Remove(profile.preset);
                availableSpawns.Remove(profile.spawn);
                availableSpawns.RemoveAll(x => profile.spawn.Associated.Contains(x));
                profiles[i] = profile;
            }

            return profiles;
        }
    }
}