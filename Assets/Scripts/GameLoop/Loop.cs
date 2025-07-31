using GMTK2025.Characters;

namespace GMTK2025.GameLoop
{
    public class Loop
    {
        private PlayerCharacter character = default;
        private NPCProfile[] npcs = default;
        private int quota = default;
        private int accumulated = default;

        public int Quota => quota;
        public int Accumulated => accumulated;

        public Loop(NPCProfile[] npcs, int quota)
        {
            this.npcs = npcs;
            this.quota = quota;
        }
    }
}