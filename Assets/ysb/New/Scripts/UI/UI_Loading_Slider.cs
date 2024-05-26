using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Loading_Slider : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        slider.value = 0;
    }

    public void SetSliderValue(float v)
    {
        slider.value = v;
    }
}
