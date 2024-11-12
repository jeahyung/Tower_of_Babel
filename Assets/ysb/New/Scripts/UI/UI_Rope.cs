using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Rope : MonoBehaviour
{
    ItemManager item;
    public TMP_Text info;
    public bool isOk = true;
    // Start is called before the first frame update
    void Start()
    {
        item = FindObjectOfType<ItemManager>();
        if(isOk == true)
        {
            GetComponent<Button>().onClick.AddListener(() => item.UseItem());
        }
        else
        {
            GetComponent<Button>().onClick.AddListener(() => item.HideRopeUI());
        }
    }

    public void SettingInfo(string mob)
    {
        info.text = mob + "몬스터를 모두 1턴간 속박한다.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
