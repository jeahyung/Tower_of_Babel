using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookPage : MonoBehaviour
{
    private RectTransform page;
    private Vector3 onPage = new Vector3(1, 1, 1);
    private Vector3 offPage = new Vector3(0, 1, 1);

    private void Awake()
    {
        page = this.GetComponent<RectTransform>();
        page.localScale = offPage;
    }
    public void OnPage()
    {
        page.localScale = onPage;
    }
    public void OffPage()
    {
        page.localScale = offPage;
    }
}
