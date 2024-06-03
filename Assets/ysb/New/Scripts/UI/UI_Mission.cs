using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Mission : MonoBehaviour
{
    [SerializeField] private TMP_Text mission;  //�̼�
    private void Start()
    {
        mission = GetComponentInChildren<TMP_Text>();
        SetMission();
    }

    public void SetMission()
    {
        int part = StageManager.instance.GetChapterCount;
        if(part == 1) { mission.text = "2�� �̳��� ������ ��, 1000��. ���� 500���� ����."; }
        else if(part == 2) { mission.text = "3�� �̳��� ������ ��, 1500��. ���� 500���� ����."; }
        else if(part == 3) { mission.text = "4�� �̳��� ������ ��, 2000��. ���� 1000���� ����."; }
        else { mission.text = "5�� �̳��� ������ ��, 3000��. ���� 1000���� ����."; }
    }
    public void HideMission()
    {

    }
}
