using UnityEngine.Events;

namespace GMTK2025.Characters
{
    public class Wallet
    {
        public int current = default;

        public int Current => current;

        public event UnityAction<int, int> OnValueChanged;

        public Wallet(int starting)
        {
            this.current = starting;
        }

        public void Add(int amount)
        {
            Set(current + amount);
        }

        public bool Remove(int amount)
        {
            if (current < amount) { return false; }

            Set(current - amount);
            return true;
        }

        public void Set(int amount)
        {
            int prev = current;
            current = amount;
            OnValueChanged?.Invoke(prev, current);
        }
    }
}