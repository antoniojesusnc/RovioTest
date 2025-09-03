using System;
using UnityEngine;
using Urd.Feedback;

namespace Urd.Services
{
    [Serializable]
    public class PhysicalFeedbackService : BaseService, IPhysicalFeedbackService
    {
        public override int LoadPriority => 100;

        [SerializeReference, SubclassSelector]
        private IHapticProvider _provider;
        
        [field: SerializeField]
        public bool IsEnabled { get; private set; }
        
        public override void Init()
        {
            base.Init();
        }

       
        public void Haptic(HapticType hapticType)
        {
            if (IsEnabled && _provider != null)
            {
                _provider.Haptic(hapticType);
            }
        }
        
        public void SetHapticEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }
    }
}
