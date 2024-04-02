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

    private void Awake()
    {
        maxEnergy = 20;
        curEnergy = maxEnergy;

        slider.value = (float)curEnergy / maxEnergy;

    }

    public void UseEnergy(int i)
    {
        curEnergy = curEnergy - i > 0 ? curEnergy - i : 0;
        slider.value = (float)curEnergy / maxEnergy;
        if(curEnergy <= 0)
        {
            Debug.Log("gameOver");
        }
    }
}
