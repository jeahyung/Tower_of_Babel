using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectWordData : MonoBehaviour 
{
    [SerializeField]
    private Canvas canvas;

    private GraphicRaycaster graphicRaycaster;
	private PointerEventData pointerEventData;
	private List<RaycastResult> raycastResults;

	[SerializeField]
    private WordBookData wbd = null;

    private void Start()
    {
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(null);
        raycastResults = new List<RaycastResult>();
    }

    public WordData ClickWord()
    {
        raycastResults.Clear();

        pointerEventData.position = Input.mousePosition;
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            for (int i = 0; i < raycastResults.Count; ++i)
            {
                //이거 문자 데이터?
                if (raycastResults[i].gameObject.CompareTag("WordData"))
                {
                    wbd = raycastResults[i].gameObject.GetComponent<WordBookData>();
                    return wbd.WordData;
                }
            }
        }
        return null;
    }
}
