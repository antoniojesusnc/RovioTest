using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Urd.Feedback
{
    public class PhysicalFeedbackServiceTest : MonoBehaviour
    {
        [SerializeField] private Button _button;
        private List<PhysicalFeedbackServiceTestInfo> _buttons = new List<PhysicalFeedbackServiceTestInfo>();
        private HapticCandyCoded _hapticCandyCoded;

        private void Start()
        {
            _hapticCandyCoded = new HapticCandyCoded(); 
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            _button.GetComponentInChildren<TextMeshProUGUI>().text = ((HapticType)0).ToString();
            _button.onClick.AddListener(() => OnClick(_button));
            _buttons.Add(new PhysicalFeedbackServiceTestInfo(_button.gameObject, (HapticType)0));
            
            for (HapticType i = (HapticType)1; i < HapticType.Size; i++)
            {
                var button = GameObject.Instantiate(_button, _button.transform.parent);
                button.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
                button.onClick.AddListener(() => OnClick(button));
                _buttons.Add(new PhysicalFeedbackServiceTestInfo(button.gameObject, i));
            }
        }

        private void OnClick(Button button)
        {
            var haptic = _buttons.Find(info => info.GameObject.GetInstanceID() == button.gameObject.GetInstanceID()).Haptic;
            _hapticCandyCoded.Haptic(haptic);
        }

        private class PhysicalFeedbackServiceTestInfo
        {
            public GameObject GameObject { get; private set; }
            public HapticType Haptic { get; private set; }

            public PhysicalFeedbackServiceTestInfo(GameObject gameObject, HapticType haptic)
            {
                GameObject = gameObject;
                Haptic = haptic;
            }
        }
    }
}
