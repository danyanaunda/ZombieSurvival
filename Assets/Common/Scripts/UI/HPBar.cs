using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpCount;
    [SerializeField] private Image filler;
    private int _maxValue;

    public void Init(int value)
    {
        _maxValue = value;
    }

    public void UpdateHitPoints(int currentValue)
    {
        hpCount.text = currentValue.ToString();
        filler.fillAmount = (float) currentValue / _maxValue;
    }
}