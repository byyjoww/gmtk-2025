using UnityEngine.Events;

namespace GMTK2025.Environment
{
    public class VisibleHoldInteraction : VisibleInteraction
    {
        public float HoldTime { get; private set; }

        public UnityEvent<float> OnProgressUpdated = new UnityEvent<float>();

        public VisibleHoldInteraction(string interaction, int action, float holdTime, InteractFunc onExecute) : base(interaction, action, onExecute)
        {
            this.HoldTime = holdTime;
        }

        public void SetProgress(float percentage)
        {
            OnProgressUpdated?.Invoke(percentage);
        }
    }
}
