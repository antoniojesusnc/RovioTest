using UnityEngine;
using UnityEngine.SceneManagement;

namespace RovioTest
{
    public class MainMenuController : MonoBehaviour
    {
        public void OpenGameScene()
        {
            SceneManager.LoadScene(SceneUtils.GameSceneIndex);
        }
    }
}
