using TMPro;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoCount;
    

    public void SetAmmo(int currentAmmo, int maxAmmo)
    {
        ammoCount.text = currentAmmo + " / " + maxAmmo;
    }


}
