using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Urd.Services
{
    [System.Serializable]
    public class AssetService : BaseService, IAssetService
    {
        public override int LoadPriority => 10;
            
        private IResourceLocator _resourceLocator;
        
        public override void Init()
        {
            IsLoaded = false;
            
            base.Init();

            Addressables.InitializeAsync().Completed += OnAddressableInitialize;
        }

        private void OnAddressableInitialize(AsyncOperationHandle<IResourceLocator> resourceLocator)
        {
            _resourceLocator = resourceLocator.Result;
            SetAsLoaded();
            
            UnityEngine.Debug.Log($"[AssetService] OnAddressableInitialize {resourceLocator.Status}");
        }

        public void LoadAsset<T>(string addressName, Action<T> assetCallback)
        {
            _resourceLocator.Locate(addressName, typeof(T), out var location);

            if (location != null && location.Count > 0)
            {
                if (location.Count > 1)
                {
                    Debug.LogWarning($"[AssetService] LoadAsset {addressName}, more than 1 asset with this name");
                }
                LoadAssetIntenal<T>(location[0], assetCallback);
                return;
            }

            LoadAssetIntenal<T>(addressName, assetCallback);
        }
        
        private void LoadAssetIntenal<T>(IResourceLocation resourceLocation, Action<T> assetCallback)
        {
            Addressables.LoadAssetAsync<T>(resourceLocation).Completed += 
                task => OnLoadAsset<T>(task, resourceLocation.PrimaryKey, assetCallback);
        }

        private void LoadAssetIntenal<T>(string addressName, Action<T> assetCallback)
        {
            Addressables.LoadAssetAsync<T>(addressName).Completed += (task) => OnLoadAsset<T>(task, addressName, assetCallback);
        }

        private void OnLoadAsset<T>(AsyncOperationHandle<T> task, string addressableName, Action<T> assetCallback)
        {
            if (task.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning($"[AssetService] OnLoadAsset {addressableName} cannot Instantiate");
                assetCallback?.Invoke(default(T));
                return;
            }
            assetCallback?.Invoke(task.Result);
        }

        public void LoadAssetByLabel<T>(string labelName, Action<List<T>> assetsCallback)
        {
            _resourceLocator.Locate(labelName, typeof(T), out var location);
            
            if (location != null && location.Count > 0)
            {
                LoadAssetByLabelInternal<T>(labelName, location, assetsCallback);
                return;
            }

            LoadAssetByLabelInternal<T>(labelName, assetsCallback);
        }

        private void LoadAssetByLabelInternal<T>(string labelName, IList<IResourceLocation> locations, Action<List<T>> assetsCallback)
        {
            Addressables.LoadAssetsAsync<T>(locations, null).Completed +=
                objects => OnLoadAssetByLabelInternal<T>(objects, labelName, assetsCallback);
        }

        private void LoadAssetByLabelInternal<T>(string labelName, Action<List<T>> assetsCallback)
        {
            Addressables.LoadAssetsAsync<T>(labelName, null).Completed +=
                objects => OnLoadAssetByLabelInternal(objects, labelName, assetsCallback);
        }

        private void OnLoadAssetByLabelInternal<T>(AsyncOperationHandle<IList<T>> objects, string labelName, Action<List<T>> assetsCallback)
        {
            if (objects.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning($"[AssetService] OnLoadAsset {labelName} cannot Instantiate");
                assetsCallback?.Invoke(new List<T>());
                return;
            }
            assetsCallback?.Invoke(new List<T>(objects.Result));
        }
        
        public void Instantiate(string addressName, Transform parent, Action<GameObject> instantiateCallback)
        {
            if (!IsLoaded)
            {
                OnServiceFinishLoad += () => Instantiate(addressName, parent, instantiateCallback);
                return;
            }
            
            _resourceLocator.Locate(addressName, typeof(GameObject), out var location);

            if(location != null && location.Count > 0)
            {
                if(location.Count > 1)
                {
                    Debug.LogWarning($"[AssetService] Instantiate {addressName}, more than 1 asset with this name");
                }
                InstantiateInternal(location[0], parent, instantiateCallback);
                return;
            }

            InstantiateInternal(addressName, parent, instantiateCallback);
        }

        public void Instantiate(GameObject prefab, Transform parent, Action<GameObject> instantiateCallback)
        {
            
            if (!IsLoaded)
            {
                OnServiceFinishLoad += () => Instantiate(prefab, parent, instantiateCallback);
                return;
            }
            
            var newGameObject = GameObject.Instantiate(prefab, parent);
            instantiateCallback.Invoke(newGameObject);
        }
        
        public void Instantiate<T>(T prefab, Transform parent, Action<T> instantiateCallback) where T : Behaviour
        {
            
            if (!IsLoaded)
            {
                OnServiceFinishLoad += () => Instantiate(prefab, parent, instantiateCallback);
                return;
            }
            
            var newGameObject = GameObject.Instantiate<T>(prefab, parent);
            instantiateCallback.Invoke(newGameObject);
        }

        public void Destroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }

        private void InstantiateInternal(string addressName, Transform parent, Action<GameObject> instantiateCallback)
        {
            Addressables.InstantiateAsync(addressName, parent).Completed += (task) 
                => OnInstantiate(task, addressName, instantiateCallback);
        }

        private void InstantiateInternal(IResourceLocation resourceLocation, Transform parent, Action<GameObject> instantiateCallback)
        {
            Addressables.InstantiateAsync(resourceLocation, parent).Completed += (task) 
                => OnInstantiate(task, resourceLocation.InternalId, instantiateCallback);
        }

        private void OnInstantiate(AsyncOperationHandle<GameObject> task, string addressableName, Action<GameObject> instantiateCallback)
        {
            if (task.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning($"[AssetService] OnInstantiate {addressableName} cannot Instantiate");
                instantiateCallback?.Invoke(null);
                return;
            }
            instantiateCallback?.Invoke(task.Result);
        }
    }
}