using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CamToggle : MonoBehaviour
{
    Image tImg;
    public Sprite[] img;

    private void Awake()
    {
        tImg = GetComponent<Image>();
        //tImg.sprite = img[0];
    }

    public void OnToggle()
    {
        tImg.sprite = img[0];

    }
    public void OffToggle()
    {
        tImg.sprite = img[1];
    }
}
