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
        w.name = "�� | An";
        w.explain = "���� | �ʹݿ� �پ��� �ൿ�� ���� ������ ȹ���϶�.";
        w.word = "�� �տ����� �� � �͵� �ǹ� ����";

        works.Add(w);

        Work w2 = new Work();
        w2.id = 1;
        w2.name = "��Ű | Enki";
        w2.explain = "���� | �پ �̵� �ɷ��� ���� �Ĺ��� �����϶�.";
        w2.word = "����� �ϴ� ������ ���� �߿��� ��";

        works.Add(w2);

        Work w3 = new Work();
        w3.id = 2;
        w3.name = "�ƴٵ� | Adad";
        w3.explain = "����ٴ��ϴ�. �پ��� �õ��� �ϴ� ���� �߿��ϴ�.";
        w3.word = "�����̵� ��ȸ���� �ʴ� ������ ��������";

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
