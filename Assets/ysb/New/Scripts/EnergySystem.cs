using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergySystem : MonoBehaviour
{
    [SerializeField]
    private int maxEnergy;
    [SerializeField]
    private int curEnergy;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMP_Text eText;

    public int useEnergy = 1;

    private void Start()
    {
        eText = slider.GetComponentInChildren<TMP_Text>();
        maxEnergy = 50;

        SetEnergy();
    }
    public void SetEnergy(int e = 0)
    {
        int energy = UpgradeManager.instance.getEnergy();
        if (energy != 0)
        {
            curEnergy = energy < maxEnergy ? energy : maxEnergy;
        }
        else
        {
            curEnergy = maxEnergy;
        }
        curEnergy = curEnergy + e > maxEnergy ? maxEnergy : curEnergy + e;
        slider.value = (float)curEnergy / maxEnergy;
        eText.text = curEnergy.ToString() + " / " + maxEnergy.ToString();
    }
    public void UseEnergy(int i = 0)
    {
        if (i == 0)
        {
            curEnergy = curEnergy - useEnergy > 0 ? curEnergy - useEnergy : 0;
        }
        else
        {
            curEnergy = curEnergy - i > 0 ? curEnergy - i : 0;
        }
        UpgradeManager.instance.getEnergy(curEnergy);
        slider.value = (float)curEnergy / maxEnergy;
        eText.text = curEnergy.ToString() + " / " + maxEnergy.ToString();
        if (curEnergy <= 0)
        {
            Debug.Log("gameOver");
        }
    }
}
