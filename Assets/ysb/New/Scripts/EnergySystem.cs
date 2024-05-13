using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : MonoBehaviour
{
    [SerializeField]
    private int maxEnergy;
    [SerializeField]
    private int curEnergy;

    [SerializeField]
    private Slider slider;

    public int useEnergy = 1;

    private void Awake()
    {
        maxEnergy = 50;
        curEnergy = maxEnergy;

        slider.value = (float)curEnergy / maxEnergy;

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

        slider.value = (float)curEnergy / maxEnergy;
        if(curEnergy <= 0)
        {
            Debug.Log("gameOver");
        }
    }
}
