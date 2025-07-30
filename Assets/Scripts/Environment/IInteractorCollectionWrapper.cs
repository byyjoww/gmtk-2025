using GMTK2025.Utils;
using SLS.Core.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GMTK2025.Environment
{
    [System.Serializable]
    public class IInteractorCollectionWrapper : CollectionWrapper<Object>
    {
        public IEnumerable<IInteractor> Interactors => Value.Cast<IInteractor>();
    }
}
