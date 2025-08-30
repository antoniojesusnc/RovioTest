using System;
using System.Collections.Generic;

namespace Urd.Services
{
    public static class StaticServiceLocator
    {
        private static Dictionary<Type, IBaseService> Services = new Dictionary<Type, IBaseService>();

        public static bool AllServicesLoaded { get; private set; }
        public static Action OnServiceInitialized;

        public static void Init()
        {
            Services = new Dictionary<Type, IBaseService>();
        }

        private static void InitServices()
        {
        }

        public static void Register<T>(T serviceInstance) where T : class, IBaseService
        {
            Register(serviceInstance, typeof(T));
        }
        
        public static void Register(IBaseService serviceInstance, Type serviceType)
        {
            Services[serviceType] = serviceInstance;
            serviceInstance.Init();
        }

        public static bool Exist<T>()
        {
            return Services.ContainsKey(typeof(T));
        }

        public static T Get<T>() where T : class, IBaseService
        {
            IBaseService instance = null;
            if (Services?.TryGetValue(typeof(T), out instance) == false)
            {
                //Debug.LogWarning($"Service of type {typeof(T)} not registered");
            }
            
            return instance as T;
        }
        
        public static void ServicesLoaded()
        {
            AllServicesLoaded = true;
            OnServiceInitialized?.Invoke();
        }
        
        public static void Reset()
        {
            Services.Clear();
        }
    }
}