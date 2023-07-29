using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;


    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        switch (e.state)
        {
            case StoveCounter.State.Frying:
                stoveOnGameObject.SetActive(true);
                particlesGameObject.SetActive(true);
                break;
            case StoveCounter.State.Fried:
                stoveOnGameObject.SetActive(true);
                particlesGameObject.SetActive(false);
                break;
            case StoveCounter.State.Idle:
            case StoveCounter.State.Burned:
            default:
                stoveOnGameObject.SetActive(false);
                break;
        }
    }
}
