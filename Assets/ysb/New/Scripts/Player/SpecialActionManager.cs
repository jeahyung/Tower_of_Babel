using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#region SpecialAction
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
    public SpecialAction_Straight(Map map) : base(map) { }
    public override void Action()
    {
        map.HideArea();
        map.FindTileInRange_FourLong();
    }
}
public class SpecialAction_Cross : SpecialAction
{
    public SpecialAction_Cross(Map map) : base(map) { }
    public override void Action()
    {
        map.HideArea();
        map.FindTileInRange_Cross();
    }
}
public class SpecialAction_King : SpecialAction
{
    public SpecialAction_King(Map map) : base(map) { }
    public override void Action()
    {
        map.HideArea();
        map.FindTileInRange_Eight(null, 1);
    }
}
#endregion

public class SpecialActionManager : MonoBehaviour
{
    private TurnManager manager_Turn;
    private Map map;

    private Button actionBtn;
    [SerializeField]
    private TMP_Text countText;
    [SerializeField]
    private GameObject cancelBtn;

    private SpecialAction action;
    private int actCount = 3;

    private bool isUse = false;
    public bool IsAct => isUse;


    private void Awake()
    {
        map = FindObjectOfType<Map>();
        manager_Turn = FindObjectOfType<TurnManager>();

        actionBtn = GetComponent<Button>();
        countText = actionBtn.transform.Find("count").GetComponent<TMP_Text>();

        //행동 설정
        //SetAct();

        //actCount = 
        countText.text = "(" + actCount.ToString() + ")";
    }

    public void SetAct()
    {
        if (UpgradeManager.instance.num == 0)
        {
            SpecialAction_Straight ac = new SpecialAction_Straight(map);
            action = ac;
        }
        else if (UpgradeManager.instance.num == 1)
        {
            SpecialAction_Cross ac = new SpecialAction_Cross(map);
            action = ac;
        }
        else
        {
            SpecialAction_King ac = new SpecialAction_King(map);
            action = ac;
        }
        actionBtn.onClick.AddListener(() => action.Action());
        actionBtn.onClick.AddListener(() => UseAction());
    }    
    public void UseAction()
    {
        map.useAction = true;
        actionBtn.enabled = false;

        actCount--;
        countText.text = "(" + actCount.ToString() + ")";
        if(actCount <= 0)
        {
            actionBtn.onClick.RemoveAllListeners();
            //actionBtn.enabled = false;
        }
        cancelBtn.SetActive(true);
    }

    public void ActDone()
    {
        actionBtn.enabled = true;
        cancelBtn.SetActive(false);
    }
}
