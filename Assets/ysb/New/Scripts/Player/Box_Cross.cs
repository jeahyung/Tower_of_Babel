using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Cross : MonoBehaviour
{
    public CheckBox_Player[] boxes;

    private void Awake()
    {
        boxes = GetComponentsInChildren<CheckBox_Player>();
    }

    public void UseCrossCheckBox()
    {
        foreach(var box in boxes)
        {
            box.CheckBlock();
        }
    }
}
