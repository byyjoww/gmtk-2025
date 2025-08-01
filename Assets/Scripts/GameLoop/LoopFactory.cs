using GMTK2025.Characters;
using GMTK2025.Environment;
using NUnit.Framework;
using SLS.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

namespace GMTK2025.GameLoop
{
    public class LoopFactory
    {
        [System.Serializable]
        public class Config
        {
            public string EmptyNametag = "Unoccupied";
            public NPCPreset[] presets = new NPCPreset[0];
        }

        private PlayerCharacter character = default;
        private DialogueRunner dialogue = default;
        private Wallet collected = default;
        private NPC[] npcs = default;
        private SpawnLocation[] spawnPositions = default;
        private Config config = default;

        public LoopFactory(PlayerCharacter character, DialogueRunner dialogue, Wallet collected, NPC[] npcs, SpawnLocation[] spawnPositions, Config config)
        {
            this.character = character;
            this.dialogue = dialogue;
            this.collected = collected;
            this.npcs = npcs;
            this.spawnPositions = spawnPositions;
            this.config = config;
        }

        public Loop Create(int numOfTotalNPCs, int numOfKnownNPCs, HashSet<NPCProfile> knownNpcs)
        {
            ResetTrain();

            int numOfExistingNPCs = Mathf.Min(numOfTotalNPCs, numOfKnownNPCs);
            int numOfNewNPCs = Mathf.Max(0, numOfTotalNPCs - numOfKnownNPCs);
            var profiles = CreateNPCProfilesForLoop(numOfNewNPCs);
            var availableKnown = knownNpcs.ToHashSet();

            for (int i = 0; i < numOfExistingNPCs; i++)
            {
                var profile = availableKnown.Random();
                availableKnown.Remove(profile);
                profiles.Add(profile);
            }

            var availableSpawns = spawnPositions.ToList();
            for (int i = 0; i < numOfTotalNPCs; i++)
            {
                var profile = profiles[i];
                var spawn = availableSpawns.Random();
                availableSpawns.Remove(spawn);
                availableSpawns.RemoveAll(x => spawn.Associated.Contains(x));

                profile.npc = GameObject.Instantiate(profile.template, spawn.Location.position, spawn.Location.rotation);
                profile.npc.Setup(dialogue);

                spawn.SetNametag(profile.Name);
            }

            return new Loop(character, collected, profiles.ToArray());
        }

        private List<NPCProfile> CreateNPCProfilesForLoop(int numOfNpcs)
        {
            var profiles = new List<NPCProfile>();
            var availableNPCs = npcs.ToList();
            var availablePresets = config.presets.ToList();

            for (int i = 0; i < numOfNpcs; i++)
            {
                if (availableNPCs.Count == 0)
                {
                    availableNPCs = npcs.ToList();
                }

                if (availablePresets.Count == 0)
                {
                    availablePresets = config.presets.ToList();
                }

                var profile = new NPCProfile
                {
                    template = availableNPCs.Random(),
                    preset = availablePresets.Random(),
                };

                availableNPCs.Remove(profile.template);
                availablePresets.Remove(profile.preset);
                profiles.Add(profile);
            }

            return profiles;
        }

        private void ResetTrain()
        {
            spawnPositions.ForEach(x => x.SetNametag(config.EmptyNametag));
        }
    }
}