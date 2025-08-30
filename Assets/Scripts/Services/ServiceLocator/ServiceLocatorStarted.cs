using UnityEngine;
using UnityEngine.SceneManagement;

namespace Urd.Services
{
    public class ServiceLocatorStarted : MonoBehaviour
    {
        [SerializeField] 
        private ServiceLocatorConfig _serviceLocatorConfig;
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            
            StaticServiceLocator.Init();

            LoadServices();
        }

        private void LoadServices()
        {
            if (_serviceLocatorConfig == null)
            {
                LoadServiceLocatorConfigInDefaultPath();
            }

            if (_serviceLocatorConfig == null)
            {
                // TODO Error Model
                string error = "[ServiceLocatorStarted] Error when try to obtain the service locator config";
                Debug.LogError(error);
                return;
            }

            var services = _serviceLocatorConfig.ListOfServices;
            for (int i = 0; i < services.Count; i++)
            {
                StaticServiceLocator.Register(services[i], services[i].GetMainInterface());
            }
            
            StaticServiceLocator.ServicesLoaded(); 
            LoadFirstScene();
        }

        private void LoadFirstScene()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        private void LoadServiceLocatorConfigInDefaultPath()
        {
            
        }
    }
}