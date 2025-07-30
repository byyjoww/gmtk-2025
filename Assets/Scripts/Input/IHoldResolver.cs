using UnityEngine.Events;

namespace GMTK2025.Inputs
{
    public interface IHoldResolver
    {
        event UnityAction OnClick;
        event UnityAction OnHoldStart;
        event UnityAction OnHoldEnd;

        void ReportHoldInputDown();
        void ReportHoldInputUp();
        void DoHoldOverride();
        void OnClickOverride();
    }
}