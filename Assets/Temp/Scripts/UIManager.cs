using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject InteractMessage;

    private void Awake()
    {
        InteractMessage = GameObject.Find("InteractMessage");
        InteractMessage.SetActive(false);
    }
    public void ShowInteractMessage(bool active)
    {
        InteractMessage.SetActive(active);
    }
}
