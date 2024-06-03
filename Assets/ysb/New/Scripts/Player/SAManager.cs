using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#region SpecialAction
public class SpecialAction
{
    protected Map map;
    public int count;
    public int score;
    public int energy;
    public SpecialAction(Map map)
    {
        this.map = map;
    }
    public virtual void Action() { }
}
public class SA_Rock : SpecialAction
{
    public SA_Rock(Map map) : base(map) { }

    public override void Action()
    {
        map.HideArea();
        map.FindTileInRange_FourLong();
    }
}
public class SA_Bishop : SpecialAction
{
    public SA_Bishop(Map map) : base(map) { }
    public override void Action()
    {
        map.HideArea();
        map.FindTileInRange_Cross();
    }
}
public class SA_King : SpecialAction
{
    public SA_King(Map map) : base(map) { }
    public override void Action()
    {
        map.HideArea();
        map.FindTileInRange_Eight(null, 1);
    }
}
#endregion

public class SAManager : MonoBehaviour
{
    private Map map;
    private SpecialAction action;
    private EnergySystem es;
    private int actCount = 3;
    private bool isKing = false;

    public float offset = 0.2f;
    public bool usedKing => isKing;
    public int getActCount => actCount;

    #region UI
    private Button actionBtn;
    public float radius = 130f;

    public List<UI_ActCount> UI_actCount = new List<UI_ActCount>(); //사용횟수 ui


    //private TMP_Text countText;
    [SerializeField]
    private GameObject cancelBtn;
    #endregion
    private void Awake()
    {
        map = FindObjectOfType<Map>();
        es = FindObjectOfType<EnergySystem>();

        actionBtn = GetComponent<Button>();
        UI_actCount.AddRange(GetComponentsInChildren<UI_ActCount>());
        for(int i = 0; i < UI_actCount.Count; ++i)
        {
            UI_actCount[i].gameObject.SetActive(false);
        }
        //countText = actionBtn.transform.Find("count").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        //플레이 전까진 액션 버튼은 비활성화합니다.
        ActActionBtn(false);
    }

    #region UI
    public void SetActCountUI(int c)
    {
        float degree = Mathf.PI * c * offset;
        for (int i = 0; i < c; i++)
        {
            UI_actCount[i].GetComponent<RectTransform>().anchoredPosition
                = new Vector3(0, 0, 0);

            float angle = Mathf.PI * 0.3f - (i - 1) * (degree / c);

            Vector3 pos = UI_actCount[i].GetComponent<RectTransform>().anchoredPosition;
            UI_actCount[i].GetComponent<RectTransform>().anchoredPosition
                = pos + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            UI_actCount[i].gameObject.SetActive(true);
            UI_actCount[i].OffSprite(false);
        }
    }
    #endregion

    public void SetAct()
    {
        actionBtn.onClick.RemoveAllListeners();

        ActActionBtn(true);
        int num = UpgradeManager.instance.GetSANum();

        //룩(직진)
        if (num == 0) { 
            SA_Rock ac = new SA_Rock(map);
            action = ac;
            action.count = 2;
            action.energy = 3;
        }

        //비숍(대각선)
         else if (num == 1) { 
            SA_Bishop ac = new SA_Bishop(map);
            action = ac;
            action.count = 2;
            action.energy = 4;
        }

        //킹(2번
        else if(num == 2) { 
            SA_King ac = new SA_King(map);
            action = ac;
            action.count = 4;
            action.energy = 1;
        }

        //업그레이드 효과
        actCount = action.count;
        actCount = actCount + UpgradeManager.instance.getBonusCount();

        //UI - act count
        SetActCountUI(actCount);
        //countText.text = "(" + actCount.ToString() + ")";

        actionBtn.onClick.AddListener(() => action.Action());
        actionBtn.onClick.AddListener(() => UseAction());
    }    

    public void BonusUse()
    {
        action.Action();
        isKing = false;
    }

    public void CheckActionCount()
    {
        if (actCount <= 0)
        {
            actionBtn.onClick.RemoveAllListeners();
        }
    }
    public void UseAction()
    {
        if(actionBtn.enabled == false) { return; }
        map.useAction = true;
        actionBtn.enabled = false;

        es.useEnergy = action.energy;

        if (UpgradeManager.instance.GetSANum() == 2)    //킹
        {
            UpgradeManager.instance.getBonusTurn(1);
            isKing = true;
        }
        UI_actCount[actCount - 1].OffSprite(true);  //기회 감소
        actCount--;
        cancelBtn.SetActive(true);
    }

    public void ActDone()
    {
        actionBtn.enabled = true;
        cancelBtn.SetActive(false);
    }
    public void ActCancel()
    {
        actCount++;
        UI_actCount[actCount - 1].OffSprite(false);
        if (UpgradeManager.instance.GetSANum() == 2)    //킹
        {
            UpgradeManager.instance.getBonusTurn(-1);
            isKing = false;
        }


        ActDone();
    }

    //킹에 의한 추가 행동
    public void UseAction_Bonus()
    {
        map.useAction = true;
        actionBtn.enabled = false;

        action.Action();
    }

    //액션 버튼 동작 여부
    public void SetActionBtn(bool b)
    {
        actionBtn.enabled = b;
    }

    //액션 버튼 활성화 여부
    public void ActActionBtn(bool b)
    {
        //actionBtn.gameObject.SetActive(b);
        actionBtn.enabled = b;
        cancelBtn.SetActive(false);
    }

    public void Score_Action()
    {
        //ScoreManager.instance.Score_SACount(actCount);
    }
}
