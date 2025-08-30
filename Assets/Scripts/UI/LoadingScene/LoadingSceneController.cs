using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RovioTest
{
    public class LoadingSceneController : MonoBehaviour
    {
        [SerializeField]
        LoadingSceneConfig _config;

        public void Start()
        {
            DOVirtual.DelayedCall(_config.LoadingTime, LoadMainMenu);
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene(SceneUtils.MainMenuSceneIndex);
        }
    }
}
