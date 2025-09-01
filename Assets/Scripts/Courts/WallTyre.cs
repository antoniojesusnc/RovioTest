using DG.Tweening;
using UnityEngine;

namespace RovioTest.View
{
    public class WallTyre : MonoBehaviour
    {
        [SerializeField] 
        private DOTweenAnimation _animation;

        public void DoAnimation()
        {
            _animation.tween.Restart();
        }
    }
}
