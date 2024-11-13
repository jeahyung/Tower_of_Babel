using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Clock : MonoBehaviour
{
    ItemManager item;
    public int i;
    public TMP_Text info;
    // Start is called before the first frame update
    void Start()
    {
        item = FindObjectOfType<ItemManager>();
    }

    public void Ok()
    {
        item.UseItem();
        gameObject.SetActive(false);
    }
    public void Cancle()
    {
        item.CancelItem();
        gameObject.SetActive(false);
    }

    public void SetRandBoxText()
    {
        //info.text = ScoreManager.instance.CalculateBoxScore().ToString() + "ÀÇ Á¡¼ö¸¦ È¹µæÇÕ´Ï´Ù.";
    }
}
