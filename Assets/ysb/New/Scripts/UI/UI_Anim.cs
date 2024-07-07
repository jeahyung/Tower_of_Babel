using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Anim : MonoBehaviour
{
    public GameObject turnui;
    private void Awake()
    {
        turnui = transform.parent.gameObject;
    }
    public void EndUI()
    {
        turnui.SetActive(false);
    }
}
