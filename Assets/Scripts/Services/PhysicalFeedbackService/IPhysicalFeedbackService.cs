using Urd.Feedback;

namespace Urd.Services
{
    public interface IPhysicalFeedbackService : IBaseService
    {
        bool IsEnabled { get; }
        void Haptic(HapticType hapticType);
        void SetHapticEnabled(bool enabled);
    }
}
