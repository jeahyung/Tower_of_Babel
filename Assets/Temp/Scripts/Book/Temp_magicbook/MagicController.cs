using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MagicController : MonoBehaviour 
{
    //[SerializeField]
    //private Camera cam;
    //[SerializeField]
    //private RectTransform draggedImg;

    [SerializeField]
    private Canvas canvas;

    private GraphicRaycaster graphicRaycaster;
	private PointerEventData pointerEventData;
	private List<RaycastResult> raycastResults;

	[SerializeField]
	private GameObject clickWord = null;
    private WordBookData wbd = null;
    private RectTransform wordPos;
    private Transform wordParent;

    public PuzzleManager mp;

    public GameObject puzzleInput;

    private void Start()
    {
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(null);
        raycastResults = new List<RaycastResult>();

        puzzleInput.SetActive(false);
    }

    //private GameObject draggedItem;
    //private Transform draggedItemParent;

    //void Update()
    //{
    //	DragItems();
    //}

    //void DragItems()
    //{
    //	if (Input.GetMouseButtonDown(0))
    //	{
    //		pointerEventData.position = Input.mousePosition;
    //		graphicRaycaster.Raycast(pointerEventData, raycastResults);

    //		if(raycastResults.Count <= 0) { return; }

    //		foreach(var result in raycastResults)
    //           {
    //			Debug.Log(result.gameObject.name);
    //           }

    //		//if (raycastResults.Count > 0)
    //		//{


    //		//	if (raycastResults[0].gameObject.GetComponent<Item>())
    //		//	{
    //		//		draggedItem = raycastResults[0].gameObject;
    //		//		draggedItemParent = draggedItem.transform.parent;
    //		//		draggedItem.transform.SetParent(UIManager.instance.canvas);
    //		//	}
    //		//	//if raycast result is not an item
    //		//	else
    //		//	{
    //		//		raycastResults.Clear();
    //		//	}
    //		//}
    //	}

    //	//check if dragged item is null
    //	if (draggedItem == null) return;

    //	//Item follows mouse
    //	if (draggedItem != null)
    //	{
    //		draggedItem.GetComponent<RectTransform>().localPosition = ScreenToCanvasPoint(Input.mousePosition);
    //	}

    //	//End Dragging
    //	//if (Input.GetMouseButtonUp(0))
    //	//{
    //	//	pointerEventData.position = Input.mousePosition;
    //	//	raycastResults.Clear();
    //	//	graphicRaycaster.Raycast(pointerEventData, raycastResults);

    //	//	//Set old parent
    //	//	draggedItem.transform.SetParent(draggedItemParent);

    //	//	if (raycastResults.Count > 0)
    //	//	{
    //	//		foreach (var result in raycastResults)
    //	//		{
    //	//			//Skip the draggedItem when it is the result
    //	//			if (result.gameObject == draggedItem) continue;

    //	//			//Empty Slot
    //	//			//if (result.gameObject.CompareTag("Slot"))
    //	//			//{
    //	//			//	//Set New Parent
    //	//			//	draggedItem.transform.SetParent(result.gameObject.transform);
    //	//			//	break;
    //	//			//}
    //	//			//Hotkey
    //	//			//if (result.gameObject.CompareTag("Hotkey"))
    //	//			//{
    //	//			//	result.gameObject.GetComponent<Hotkey>().SetUsable(draggedItem.GetComponent<IUsable>());
    //	//			//	break;
    //	//			//}
    //	//			////Another Item
    //	//			//if (result.gameObject.CompareTag("ItemIcon"))
    //	//			//{
    //	//			//	//Swap Items
    //	//			//	if (result.gameObject.name != draggedItem.name)
    //	//			//	{
    //	//			//		draggedItem.transform.SetParent(result.gameObject.transform.parent);
    //	//			//		result.gameObject.transform.SetParent(draggedItemParent);
    //	//			//		result.gameObject.transform.localPosition = Vector3.zero;
    //	//			//		break;
    //	//			//	}
    //	//			//	//Stack items (IF THE ARE THE SAME)
    //	//			//	else
    //	//			//	{
    //	//			//		result.gameObject.GetComponent<Item>().quantity += draggedItem.GetComponent<Item>().quantity;
    //	//			//		result.gameObject.transform.Find("QuantityText").GetComponent<Text>().text = result.gameObject.GetComponent<Item>().quantity.ToString();
    //	//			//		GameObject.Destroy(draggedItem);
    //	//			//		draggedItem = null;
    //	//			//		raycastResults.Clear();
    //	//			//		return;
    //	//			//	}
    //	//			//}
    //	//		}
    //	//	}
    //	//	//Reset position to zero
    //	//	draggedItem.transform.localPosition = Vector3.zero;
    //	//	draggedItem = null;
    //	//}

    //	//raycastResults.Clear();
    //}

    //public void SetDraggedImage(Vector3 pos)
    //{
    //    draggedImg.gameObject.SetActive(true);
    //    draggedImg.localPosition = pos;
    //}
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            raycastResults.Clear();

            pointerEventData.position = Input.mousePosition;
            graphicRaycaster.Raycast(pointerEventData, raycastResults);

            if (raycastResults.Count > 0)
            {
                for(int i = 0; i< raycastResults.Count; ++i)
                {
                    //Debug.Log(raycastResults[i].gameObject.name);
                    //이거 문자 데이터임?
                    if (raycastResults[i].gameObject.CompareTag("WordData"))
                    {
                        clickWord = raycastResults[i].gameObject.GetComponent<WordBookData>().WordObject;
                        wbd = raycastResults[i].gameObject.GetComponent<WordBookData>();
                        wordParent = clickWord.transform.parent;
                        //Debug.Log(wordParent.name);
                        clickWord.transform.SetParent(canvas.transform);
                        break;
                    }
                    //이거 조합된 문자임?
                    if(raycastResults[i].gameObject.CompareTag("Combinate"))
                    {
                        clickWord = raycastResults[i].gameObject;

                        if(clickWord.GetComponent<MagicCanMgr>().isMax != true)
                        {
                            clickWord = null;
                            break;
                        }
                        puzzleInput.SetActive(true);
                        Debug.Log(clickWord.name);
                        mp.MoveBook();
                        wordParent = clickWord.transform.parent;
                        clickWord.transform.SetParent(canvas.transform);
                        break;
                    }
                }
            }            
        }
        //이미지 이동
        if (clickWord == null) return;
        clickWord.GetComponent<RectTransform>().localPosition = ScreenToCanvasPoint(Input.mousePosition);

        //드래그 종료
        if (Input.GetMouseButtonUp(0))
        {
            pointerEventData.position = Input.mousePosition;
            raycastResults.Clear();
            graphicRaycaster.Raycast(pointerEventData, raycastResults);

            clickWord.transform.SetParent(wordParent);

            if (raycastResults.Count > 0)
            {
                foreach (var result in raycastResults)
                {
                    Debug.Log(result.gameObject.name);
                    if (result.gameObject.CompareTag("MixCan"))
                    {
                        result.gameObject.GetComponent<MagicCan>().ChangeWord(wbd.WordData);
                        break;
                    }

                    if(clickWord.GetComponent<MagicCanMgr>()!=null && result.gameObject.CompareTag("PuzzleCan"))
                    {
                        result.gameObject.SendMessage("SetWord", clickWord.GetComponent<MagicCanMgr>().GetWord());
                    }
                }
            }
            clickWord.transform.localPosition = Vector3.zero + new Vector3(90f, 0f, 0f);
            if(clickWord.GetComponent<MagicCanMgr>() != null)
            {
                clickWord.SendMessage("SetPos");
            }
            clickWord = null;
        }


        //if (Input.GetMouseButton(0))
        //      {
        //          if (wordPos.gameObject.activeSelf == false) { return; }
        //          wordPos.localPosition = ScreenToCanvasPoint(Input.mousePosition);
        //      }
    }

    public Vector2 ScreenToCanvasPoint(Vector2 screenPosition)   //스크린 좌표를 뷰포터 좌표로 변환 -> 인벤토리에 사용됨
    {
        Vector2 viewportPoint = Camera.main.ScreenToViewportPoint(screenPosition);
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        return (new Vector2(viewportPoint.x * canvasSize.x, viewportPoint.y * canvasSize.y) - (canvasSize / 2));
    }
}
