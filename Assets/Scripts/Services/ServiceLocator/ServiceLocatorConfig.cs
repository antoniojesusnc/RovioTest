using System;
using System.Collections.Generic;
using UnityEngine;
using Urd.Utils;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "ServiceLocatorConfig", menuName = "Urd/Services/Service Locator Config", order = 1)]
    public class ServiceLocatorConfig : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]
        public List<IBaseService> ListOfServices { get; private set; } = new ();

        [ContextMenu("FillWithAllServices")]
        public void FillWithAllServices()
        {
            ListOfServices.Clear();
            var types = AssemblyHelper.GetClassTypesThatImplement<IBaseService>();
            for (int i = 0; i < types.Count; i++)
            {
                var baseService = Activator.CreateInstance(types[i]) as IBaseService;
                ListOfServices.Add(baseService);
            }

            ListOfServices.Sort(SortByPriority);
        }

        private int SortByPriority(IBaseService service1, IBaseService service2)
        {
            return service1.LoadPriority.CompareTo(service2.LoadPriority);
        }
    }
}