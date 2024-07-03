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

    private Vector3 baseScale;
    private Vector3 bigScale;

    public void SetData(int i, string name)
    {
        wText = GetComponentInChildren<TMP_Text>();
        work = GetComponentInParent<WorkPanel>();

        baseScale = transform.localScale;
        bigScale = baseScale * 1.2f;

        btn = GetComponent<Button>();

        num = i;
        wText.text = name;

        btn.onClick.AddListener(() => SelectWork());
    }

    public void SelectWork()
    {
        work.SetInfo(num);
        btn.interactable = false;
        transform.localScale = baseScale;
    }

    public void ActiveBtn()
    {
        btn.interactable = true;

    }

    public void OnPointer()
    {
        if(btn.interactable == false) { transform.localScale = baseScale; return; }
        transform.localScale = bigScale;
    }
    public void ExitPointer()
    {
        transform.localScale = baseScale;
    }
}
