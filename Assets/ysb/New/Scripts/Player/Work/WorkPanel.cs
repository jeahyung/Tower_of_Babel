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

    public int score;
    public int move;
}
public class WorkPanel : MonoBehaviour
{
    private UpgradeController mgr_up;
    private Player_Move playerMove;

    private List<Work> works = new List<Work>();
    [SerializeField]private List<Sprite> imgdata = new List<Sprite>();

    private List<WorkButton> wBtn = new List<WorkButton>();

    [SerializeField] private Image workImg;

    [SerializeField] private TMP_Text nText;    //name
    [SerializeField] private TMP_Text exText;   //설명
    [SerializeField] private TMP_Text wText;    //한마디

    [SerializeField] private GameObject[] sImg;
    [SerializeField] private GameObject[] mImg;

    private int selNum = 2; //선택한 번호

    private void Awake()
    {
        mgr_up = GetComponentInParent<UpgradeController>();
        playerMove = FindObjectOfType<Player_Move>();
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
        w3.explain = "다재다능하다.  다양한 시도를 하는 것이 중요하다.";
        w3.word = "무엇이든 후회하지 않는 강인한 마음가짐";
        w3.score = 3;
        w3.move = 4;
        works.Add(w3);

        //비숍
        Work w2 = new Work();
        w2.id = 1;
        w2.name = "엔키  |  Enki";
        w2.explain = "뛰어난 이동 능력을 통해 후반을 도모하라.";
        w2.word = "살고자 하는 의지가 가장 중요한 법";

        w2.score = 2;
        w2.move = 5;

        works.Add(w2);

        //룩
        Work w = new Work();
        w.id = 2;
        w.name =  "아다드  |  Adad";
        w.explain = "초반에 다양한 행동을 통해 점수를 획득하라.";
        w.word = "힘 앞에서는 그 어떤 것도 의미 없다";

        w.score = 5;
        w.move = 2;

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
        wText.text = works[i].word;

        for (int j = 0; j < 5; ++j)
        {
            sImg[j].SetActive(false);
            mImg[j].SetActive(false);
        }
        for (int j = 0; j < works[i].score; ++j)
        {
            sImg[j].SetActive(true);
        }
        for(int j = 0; j < works[i].move; ++j)
        {
            mImg[j].SetActive(true);
        }

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
