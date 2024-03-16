using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMouse : MonoBehaviour
{
    public Dictionary dictionary;
    public Transform canvas;
    public RectTransform obj;

    public bool isAct = false;

    private void Awake()
    {
        dictionary = GetComponent<Dictionary>();
    }
    // Update is called once per frame
    void Update()
    {
        if(isAct == false) { return; }
        if(Input.GetMouseButton(0))
        {
            dictionary.IsOpen = true;
            Vector3 tmpPosition = ScreenToCanvasPoint(Input.mousePosition);
            Vector3 dicPosition = new Vector3(0, tmpPosition.y - 450f, 0);

            obj.localPosition = dicPosition;
        }        
    }

    public Vector2 ScreenToCanvasPoint(Vector2 screenPosition)
    {
        Vector2 viewportPoint = Camera.main.ScreenToViewportPoint(screenPosition);
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        return (new Vector2(viewportPoint.x * canvasSize.x, viewportPoint.y * canvasSize.y) - (canvasSize / 2));
    }
}
