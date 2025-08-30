using UnityEngine;
using UnityEngine.SceneManagement;
using Urd.Services;

namespace RovioTest
{
    public class MainMenuController : MonoBehaviour
    {
        public void OpenGameScene()
        {
            StaticServiceLocator.Get<IGamePlayService>().BeginBattle();
        }
    }
}
