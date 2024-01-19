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

    private GameObject panel_Canvas;
    private GameObject panel_Select;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults;

    private bool isAct = false;
    private bool isMoving = false;  //
    private int piece_result = -2; //null = -2

    public SelectPiece selectedPiece = null;    //

    private SpriteRenderer wordimg;
    private GameObject rotObj; //
    private string wordmean;

    [SerializeField]
    private Material[] resMaterals; 
    private MeshRenderer resultObj;

    private bool isRotate = false;  //
    private float x = 0;
    private float targetX;
    [SerializeField]
    private float rotateSpeed = 150f;


    //¿Ü°û
    [SerializeField]
    private Material outline;
    private Renderer renderer;
    private List<Material> materials = new List<Material>();

    //Å×½ºÆ®
    private PieceManager manager_Piece;
    private Button btn;

    private void Awake()
    {
        mgr_puzzle = GetComponentInParent<CrossSelectPuzzleManager>();
        mgr_grid = GetComponentInParent<CrossSelectGridMgr>();
        wordimg = transform.Find("wordimg").GetComponent<SpriteRenderer>();    //transform.GetChild(0).GetComponent<SpriteRenderer>();
        rotObj = transform.Find("rotObj").gameObject;
        resultObj = rotObj.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>();

        panel_Canvas = mgr_puzzle.transform.Find("Puzzle_Canvas").gameObject;
        panel_Select = panel_Canvas.transform.Find("Panel_Select").gameObject;
        //panel_Canvas.GetComponent<Canvas>().scaleFactor = 0;
        panel_Select.transform.localScale = new Vector3(0, 1, 1);

        graphicRaycaster = panel_Canvas.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(null);
        raycastResults = new List<RaycastResult>();

        outline = new Material(Shader.Find("Unlit/GridOutline"));

        manager_Piece = panel_Select.GetComponent<PieceManager>();


        btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => mgr_puzzle.SelectGrid(this));
    }


    //====================================test
    private Sprite img;
    public void AddWord(Sprite sprite, string mean, SelectPiece piece)
    {
        wordmean = "";
        wordimg.sprite = null;

        if(piece.IsSelected == true) { return; }
        piece.IsSelected = true;
        if(selectedPiece != null) { selectedPiece.IsSelected = false; }
        selectedPiece = piece;

        //wordimg.sprite = sprite;
        wordmean = mean;
        img = sprite;

        RotateGrid();
        NoneSelectGrid();   //½¦ÀÌ´õ


        //panel_Select.transform.localScale = new Vector3(0, 1, 1);
        isAct = false;
        mgr_puzzle.isAct = isAct;
    }
    public void SelectPiece()
    {
        if (isAct == true || isRotate == true) { return; }
        isAct = true;
        mgr_puzzle.isAct = isAct;
        //panel_Canvas.GetComponent<Canvas>().scaleFactor = 1;
        //panel_Select.transform.localScale = new Vector3(1, 1, 1);

        manager_Piece.OpenSelectPanel(this);

        SelectGrid();
    }
    public void SetResult(int i)
    {
        if (isMoving == false) { return; }
        piece_result = i;

        switch (i)
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

    public void RotateGrid()
    {
        if (isRotate == true) { return; }
        isRotate = true;

        StartCoroutine(Rotate());
    }
    private IEnumerator Rotate()
    {
        targetX = x + 360;
        while (true)
        {
            x += Time.deltaTime * rotateSpeed;
            rotObj.transform.rotation = Quaternion.Euler(new Vector2(x, 0));

            if (x >= targetX)
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

        wordimg.sprite = img;
        //wordimg.sprite = selectedPiece.SetWordPieceImg();
    }

    //private void Update()
    //{
    //    if (isAct == false || isRotate == true) { return; }
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        raycastResults.Clear();

    //        pointerEventData.position = Input.mousePosition;
    //        graphicRaycaster.Raycast(pointerEventData, raycastResults);

    //        if (raycastResults.Count > 0)
    //        {
    //            foreach (var result in raycastResults)
    //            {
    //                if (result.gameObject.CompareTag("WordData"))
    //                {
    //                    wordmean = "";
    //                    wordimg.sprite = null;

    //                    bool isSelected = result.gameObject.GetComponent<SelectPiece>().IsSelected;
    //                    if (isSelected == true) { return; }
    //                    if (selectedPiece != null) { selectedPiece.IsSelected = false; }

    //                    selectedPiece = result.gameObject.GetComponent<SelectPiece>();
    //                    wordmean = selectedPiece.SetWordPieceMean();

    //                    RotateGrid();
    //                    NoneSelectGrid();   //½¦ÀÌ´õ

    //                    panel_Select.transform.localScale = new Vector3(0, 1, 1);
    //                    //panel_Canvas.GetComponent<Canvas>().scaleFactor = 0;
    //                    //Debug.Log(result.gameObject.name);
    //                    isAct = false;
    //                    mgr_puzzle.isAct = isAct;
    //                    break;
    //                }
    //            }

    //        }
    //    }
    //}

    //-------------------------------------------Reset
    public void ResetGrid()
    {
        if (selectedPiece == null) { return; }
        wordmean = "";
        wordimg.sprite = null;
        resultObj.material = resMaterals[4];    //default
    }

    //==================================================½¦ÀÌ´õ
    private void SelectGrid()
    {
        renderer = rotObj.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();

        materials.Clear();
        materials.AddRange(renderer.sharedMaterials);
        materials.Add(outline);

        renderer.materials = materials.ToArray();
    }

    private void NoneSelectGrid()
    {
        renderer = rotObj.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();

        materials.Clear();
        materials.AddRange(renderer.sharedMaterials);
        materials.Remove(outline);

        renderer.materials = materials.ToArray();
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
    //            //
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