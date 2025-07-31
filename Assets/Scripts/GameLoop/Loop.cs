using GMTK2025.Characters;

namespace GMTK2025.GameLoop
{
    public class Loop
    {
        private PlayerCharacter character = default;
        private NPCProfile[] npcs = default;

        public int Quota { get; private set; }
        public int Accumulated { get; private set; }
        public int Kicked { get; private set; }

        public Loop(NPCProfile[] npcs, int quota)
        {
            this.npcs = npcs;
            this.Quota = quota;
            this.Accumulated = 0;
            this.Kicked = 0;
        }
    }
}