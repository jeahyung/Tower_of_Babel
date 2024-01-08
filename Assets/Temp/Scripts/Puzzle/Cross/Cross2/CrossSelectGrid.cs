using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CrossSelectGrid : MonoBehaviour
{
    private CrossSelectPuzzleManager mgr_puzzle;
    private CrossSelectGridMgr mgr_grid;
    public int grid_id;

    //클릭
    private GameObject panel_Select;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults;

    //이거 작동 중인가요?
    private bool isAct = false;
    private bool isMoving = false;  //이거 돌아갔나?
    private int piece_result = -2; //null = -2

    public SelectPiece selectedPiece = null;    //현재

    private SpriteRenderer wordimg;
    private GameObject rotObj; //회전하는 오브젝트
    private string wordmean;

    [SerializeField]
    private Material[] resMaterals; //결과 마테리얼
    [SerializeField]
    private MeshRenderer resultObj;

    private bool isRotate = false;  //회전 중인가?
    private float x = 0;
    private float targetX;
    private void Awake()
    {
        mgr_puzzle = GetComponentInParent<CrossSelectPuzzleManager>();
        mgr_grid = GetComponentInParent<CrossSelectGridMgr>();
        wordimg =  transform.Find("wordimg").GetComponent<SpriteRenderer>();    //transform.GetChild(0).GetComponent<SpriteRenderer>();
        rotObj = transform.Find("rotObj").gameObject;
        //resultObj = transform.Find("result").GetComponent<MeshRenderer>();

        panel_Select = mgr_grid.transform.parent.Find("Puzzle_Canvas").gameObject;
        panel_Select.GetComponent<Canvas>().scaleFactor = 0;

        graphicRaycaster = panel_Select.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(null);
        raycastResults = new List<RaycastResult>();
    }
    public void SelectPiece()
    {
        if(isAct == true || isRotate == true) { return; }
        isAct = true;
        mgr_puzzle.isAct = isAct;
        panel_Select.GetComponent<Canvas>().scaleFactor = 1;
    }
    public void SetResult(int i)
    {
        if(isMoving == false) { return; }
        piece_result = i;

        switch(i)
        {
            case -1:
                resultObj.material = resMaterals[0];
                break;
            case 0:
                resultObj.material = resMaterals[1];
                break;
            case 1:
                resultObj.material = resMaterals[2];
                break;
            default:
                break;
        }
        isRotate = false;
    }
    public void CompleteWord()
    {
        resultObj.material = resMaterals[3];
    }

    //그리드 회전
    public void RotateGrid()
    {
        if(isRotate == true) { return; }
        isRotate = true;

        StartCoroutine(Rotate());
    }
    private IEnumerator Rotate()
    {
        targetX = x + 180;
        while(true)
        {
            x += Time.deltaTime * 100f;
            rotObj.transform.rotation = Quaternion.Euler(new Vector2(x, 0));

            if(x >= targetX)
            {
                x = targetX;
                rotObj.transform.rotation = Quaternion.Euler(new Vector2(x, 0));
                isMoving = true;
                SendWordData();
                yield break;
            }
            yield return null;
        }
    }
    private void SendWordData()
    {
        isRotate = false;
        mgr_grid.AddWordPiece(grid_id, wordmean);
        wordimg.sprite = selectedPiece.SetWordPieceImg();
    }

    private void Update()
    {
        if(isAct == false || isRotate == true) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            raycastResults.Clear();

            pointerEventData.position = Input.mousePosition;
            graphicRaycaster.Raycast(pointerEventData, raycastResults);

            if (raycastResults.Count > 0)
            {
                foreach(var result in raycastResults)
                {
                    if (result.gameObject.CompareTag("WordData"))
                    {
                        wordmean = "";
                        wordimg.sprite = null;

                        bool isSelected = result.gameObject.GetComponent<SelectPiece>().IsSelected;
                        if(isSelected == true) { return; }
                        if(selectedPiece != null) { selectedPiece.IsSelected = false; }

                        //세팅
                        selectedPiece = result.gameObject.GetComponent<SelectPiece>();
                        wordmean = selectedPiece.SetWordPieceMean();

                        //그리드 회전
                        RotateGrid();

                        panel_Select.GetComponent<Canvas>().scaleFactor = 0;             
                        //Debug.Log(result.gameObject.name);
                        isAct = false;
                        mgr_puzzle.isAct = isAct;
                        break;
                    }
                }
            
            }
        }
    }

    //-------------------------------------------Reset
    public void ResetGrid()
    {
        if (selectedPiece == null) { return; }
        wordmean = "";
        wordimg.sprite = null;
        resultObj.material = resMaterals[4];    //default
    }
    //public void SetPos()
    //{
    //    float distance = Camera.main.WorldToScreenPoint(transform.position).z;
    //    Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
    //    Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

    //    transform.position = objPos;
    //}
    //public void SetPos(float xPos, float zPos)
    //{
    //    transform.position = new Vector3(xPos, transform.position.y, zPos);
    //}
    //public void FindGrid()
    //{
    //    Collider[] col = Physics.OverlapSphere(transform.position, 0.8f);
    //    foreach (Collider c in col)
    //    {
    //        if (c.gameObject == this.gameObject) { continue; }
    //        if (c.CompareTag("PuzzleGrid"))
    //        {
    //            grid = c.gameObject;

    //            CrossPuzzleGrid pGrid = grid.GetComponent<CrossPuzzleGrid>();
    //            if (pGrid == null) { continue; }

    //            bool isAdd = pGrid.SetWordPiece(piece, this.gameObject);
    //            if (isAdd == false)
    //            {
    //                ResetPuzzle();
    //                break;
    //            }
    //            //위치 설정
    //            curParent = c.transform;
    //            transform.SetParent(curParent);
    //            transform.localPosition = Vector3.zero;
    //            break;
    //            //SetPos(c.transform.position.x, c.transform.position.z);
    //        }
    //    }
    //}
    //public void ResetGrid()
    //{
    //    if (grid == null) { return; }
    //    grid.SendMessage("ResetGrid");
    //    grid = null;

    //    transform.SetParent(baseParent);
    //}
    //public void ResetPuzzle()
    //{
    //    SetPos(basePosition.x, basePosition.z);
    //    grid = null;
    //    transform.SetParent(baseParent);
    //}

}
