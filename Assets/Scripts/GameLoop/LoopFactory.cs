namespace GMTK2025.GameLoop
{
    public class LoopFactory
    {
        public LoopFactory()
        {

        }

        public Loop Create()
        {
            return new Loop(new NPCProfile[0], 100);
        }
    }
}