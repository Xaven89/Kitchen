using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CuttingCounter;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject gameObjectThatImplamentsIHasProgress;


     private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = gameObjectThatImplamentsIHasProgress.GetComponent<IHasProgress>();

        if (hasProgress == null ) 
        {
            Debug.LogError("Does not implement IHasProgress.");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged; ;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgreesChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
            Hide();
        else
            Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
