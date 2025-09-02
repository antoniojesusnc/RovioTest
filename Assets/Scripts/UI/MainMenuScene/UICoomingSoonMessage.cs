using DG.Tweening;
using UnityEngine;

namespace RovioTest.UI
{
    public class UICoomingSoonMessage : MonoBehaviour
    {
        public void ShowComingSoonMessage()
        {
            DOTween.Restart(gameObject);
        }
    }
}