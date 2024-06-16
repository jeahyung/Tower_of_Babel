using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Cam : MonoBehaviour
{
    List<UI_CamToggle> toggle = new List<UI_CamToggle>();
    int curToggle = 0;

    private void Awake()
    {
        toggle.AddRange(GetComponentsInChildren<UI_CamToggle>());
    }

    public void ChangeCam()
    {
        toggle[curToggle].OffToggle();
        curToggle++;
        if (curToggle >= toggle.Count) { curToggle = 0; }
        toggle[curToggle].OnToggle();
    }

    public void BackMainCam()
    {
        
        for (int i = 1; i < toggle.Count; ++i)
        {
            toggle[i].OffToggle();
        }
        toggle[0].OnToggle();
        curToggle = 0;
    }
}
