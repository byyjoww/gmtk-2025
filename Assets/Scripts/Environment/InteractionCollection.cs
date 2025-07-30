using System.Collections.Generic;
using System.Linq;

namespace GMTK2025.Environment
{
    public class InteractionCollection
    {
        public List<IInteraction> Options { get; internal set; }

        public InteractionCollection(params IInteraction[] interactions)
        {
            this.Options = new List<IInteraction>();
            this.Options.AddRange(interactions);
        }

        public void Add(IEnumerable<IInteraction> interactions)
        {
            this.Options.AddRange(interactions);
        }

        public void Add(IInteraction interaction)
        {
            this.Options.Add(interaction);
        }

        public T GetInteractionOfType<T>() where T : IInteraction
        {
            return (T)Options.FirstOrDefault(x => x is T);
        }

        public IEnumerable<T> GetInteractionsOfType<T>() where T : IInteraction
        {
            return Options.Where(x => x is T).Cast<T>();
        }

        public IVisibleInteraction[] GetVisibleInteractions(int action)
        {
            return GetInteractionsOfType<IVisibleInteraction>()
                .Where(x => x.Trigger == action)
                .ToArray();
        }

        public bool TryGetVisibleInteraction(int action, out IVisibleInteraction interaction)
        {
            interaction = GetInteractionsOfType<IVisibleInteraction>()
                .FirstOrDefault(x => x.Trigger == action);

            return interaction != null;
        }          
    }
}
