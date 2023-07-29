using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgreesChangedEventArgs> OnProgressChanged;
   
    public event EventHandler OnCut;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipesSOArray;

    private int cuttingProgress;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject() 
            && player.HasKitchenObject()
            && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            cuttingProgress = 0;
            CuttingRecipeSO cuttingRecipeSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            if (cuttingRecipeSO != null)
            {
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgreesChangedEventArgs()
                {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
            }
        }
        else if (HasKitchenObject() )
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        ResetCuttingProgress();
                    }
                }
            }
            else 
            { 
            
                GetKitchenObject().SetKitchenObjectParent(player);
                ResetCuttingProgress();
            }
        }
    }

    private void ResetCuttingProgress()
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgreesChangedEventArgs()
        {
            progressNormalized = 0
        });
        cuttingProgress = 0;
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            CuttingRecipeSO cuttingRecipeSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            if (cuttingRecipeSO == null)
                return;
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgreesChangedEventArgs()
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            if (cuttingProgress < cuttingRecipeSO.cuttingProgressMax)
                return;
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
        }
    }

    private CuttingRecipeSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return cuttingRecipesSOArray
            .Where(recipeSO => recipeSO.input == inputKitchenObjectSO)
            .FirstOrDefault();
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObject)
    {
        return cuttingRecipesSOArray.Any(recipeSO => recipeSO.input == inputKitchenObject);
    }
}
