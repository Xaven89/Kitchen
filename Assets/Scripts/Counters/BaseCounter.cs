using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BaseCounter : MonoBehaviour, IInteractable , IKitchenObjectParent
    {
        [SerializeField] private Transform counterTopPoint;
        private KitchenObject kitchenObject;
        public virtual void Interact(Player player) 
        { 
        
        }

        public virtual void InteractAlternate(Player player)
        {

        }

        public Transform GetKitchenObjectFollowTransform()
        {
            return counterTopPoint;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;
        }

        public KitchenObject GetKitchenObject() { return kitchenObject; }
        public void ClearKitchenObject() { kitchenObject = null; }
        public bool HasKitchenObject() { return kitchenObject != null; }
    }
}
