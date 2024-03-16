using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brazier : InteractionObject
{
    public Mural mural;
    public GameObject pointLight;
    public GameObject stageLight;

    private bool isOn = true;
    public override void InteractObject()
    {
        isOn = !isOn;
        pointLight.SetActive(isOn);
        stageLight.SetActive(isOn);

        if(isOn == true)
        {
            mural.OnLight();
        }
        else
        {
            mural.OffLight();
        }
    }
}
