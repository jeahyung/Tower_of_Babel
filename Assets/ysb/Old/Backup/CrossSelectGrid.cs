using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CrossSelectGrid : MonoBehaviour
{
    private CrossSelectPuzzleManager manager_Puzzle;
    private CrossSelectGridMgr manager_Grid;
    public int grid_id;


    private bool isAct = false;
    private bool isMoving = false;  
    private int piece_result = -2;  //null = -2

    public SelectPiece selectedPiece = null;

    private SpriteRenderer wordimg;
    private GameObject rotObj;
    private string wordmean;

    [SerializeField]
    private Material[] resMaterals; 
    private MeshRenderer resultObj;

    private bool isRotate = false;
    private float x = 0;
    private float targetX;
    [SerializeField]
    private float rotateSpeed = 150f;


    //�ܰ�
    //[SerializeField]
    //private Material outline;
    //private Renderer renderer;
    //private List<Material> materials = new List<Material>();

    //�׽�Ʈ
    private PieceManager manager_Piece;

    private void Awake()
    {
        manager_Puzzle = GetComponentInParent<CrossSelectPuzzleManager>();
        manager_Grid = GetComponentInParent<CrossSelectGridMgr>();

        wordimg = transform.Find("wordimg").GetComponent<SpriteRenderer>();
        rotObj = transform.Find("rotObj").gameObject;
        resultObj = rotObj.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>();

        //outline = new Material(Shader.Find("Unlit/GridOutline"));

        manager_Piece = FindObjectOfType<PieceManager>();
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

        wordmean = mean;
        img = sprite;

        RotateGrid();
        NoneSelectGrid();   //���̴�

        isAct = false;
        manager_Puzzle.isAct = isAct;
    }

    public void ClosePanel()
    {
        isAct = false;
        NoneSelectGrid();
        manager_Piece.CloseSelectPanel();
    }

    //====================================test
    public void SelectPiece()
    {
        if (isAct == true || isRotate == true) { return; }
        isAct = true;
        manager_Puzzle.isAct = isAct;
        
        if(manager_Piece == null)
        {
            manager_Piece = FindObjectOfType<PieceManager>();
        }
        manager_Piece.OpenSelectPanel(this);

        SelectGrid();
    }
    public void SetResult(int i)
    {
        if (isMoving == false || selectedPiece == null) { return; }
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
        if(selectedPiece == null) { return; }
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
        manager_Grid.AddWordPiece(grid_id, wordmean);

        wordimg.sprite = img;
    }
    public void ResetGrid()
    {
        if (selectedPiece == null) { return; }
        wordmean = "";
        wordimg.sprite = null;
        resultObj.material = resMaterals[4];    //default
        selectedPiece = null;
    }

    //==================================================���̴�
    private void SelectGrid()
    {
        //renderer = rotObj.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();

        //materials.Clear();
        //materials.AddRange(renderer.sharedMaterials);
        //materials.Add(outline);

        //renderer.materials = materials.ToArray();
    }

    private void NoneSelectGrid()
    {
        //renderer = rotObj.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();

        //materials.Clear();
        //materials.AddRange(renderer.sharedMaterials);
        //materials.Remove(outline);

        //renderer.materials = materials.ToArray();
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
    //                    NoneSelectGrid();   //���̴�

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

}