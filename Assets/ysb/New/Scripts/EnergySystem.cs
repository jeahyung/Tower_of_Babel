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
    private static int curEnergy = 0;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMP_Text eText;

    public int useEnergy = 1;

    private void Start()
    {
        eText = slider.GetComponentInChildren<TMP_Text>();
        maxEnergy = 50;

        if(curEnergy <= 0) { curEnergy = maxEnergy; }
        slider.value = (float)curEnergy / maxEnergy;
        eText.text = curEnergy.ToString() + " / " + maxEnergy.ToString();
        //SetEnergy();
    }



    public void SetEnergy(int e = 0)
    {
        curEnergy = curEnergy + e > maxEnergy ? maxEnergy : curEnergy + e;
        slider.value = (float)curEnergy / maxEnergy;
        eText.text = curEnergy.ToString() + " / " + maxEnergy.ToString();
    }
    public bool UseEnergy(int i = 0)
    {
        if (i == 0)
        {
            curEnergy = curEnergy - useEnergy > 0 ? curEnergy - useEnergy : 0;
        }
        else
        {
            curEnergy = curEnergy - i > 0 ? curEnergy - i : 0;
        }
        //UpgradeManager.instance.getEnergy(curEnergy);
        slider.value = (float)curEnergy / maxEnergy;
        eText.text = curEnergy.ToString() + " / " + maxEnergy.ToString();
        if (curEnergy <= 0)
        {
            StageManager.instance.GameOver();
            Debug.Log("gameOver");
            return false;
        }

        return true;
    }
}
