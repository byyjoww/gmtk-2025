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
            [System.Serializable]
            public struct NPCName
            {
                public string Name;
                public NPCGender Gender;
            }

            public string emptyNametag = "Unoccupied";
            public NPCName[] names = new NPCName[0];
            public NPCPreset[] presets = new NPCPreset[0];
            public NPC[] npcs = default;
        }

        private PlayerCharacter character = default;
        private DialogueRunner dialogue = default;
        private Wallet collected = default;
        private SpawnLocation[] spawnPositions = default;
        private Config config = default;

        private List<NPC> availableNPCs = default;
        private List<TextAsset> availableDialogues = default;
        private List<string> availableNames = default;

        public LoopFactory(PlayerCharacter character, DialogueRunner dialogue, Wallet collected, SpawnLocation[] spawnPositions, Config config)
        {
            this.character = character;
            this.dialogue = dialogue;
            this.collected = collected;
            this.spawnPositions = spawnPositions;
            this.config = config;

            availableNPCs = config.npcs.ToList();
            availableDialogues = config.presets.Select(x => x.dialogue).ToList();
            availableNames = config.names.Select(x => x.Name).ToList();
        }

        public Loop Create(int numOfTotalNPCs, int numOfKnownNPCs, HashSet<NPCProfile> knownNpcs)
        {
            ResetTrain();

            int numOfExistingNPCs = Mathf.Min(numOfTotalNPCs, numOfKnownNPCs);
            int numOfNewNPCs = Mathf.Max(0, numOfTotalNPCs - numOfKnownNPCs);
            var profiles = CreateNewNPCProfilesForLoop(numOfNewNPCs);
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
                if (availableDialogues.Count == 0)
                {
                    availableDialogues = config.presets.Select(x => x.dialogue).ToList();
                }

                // pick profile
                var possiblePresets = config.presets
                    .Where(x => availableDialogues.Contains(x.dialogue)
                        && (x.gender == profile.template.Gender || x.gender == NPCGender.Both)
                        && x.isKnown == knownNpcs.Contains(profile))
                    .Select(x => x)
                    .ToList();

                if (possiblePresets.Count == 0)
                {
                    availableDialogues = config.presets.Select(x => x.dialogue).ToList();
                    possiblePresets = config.presets
                        .Where(x => availableDialogues.Contains(x.dialogue)
                            && (x.gender == profile.template.Gender || x.gender == NPCGender.Both)
                            && x.isKnown == knownNpcs.Contains(profile))
                        .Select(x => x)
                        .ToList();
                }

                profile.preset = possiblePresets.Random();
                availableDialogues.Remove(profile.preset.dialogue);

                var spawn = availableSpawns.Random();
                availableSpawns.Remove(spawn);
                availableSpawns.RemoveAll(x => spawn.Associated.Contains(x));

                profile.npc = GameObject.Instantiate(profile.template, spawn.Location.position, spawn.Location.rotation);
                profile.npc.Setup(dialogue);
                profile.npc.Profile = profile;

                spawn.SetNametag(profile.name);
            }

            return new Loop(character, collected, profiles.ToArray());
        }

        private List<NPCProfile> CreateNewNPCProfilesForLoop(int numOfNpcs)
        {
            var profiles = new List<NPCProfile>();

            for (int i = 0; i < numOfNpcs; i++)
            {
                if (availableNPCs.Count == 0)
                {
                    availableNPCs = config.npcs.ToList();
                }

                if (availableNames.Count == 0)
                {
                    availableNames = config.names.Select(x => x.Name).ToList();
                }

                var template = availableNPCs.Random();
                var possibleNames = config.names
                    .Where(x => availableNames.Contains(x.Name) && (x.Gender == template.Gender || x.Gender == NPCGender.Both))
                    .Select(x => x.Name)
                    .ToList();

                if (possibleNames.Count == 0)
                {
                    availableNames = config.names.Select(x => x.Name).ToList();
                    possibleNames = config.names
                        .Where(x => availableNames.Contains(x.Name) && (x.Gender == template.Gender || x.Gender == NPCGender.Both))
                        .Select(x => x.Name)
                        .ToList();
                }

                var profile = new NPCProfile
                {
                    template = template,
                    name = possibleNames.Random(),
                };

                availableNPCs.Remove(profile.template);                
                availableNames.Remove(profile.name);
                profiles.Add(profile);
            }

            return profiles;
        }

        private void ResetTrain()
        {
            spawnPositions.ForEach(x => x.SetNametag(config.emptyNametag));
        }
    }
}