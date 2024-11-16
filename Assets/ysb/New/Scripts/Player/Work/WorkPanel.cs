using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Work
{
    public int id;
    public string name;
    public string explain;
    public string word;   
    public string cnt;

    public int score;
    public int move;
    public int item;
    public int itemuse;
}
public class WorkPanel : MonoBehaviour
{
    private UpgradeController mgr_up;

    private List<Work> works = new List<Work>();
    [SerializeField]private List<Sprite> imgdata = new List<Sprite>();

    private List<WorkButton> wBtn = new List<WorkButton>();

    [SerializeField] private Image workImg;

    [SerializeField] private TMP_Text nText;    //name
    [SerializeField] private TMP_Text exText;   //설명
    [SerializeField] private TMP_Text howText;   //설명
    [SerializeField] private TMP_Text cntText;   //사용횟수
                                                 // [SerializeField] private TMP_Text wText;    //한마디


    private int selNum = 2; //선택한 번호
    private Player_Move playerMove;

    private void Awake()
    {   
        playerMove = FindObjectOfType<Player_Move>();
        mgr_up = GetComponentInParent<UpgradeController>();
        SetData();

        wBtn.AddRange(GetComponentsInChildren<WorkButton>());
        for(int i = 0; i < wBtn.Count;++i)
        {
            wBtn[i].SetData(i, works[i].name);
        }

        wBtn[0].SelectWork();


    }
    private void Start()
    {
        StageManager.instance.PlayerMoving(false);
    }


    public void ResetPanel()
    {
        wBtn[0].SelectWork();
        //SetInfo(0);
    }
    private void SetData()
    {
        works.Clear();

        //킹
        Work w3 = new Work();
        w3.id = 0;
        w3.name = "안  |  An";
        w3.explain = "균형있는 능력으로 직관적인 플레이";
        w3.word = "맵 전역으로 십자 방향 이동";
        w3.cnt = "스테이지당 2회 사용";
        works.Add(w3);

        //비숍
        Work w2 = new Work();
        w2.id = 1;
        w2.name = "엔키  |  Enki";
        w2.explain = "뛰어난 기동력으로 최대한 멀리 도달";
        w2.word = "맵 전역으로 대각 방향 이동";
       
        w2.cnt = "스테이지당 2회 사용";


        works.Add(w2);

        //룩
        Work w = new Work();
        w.id = 2;
        w.name =  "아다드  |  Adad";
        w.explain = "자유로운 이동으로 다양한 플레이 시도";
        w.word = "전 방향으로 1칸 이동 및 추가 턴 획득";
      
        w.cnt = "스테이지당 4회 사용";


        works.Add(w);

        //Sprite img = Resources.Load<Sprite>("Data/Icon/Work/0");
        //imgdata.Add(img);
    }

    public void SetInfo(int i)
    {
        selNum = i;
        workImg.sprite = imgdata[i];

        nText.text = works[i].name;
        exText.text = works[i].explain;
        howText.text = works[i].word;
        cntText.text = works[i].cnt;
     //   wText.text = works[i].word;


        for (int j = 0; j < wBtn.Count; ++j)
        {
            if(i == j) { continue; }
            wBtn[j].ActiveBtn();
        }
    }

    public void SelectWork()
    {
        mgr_up.SelectAction(selNum);
        playerMove.CheckAndMoving();
    }
}
