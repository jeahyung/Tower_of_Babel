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
    private TMP_Text btnText;

    private void Awake()
    {
        upController = GetComponentInParent<UpgradeController>();

        btn = GetComponent<Button>();
        btnText = GetComponentInChildren<TMP_Text>();
        btn.onClick.AddListener(() => Select());
    }

    public void SetBtn(Upgrade up)
    {
        btnText.text = up.state.ToString();
    }

    public void Select()
    {
        upController.SelectUpgrade(num);
    }
}
