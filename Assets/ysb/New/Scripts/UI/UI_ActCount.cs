using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActCount : MonoBehaviour
{
    [SerializeField] private Sprite[] img = new Sprite[2];



    public void OffSprite(bool off)
    {
        if(off == true)
        {
            GetComponent<Image>().sprite = img[1];
        }
        else
        {
            GetComponent<Image>().sprite = img[0];
        }
    }
}
