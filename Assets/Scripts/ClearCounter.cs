using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour , IInteractable
{

    [SerializeField] private KitchenObjectSO kitchenObjectSo;
    [SerializeField] private Transform counterTopPoint;
    public void Interact()
    {
        Debug.Log("Interact with ClearCounter");
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab, counterTopPoint);
        kitchenObjectTransform.localPosition = Vector3.zero;

        
    }
}
