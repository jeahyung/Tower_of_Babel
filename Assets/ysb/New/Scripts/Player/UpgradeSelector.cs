using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeSelector : MonoBehaviour
{
    private UpgradeController upController;
    private int num;
    public int Num { set { num = value; } }

    //ui
    private Button btn;

    private TMP_Text upname;
    private TMP_Text explain;

    private Image icon;

    //private TMP_Text state;

    private void Awake()
    {
        upController = GetComponentInParent<UpgradeController>();

        upname = transform.GetChild(0).GetComponent<TMP_Text>();
        explain = transform.GetChild(1).GetComponent<TMP_Text>();

        icon = transform.GetChild(2).GetComponent<Image>();

        btn = GetComponent<Button>();
        //state = GetComponentInChildren<TMP_Text>();
        btn.onClick.AddListener(() => Select());
    }

    public void SetBtn(Upgrade up)
    {
        upname.text = "";
        explain.text = "";
        icon.sprite = null;

        upname.text = up.name;
        explain.text = up.explain;

        //아이콘 불러오기
        string path = "Data/Icon/";
        Sprite img = Resources.Load<Sprite>(path + up.id.ToString());

        icon.sprite = img;
        //state.text = up.state.ToString();
    }

    public void Select()
    {
        upController.SelectUpgrade(num);
    }
}
