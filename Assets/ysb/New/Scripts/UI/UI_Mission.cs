using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Mission : MonoBehaviour
{
    [SerializeField] private TMP_Text mission;  //미션
    private void Start()
    {
        mission = GetComponentInChildren<TMP_Text>();
        SetMission();
    }

    public void SetMission()
    {
        int part = StageManager.instance.GetChapterCount;
        if(part == 1) { mission.text = "2턴 이내에 도착할 시, 1000점. 이후 500점씩 차감."; }
        else if(part == 2) { mission.text = "3턴 이내에 도착할 시, 1500점. 이후 500점씩 차감."; }
        else if(part == 3) { mission.text = "4턴 이내에 도착할 시, 2000점. 이후 1000점씩 차감."; }
        else { mission.text = "5턴 이내에 도착할 시, 3000점. 이후 1000점씩 차감."; }
    }
    public void HideMission()
    {

    }
}
