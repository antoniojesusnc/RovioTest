using System;
using UnityEngine;

namespace Urd.Feedback
{
    [Serializable]
    public class HapticCandyCoded : IHapticProvider
    {
        public void Haptic(HapticType hapticType)
        {
#if UNITY_IOS && !UNITY_EDITOR
            CandyCoded.HapticFeedback.iOS.HapticFeedback.PerformHapticFeedback(GetIOSStringFromHaptic(hapticType));
#elif UNITY_ANDROID && !UNITY_EDITOR
            CandyCoded.HapticFeedback.Android.HapticFeedback.PerformHapticFeedback(GetAndroidFromHaptic(hapticType));
#else
            //Debug.Log($"Haptic: {hapticType.ToString()}");
#endif
        }
        
#if UNITY_IOS && !UNITY_EDITOR
        private string GetIOSStringFromHaptic(HapticType hapticType)
        {
            switch (hapticType)
            {
                case HapticType.Medium: return "medium";
                case HapticType.Heavy: return "heavy";
                default: return "light";
            }
        }
#elif UNITY_ANDROID && !UNITY_EDITOR
        private CandyCoded.HapticFeedback.Android.HapticFeedbackConstants GetAndroidFromHaptic(HapticType hapticType)
        {
            switch (hapticType)
            {
                case HapticType.Light: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.CLOCK_TICK;
                case HapticType.Medium: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.VIRTUAL_KEY;
                case HapticType.Heavy: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.LONG_PRESS;
                case HapticType.LongPress: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.LONG_PRESS;
                case HapticType.Drag: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.DRAG_CROSSING;
                case HapticType.EntryBump: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.ENTRY_BUMP;
                case HapticType.EdgeSqueeze: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.EDGE_SQUEEZE;
                case HapticType.EdgeRelease: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.EDGE_RELEASE;
                case HapticType.Confirm: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.CONFIRM;
                case HapticType.Reject: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.REJECT;
                case HapticType.GestureStart: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.GESTURE_START;
                case HapticType.GestureEnd: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.GESTURE_END;
                default: return CandyCoded.HapticFeedback.Android.HapticFeedbackConstants.CONTEXT_CLICK;
            }
        }
#endif
    }
}