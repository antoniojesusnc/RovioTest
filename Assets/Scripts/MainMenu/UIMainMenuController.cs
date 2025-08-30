using RovioTest.Services;
using UnityEngine;
using Urd.Services;

namespace RovioTest.UI
{
    public class UIMainMenuController : MonoBehaviour
    {
        public void OpenGameScene()
        {
            StaticServiceLocator.Get<IGamePlayService>().BeginBattle();
        }
    }
}
