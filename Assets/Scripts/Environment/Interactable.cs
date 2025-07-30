using SLS.Core.Attributes;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GMTK2025.Environment
{
    // [RequireComponent(typeof(Outline))]
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField, ReadOnly] private bool isEnabled = default;
        [SerializeField, ReadOnly] private new Collider collider = default;
        [SerializeField] protected string interaction = "Interact";
        [SerializeField] private bool enableOnAwake = true;
        [SerializeField] protected bool disableOnInteract = true;
        [SerializeField] private bool useInteractionArea = false;
        [ConditionalField(nameof(useInteractionArea))]
        [SerializeField] private Collider volume = default;
        [SerializeField] private bool restrictInteractors = false;
        [ConditionalField(nameof(restrictInteractors))]
        [SerializeField] private IInteractorCollectionWrapper canInteract = new IInteractorCollectionWrapper();
        [SerializeField, Space, HideInInspector] private UnityEvent onInteract = new UnityEvent();

        public UnityEvent OnInteract => onInteract;

        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        public event UnityAction OnForcedUnfocus;

        protected virtual void Awake()
        {
            isEnabled = enableOnAwake;
        }

        public virtual InteractionCollection GetInteractions(IInteractor interactor)
        {
            var baseInteraction = new VisibleInteraction(interaction, 1, DoInteract);
            return new InteractionCollection(baseInteraction);
        }

        public virtual bool CanFocus(IInteractor interactor)
        {
            return IsEnabled
                && (!restrictInteractors || canInteract.Interactors.Contains(interactor))
                && (!useInteractionArea || IsWithinInteractionArea(interactor));
        }

        public virtual void OnSetFocus(IInteractor interactor)
        {
            
        }

        public virtual void OnRemoveFocus()
        {

        }

        protected virtual bool DoInteract(IInteractor interactor)
        {
            if (!IsEnabled || !CanFocus(interactor)) { return false; }
            IsEnabled = !disableOnInteract;
            onInteract?.Invoke();
            return true;
        }

        protected void TriggerOnForcedUnfocus()
        {
            OnForcedUnfocus?.Invoke();
        }

        private bool IsWithinInteractionArea(IInteractor interactor)
        {
            return useInteractionArea
                ? volume.bounds.Contains(interactor.transform.position)
                : true;
        }

        protected virtual void OnDestroy()
        {
            TriggerOnForcedUnfocus();
        }

        protected virtual void OnValidate()
        {
            if (collider == null)
            {
                Collider existing = gameObject.GetComponent<Collider>();
                collider = existing != null
                    ? existing
                    : CreateDefaultCollider();
            }
        }

        private Collider CreateDefaultCollider()
        {
            var col = gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(0.2f, 0.2f, 0.2f);
            col.isTrigger = true;
            return col;
        }
    }
}
