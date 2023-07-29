using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IKitchenObjectParent
    {
        public Transform GetKitchenObjectFollowTransform();
        public void SetKitchenObject(KitchenObject kitchenObject);
        public KitchenObject GetKitchenObject() ;
        public void ClearKitchenObject() ;
        public bool HasKitchenObject() ;
    }
}
