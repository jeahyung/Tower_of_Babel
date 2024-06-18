using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UpgradeController : MonoBehaviour
{
    private Transform paenl;
    private UpgradeSelector[] selectors = new UpgradeSelector[3];   //선택버튼
    private SAManager sa;

    [SerializeField]
    private List<Upgrade> upgrades = new List<Upgrade>();   //업그레이드 목록
    private List<Upgrade> selectedUp = new List<Upgrade>(); //선택된 업그레이드 목록

    public int selectCount = 3;

    [SerializeField] private Transform panel_action;    //액션 선택창

    [SerializeField] public List<Sprite> img = new List<Sprite>();

    private UiWH uiwh;

    private void Awake()
    {
        upgrades.Clear();
        uiwh = GetComponent<UiWH>();
        if (uiwh == null)
        {
            Debug.Log("uiwh할당 안됨!!!!!!!!!!!!");
            uiwh = FindObjectOfType<UiWH>();
        }
        sa = FindObjectOfType<SAManager>();
        paenl = transform.Find("Panel").transform;        

        //버튼 세팅
        selectors = GetComponentsInChildren<UpgradeSelector>();
        for(int i = 0; i < 3; ++i)
        {
            selectors[i].Num = i;
        }
        paenl.localScale = new Vector3(0, 1, 1);
    }

    private void Update()
    {
        if(panel_action.localScale.x > 0)
        {
            StageManager.instance.PlayerMoving(false);
        }
    }
    public void SetActionUpgrade(List<Upgrade> up)
    {
        upgrades.Clear();
        upgrades = up;
    }   //구

    //액션 선택 - 신
    public void OpenActionPanel()
    {
        StageManager.instance.PlayerMoving(false);
        panel_action.localScale = new Vector3(1, 1, 1);
    }
    public void SelectAction(int i)
    {
        UpgradeManager.instance.SetAction(i);

        sa.SetAct();
        panel_action.localScale = new Vector3(0, 1, 1);
        panel_action.SendMessage("ResetPanel");

        StageManager.instance.PlayerMoving(true);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.UI_Click);
    }


    //증강체
    public void SetUpgrade(List<Upgrade> up)
    {
        upgrades.Clear();
        upgrades.AddRange(up);
        Debug.Log("set upgrade : " + upgrades.Count);
    }

    //증강체 오픈
    public void OpenUpgradePanel()
    {        
        StageManager.instance.PlayerMoving(false);
        SetSelectList();
        uiwh.UIImageSize(0.5f);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.UiOpen);
    }

    private void SetSelectList()
    {
        selectedUp.Clear();
        for(int i = 0; i < selectCount; ++i)
        {
            int rand = Random.Range(0, upgrades.Count);
            //Debug.Log("set upgrade : " + upgrades.Count + "/ " + rand);
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
        //선택
        Upgrade up = selectedUp[i];

        //remove
        selectedUp.Remove(up);
        upgrades.AddRange(selectedUp);

        //ui
        paenl.localScale = new Vector3(0, 1, 1);
        uiwh.ResetUIImageSize();

        UpgradeManager.instance.AddUpgrade(up);
        UpgradeDatabase.instance.RemoveData(up);

        sa.SetAct();

        StageManager.instance.PlayerMoving(true);
        //StageManager.instance.EndGame();
    }
}
