using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAction 
{
    protected Map map;
    public SpecialAction(Map map)
    {
        this.map = map;
    }
    public virtual void Action() { }
}
public class SpecialAction_Straight : SpecialAction
{
    public SpecialAction_Straight(Map map): base(map) { }
    public override void Action()
    {
        map.FindTileInRange_Cross();
    }
}
public class SpecialAction_Cross:SpecialAction
{
    public SpecialAction_Cross(Map map) : base(map) { }
    public override void Action()
    {
        map.FindTileInRange_Cross();
    }
}
public class SpecialAction_King:SpecialAction
{
    public SpecialAction_King(Map map) : base(map) { }
    public override void Action()
    {
        map.FindTileInRange_Eight(1);
    }
}

public class UpgradeController : MonoBehaviour
{
    //private UpgradeManager manager_Up;

    private Transform paenl;
    private UpgradeSelector[] selectors = new UpgradeSelector[3];   //선택버튼

    public Map map;
    public Button actionBtn;

    [SerializeField]
    private List<Upgrade> upgrades = new List<Upgrade>();   //업그레이드 목록
    private List<Upgrade> selectedUp = new List<Upgrade>(); //선택된 업그레이드 목록

    public int selectCount = 3;

    public bool isFirst = true; //처음 선택?

    private void Awake()
    {
        //manager_Up = FindObjectOfType<UpgradeManager>();
        paenl = transform.Find("Panel").transform;
        map = FindObjectOfType<Map>();

        //버튼 세팅
        selectors = GetComponentsInChildren<UpgradeSelector>();
        for(int i = 0; i < 3; ++i)
        {
            selectors[i].Num = i;
        }
        paenl.localScale = new Vector3(0, 1, 1);
    }

    public void SetUpgrade(List<Upgrade> up)
    {
        upgrades = up;
    }

    public void SetSelectList()
    {
        selectedUp.Clear();

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

    public void SetAction(SpecialAction action)
    {
        actionBtn.onClick.AddListener(() => action.Action());
    }

    public void SelectUpgrade(int i)
    {
        //if(isFirst == true)
        //{
        //    SetAction(SelectAction(i));
        //    return;
        //}

        //선택
        Upgrade up = selectedUp[i];
        UpgradeManager.instance.AddUpgrade(up);

        //remove
        selectedUp.Remove(up);
        upgrades.AddRange(selectedUp);

        //ui
        paenl.localScale = new Vector3(0, 1, 1);
    }

    //업그레이드 매니저로
    public SpecialAction SelectAction(int i)
    {
        if(i == 0)
        {
            SpecialAction_Straight action = new SpecialAction_Straight(map);
            return action;
        }
        if(i == 1)
        {
            SpecialAction_Cross action = new SpecialAction_Cross(map);
            return action;
        }
        else
        {
            SpecialAction_King action = new SpecialAction_King(map);
            return action;
        }
    }
}
