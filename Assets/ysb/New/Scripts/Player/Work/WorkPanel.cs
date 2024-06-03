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

    private List<Work> works = new List<Work>();
    [SerializeField]private List<Sprite> imgdata = new List<Sprite>();

    private List<WorkButton> wBtn = new List<WorkButton>();

    [SerializeField] private Image workImg;

    [SerializeField] private TMP_Text nText;    //name
    [SerializeField] private TMP_Text exText;   //����
    [SerializeField] private TMP_Text wText;    //�Ѹ���

    [SerializeField] private GameObject[] sImg;
    [SerializeField] private GameObject[] mImg;

    private int selNum = 0; //������ ��ȣ

    private void Awake()
    {
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

        Work w = new Work();
        w.id = 0;
        w.name = "��  |  An";
        w.explain = "�ʹݿ� �پ��� �ൿ�� ���� ������ ȹ���϶�.";
        w.word = "�� �տ����� �� � �͵� �ǹ� ����";

        w.score = 5;
        w.move = 2;

        works.Add(w);

        Work w2 = new Work();
        w2.id = 1;
        w2.name = "��Ű  |  Enki";
        w2.explain = "�پ �̵� �ɷ��� ���� �Ĺ��� �����϶�.";
        w2.word = "����� �ϴ� ������ ���� �߿��� ��";

        w2.score = 2;
        w2.move = 5;

        works.Add(w2);

        Work w3 = new Work();
        w3.id = 2;
        w3.name = "�ƴٵ�  |  Adad";
        w3.explain = "����ٴ��ϴ�.  �پ��� �õ��� �ϴ� ���� �߿��ϴ�.";
        w3.word = "�����̵� ��ȸ���� �ʴ� ������ ��������";
        w3.score = 3;
        w3.move = 4;
        works.Add(w3);

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
    }
}
