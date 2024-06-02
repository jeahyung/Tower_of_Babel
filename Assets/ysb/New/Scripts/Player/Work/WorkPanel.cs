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
}
public class WorkPanel : MonoBehaviour
{
    private UpgradeController mgr_up;

    private List<Work> works = new List<Work>();
    private List<Sprite> imgdata = new List<Sprite>();

    private List<WorkButton> wBtn = new List<WorkButton>();

    [SerializeField] private Image workImg;

    [SerializeField] private TMP_Text nText;    //name
    [SerializeField] private TMP_Text exText;   //설명
    
    private int selNum = 0; //선택한 번호

    private void Awake()
    {
        mgr_up = GetComponentInParent<UpgradeController>();

        //nText = GameObject.Find("Name").GetComponent<TMP_Text>();
        //exText = GameObject.Find("Explain").GetComponent<TMP_Text>();
        //workImg = GameObject.Find("Img_Work").GetComponent<Image>();

        SetData();

        string path = "Data/Icon/Work/";
        for(int i = 0; i < works.Count; ++i)
        {
            Sprite img = Resources.Load<Sprite>(path + i.ToString());
            imgdata.Add(img);
        }

        wBtn.AddRange(GetComponentsInChildren<WorkButton>());
        for(int i = 0; i < wBtn.Count;++i)
        {
            wBtn[i].SetData(i, works[i].name);
        }
        wBtn[0].SelectWork();
        //SetInfo(0);
    }
    public void ResetPanel()
    {
        wBtn[0].SelectWork();
        //SetInfo(0);
    }
    private void SetData()
    {
        works.Clear();

        Work w = new Work();
        w.id = 0;
        w.name = "룩";
        w.explain = "4방향 이동";

        works.Add(w);

        Work w2 = new Work();
        w2.id = 1;
        w2.name = "비숍";
        w2.explain = "대각선 4방향 이동";

        works.Add(w2);

        Work w3 = new Work();
        w3.id = 2;
        w3.name = "킹";
        w3.explain = "8방향 이동. 1회 재행동.";

        works.Add(w3);
    }

    public void SetInfo(int i)
    {
        selNum = i;

        workImg.sprite = imgdata[i];

        nText.text = works[i].name;
        exText.text = works[i].explain;

        for (int j = 0; j < wBtn.Count; ++j)
        {
            if(i == j) { continue; }
            wBtn[j].ActiveBtn();
        }
    }

    public void SelectWork()
    {
        mgr_up.SelectAction(selNum);
    }
}
