using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Btn_Suicide : MonoBehaviour
{
    public GameObject ui_suicide;
    void Start()
    {
        ui_suicide = GameObject.Find("Suicide_UI");
        ui_suicide.SetActive(false);
        GetComponent<Button>().onClick.AddListener(() => AreYouDie());//StageManager.instance.GameOver());
    }

    public void AreYouDie()
    {
        ui_suicide.SetActive(true);
    }

    public void OkIDIe()
    {
        CancleDie();
        StageManager.instance.GameOver_suicide();
    }
    public void CancleDie()
    {
        ui_suicide.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
