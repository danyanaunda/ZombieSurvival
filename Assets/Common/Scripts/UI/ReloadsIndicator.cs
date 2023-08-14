using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReloadsIndicator : MonoBehaviour
{
    [SerializeField] private Image filler;
    [SerializeField] private TextMeshProUGUI text;


    public void Activate(float timeInReload, float timeReload)
    {
        filler.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        var timeLeft = Math.Round(timeReload - timeInReload, 2);

        filler.fillAmount = 1 - timeInReload / timeReload;
        text.text = timeLeft.ToString();
    }

    public void Deactivate()
    {
        filler.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
}