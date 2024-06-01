using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WorkButton : MonoBehaviour
{
    private WorkPanel work;

    private int num;
    private TMP_Text wText;
    private Button btn;

    public void SetData(int i, string name)
    {
        wText = GetComponentInChildren<TMP_Text>();
        work = GetComponentInParent<WorkPanel>();

        btn = GetComponent<Button>();

        num = i;
        wText.text = name;

        btn.onClick.AddListener(() => SelectWork());
    }

    public void SelectWork()
    {
        work.SetInfo(num);
        btn.interactable = false;
    }

    public void ActiveBtn()
    {
        btn.interactable = true;

    }
}
