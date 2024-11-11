using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Rope : MonoBehaviour
{
    ItemManager item;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
