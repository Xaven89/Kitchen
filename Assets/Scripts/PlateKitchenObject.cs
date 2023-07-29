using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }
    private List<KitchenObjectSO> kitechObjectSOList;
    [SerializeField] private List<KitchenObjectSO> validKichenObjectSOList;
    private void Awake()
    {
        kitechObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (kitechObjectSOList.Contains(kitchenObjectSO)
            || !validKichenObjectSOList.Contains(kitchenObjectSO))
            return false;
        kitechObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this,new OnIngredientAddedEventArgs()
        {
            kitchenObjectSO = kitchenObjectSO
        });
        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    { return kitechObjectSOList; }
}
