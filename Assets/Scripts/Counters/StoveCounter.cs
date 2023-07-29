using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs:EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private State state;
    public event EventHandler<IHasProgress.OnProgreesChangedEventArgs> OnProgressChanged;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    if (fryingRecipeSO != null)
                    {
                        fryingTimer += Time.deltaTime;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgreesChangedEventArgs()
                        {
                            progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                        });
                        if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                        {
                            GetKitchenObject().DestroySelf();
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgreesChangedEventArgs()
                            {
                                progressNormalized = 0
                            });
                            fryingTimer = 0f;
                            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                            burningRecipeSO = GetOutputForInputBurning(GetKitchenObject().GetKitchenObjectSO());
                            state = State.Fried;
                            burningTimer = 0f;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                            {
                                state = state,
                            });
                        }
                    }
                    break;
                case State.Fried:
                     if (burningRecipeSO != null)
                    {
                        burningTimer += Time.deltaTime;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgreesChangedEventArgs()
                        {
                            progressNormalized = (float)burningTimer / burningRecipeSO.burningTimerMax
                        });
                        if (burningTimer > burningRecipeSO.burningTimerMax)
                        {
                            GetKitchenObject().DestroySelf();
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgreesChangedEventArgs()
                            {
                                progressNormalized = 0
                            });
                            burningTimer = 0f;
                            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                            state = State.Burned;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
                            {
                                state = state,
                            });
                        }
                    }
                    break;
                case State.Burned:
                    break;
                default:
                    break;
            }
        }
        
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()
            && player.HasKitchenObject()
            && HasFryingRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
            fryingRecipeSO = GetOutputForInputFrying(GetKitchenObject().GetKitchenObjectSO());
            state = State.Frying;
            fryingTimer = 0f;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
            {
                state = state,
            });
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            if (player.HasKitchenObject()) 
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        ResetBurningProgress();
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                ResetBurningProgress();
            }
        }
    }

    private void ResetBurningProgress()
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgreesChangedEventArgs()
        {
            progressNormalized = 0
        });
        state = State.Idle;

        fryingTimer = 0f;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
        {
            state = state,
        });
    }

    private FryingRecipeSO GetOutputForInputFrying(KitchenObjectSO inputRecipeSO)
    {
        return fryingRecipeSOArray
            .Where(recipeSO => recipeSO.input == inputRecipeSO)
            .FirstOrDefault();
    }

    private bool HasFryingRecipeWithInput(KitchenObjectSO inputKitchenObject)
    {
        return fryingRecipeSOArray.Any(recipeSO => recipeSO.input == inputKitchenObject);
    }

    private BurningRecipeSO GetOutputForInputBurning(KitchenObjectSO inputRecipeSO)
    {
        return burningRecipeSOArray
            .Where(recipeSO => recipeSO.input == inputRecipeSO)
            .FirstOrDefault();
    }

    private bool HasBurningRecipeWithInput(KitchenObjectSO inputKitchenObject)
    {
        return burningRecipeSOArray.Any(recipeSO => recipeSO.input == inputKitchenObject);
    }
}
