namespace GMTK2025.Characters
{
    public class Wallet
    {
        public int current = default;

        public Wallet(int starting)
        {
            this.current = starting;
        }

        public void Add(int amount)
        {
            current += amount;
        }

        public bool Remove(int amount)
        {
            if (current < amount) { return false; }

            current -= amount;
            return true;
        }
    }
}