using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UpgradeController : MonoBehaviour
{
    //private UpgradeManager manager_Up;

    private Transform paenl;
    private UpgradeSelector[] selectors = new UpgradeSelector[3];   //���ù�ư

    [SerializeField]
    private List<Upgrade> upgrades = new List<Upgrade>();   //���׷��̵� ���
    private List<Upgrade> selectedUp = new List<Upgrade>(); //���õ� ���׷��̵� ���

    public int selectCount = 3;

    private void Awake()
    {
        //manager_Up = FindObjectOfType<UpgradeManager>();
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
    }
    public void SetUpgrade(List<Upgrade> up)
    {
        upgrades.Clear();
        upgrades = up;
        Debug.Log("set" + upgrades.Count);
    }

    public void ShowUpgrade()
    {
        SetSelectList();
    }
    public void SetSelectList()
    {
        selectedUp.Clear();

        for(int i = 0; i < upgrades.Count; ++i)
        {
            Debug.Log(upgrades[i].state + "/" + upgrades[i].upType);
        }

        for(int i = 0; i < selectCount; ++i)
        {
            int rand = Random.Range(0, upgrades.Count);

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
        UpgradeManager.instance.AddUpgrade(up);

        //remove
        selectedUp.Remove(up);
        upgrades.AddRange(selectedUp);

        //ui
        paenl.localScale = new Vector3(0, 1, 1);
    }
}
