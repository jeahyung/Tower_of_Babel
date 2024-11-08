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
        maxEnergy = 100;

        if(curEnergy <= 0) { curEnergy = maxEnergy; }
        slider.value = (float)curEnergy / maxEnergy;
        eText.text = curEnergy.ToString() + " / " + maxEnergy.ToString();
        Debug.Log(curEnergy);
        //SetEnergy();
    }

    public void ResetEnergy()
    {
        curEnergy = 0;
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
            Debug.Log("0case????????");
            curEnergy = curEnergy - useEnergy > 0 ? curEnergy - useEnergy : 0;
        }     
        else if(i==999)
        {
            curEnergy = curEnergy > 0 ? curEnergy : 0;
        }
        else
        {
            Debug.Log("1case????????");
            curEnergy = curEnergy - i > 0 ? curEnergy - i : 0;
        }
        //UpgradeManager.instance.getEnergy(curEnergy);
        slider.value = (float)curEnergy / maxEnergy;
        eText.text = curEnergy.ToString() + " / " + maxEnergy.ToString();
        if (curEnergy <= 0)
        {
            curEnergy = -1;
            gameObject.SendMessage("Die");
            //StageManager.instance.GameOver();//GameOver_suicide();
            Debug.Log("gameOver");
            return false;
        }

        return true;
    }

    public int GetEnergy() { return curEnergy; }
}
