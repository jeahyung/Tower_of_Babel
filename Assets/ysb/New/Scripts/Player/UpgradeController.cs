using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UpgradeController : MonoBehaviour
{
    private Transform paenl;
    private UpgradeSelector[] selectors = new UpgradeSelector[3];   //���ù�ư

    [SerializeField]
    private List<Upgrade> upgrades = new List<Upgrade>();   //���׷��̵� ���
    private List<Upgrade> selectedUp = new List<Upgrade>(); //���õ� ���׷��̵� ���

    public int selectCount = 3;

    [SerializeField] private Transform panel_action;    //�׼� ����â

    private void Awake()
    {
        upgrades.Clear();
        paenl = transform.Find("Panel").transform;        

        //��ư ����
        selectors = GetComponentsInChildren<UpgradeSelector>();
        for(int i = 0; i < 3; ++i)
        {
            selectors[i].Num = i;
        }
        paenl.localScale = new Vector3(0, 1, 1);
    }
    public void SetActionUpgrade(List<Upgrade> up)
    {
        upgrades.Clear();
        upgrades = up;
    }   //��

    //�׼� ���� - ��
    public void OpenActionPanel()
    {
        panel_action.localScale = new Vector3(1, 1, 1);
    }
    public void SelectAction(int i)
    {
        UpgradeManager.instance.SetAction(i);
        panel_action.localScale = new Vector3(0, 1, 1);
        panel_action.SendMessage("ResetPanel");

        AudioManager.instance.PlaySfx(AudioManager.Sfx.UI_Click);
    }


    //����ü
    public void SetUpgrade(List<Upgrade> up)
    {
        upgrades.Clear();
        upgrades.AddRange(up);
        Debug.Log("set upgrade : " + upgrades.Count);
    }

    //����ü ����
    public void OpenUpgradePanel()
    {
        SetSelectList();
    }

    private void SetSelectList()
    {
        selectedUp.Clear();
        for(int i = 0; i < selectCount; ++i)
        {
            int rand = Random.Range(0, upgrades.Count);
            Debug.Log("set upgrade : " + upgrades.Count + "/ " + rand);
            Upgrade up = upgrades[rand];
            selectedUp.Add(up);
            upgrades.Remove(up);

            //ui
            selectors[i].SetBtn(up);
        }
        paenl.localScale = new Vector3(1, 1, 1);
    }

    public void SelectUpgrade(int i)
    {
        //����
        Upgrade up = selectedUp[i];

        //remove
        selectedUp.Remove(up);
        upgrades.AddRange(selectedUp);

        //ui
        paenl.localScale = new Vector3(0, 1, 1);

        UpgradeManager.instance.AddUpgrade(up);
        UpgradeDatabase.instance.RemoveData(up);

        //StageManager.instance.EndGame();
    }
}
