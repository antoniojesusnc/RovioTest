using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Events;

namespace Urd.Utils
{
    public class ColliderComponentHelper : MonoBehaviour
    {
        [SerializeField] 
        private LayerMask _layerMask;
        
        [Foldout("Triggers")]
        [SerializeField]
        private UnityEvent<Collider> onTriggerEnter;
        [Foldout("Triggers")]
        [SerializeField]
        private UnityEvent<Collider> onTriggerStay;
        [Foldout("Triggers")]
        [SerializeField]
        private UnityEvent<Collider> onTriggerExit;
        
        [Foldout("Colliders")]
        [SerializeField]
        private UnityEvent<Collision> onCollisionEnter;
        [Foldout("Colliders")]
        [SerializeField]
        private UnityEvent<Collision> onCollisionStay;
        [Foldout("Colliders")]
        [SerializeField]
        private UnityEvent<Collision> onCollisionExit;

        private void OnTriggerEnter(Collider collider)
        {
            if ((_layerMask & 1 << collider.gameObject.layer) > 0)
            {
                onTriggerEnter?.Invoke(collider);
            }
        }

        private void OnTriggerStay(Collider collider)
        {
            if ((_layerMask & 1 << collider.gameObject.layer) > 0)
            {
                onTriggerStay?.Invoke(collider);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if ((_layerMask & 1 << collider.gameObject.layer) > 0)
            {
                onTriggerExit?.Invoke(collider);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((_layerMask & 1 << collision.gameObject.layer) > 0)
            {
                onCollisionEnter?.Invoke(collision);
            }
        }
        
        private void OnCollisionStay(Collision collision)
        {
            if ((_layerMask & 1 << collision.gameObject.layer) > 0)
            {
                onCollisionStay?.Invoke(collision);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if ((_layerMask & 1 << collision.gameObject.layer) > 0)
            {
                onCollisionExit?.Invoke(collision);
            }
        }
    }
}