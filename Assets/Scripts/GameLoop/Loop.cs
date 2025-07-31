using GMTK2025.Characters;

namespace GMTK2025.GameLoop
{
    public class Loop
    {
        private PlayerCharacter character = default;
        private Wallet collected = default;

        public NPCProfile[] NPCs { get; private set; }
        public int Kicked { get; private set; }

        public Loop(PlayerCharacter character, Wallet collected, NPCProfile[] npcs)
        {
            this.character = character;
            this.collected = collected;
            this.NPCs = npcs;
            this.Kicked = 0;
        }
    }
}