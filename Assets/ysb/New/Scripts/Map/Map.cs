using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private PlayerMovement player;
    private TurnManager manager_Turn;
    private SpecialActionManager manager_Action;

    public int LineCount;
    public Transform[] tileLines;

    public Tile[,] tiles;
    public List<Tile> moveArea = new List<Tile>();

    private Tile clickTile = null;
    public Tile nowTile;
    public Vector2Int startCoord;   //������
    public Vector2Int[] distance;

    public bool useAction = false;
    public bool useItem = false;
    private ItemManager manager_Item;

    public bool canControl = true;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        manager_Turn = player.GetComponent<TurnManager>();
        manager_Item = FindObjectOfType<ItemManager>();
        manager_Action = FindObjectOfType<SpecialActionManager>();

        tiles = new Tile[LineCount + 2, LineCount];
        for(int i = 0; i < LineCount + 2; ++i)  //��ŸƮ/���� ���� ����
        {
            Tile[] tempTile = tileLines[i].GetComponentsInChildren<Tile>();
            for(int j = 0; j < LineCount; ++j)
            {
                tiles[i, j] = tempTile[j];
                tiles[i, j].SetTileCoord(i, j);
            }
        }

        nowTile = tiles[0, 0];

        distance = new Vector2Int[8];   //����
        distance[0] = Vector2Int.up;
        distance[1] = Vector2Int.down;

        distance[2] = Vector2Int.right;
        distance[3] = Vector2Int.left;

        distance[4] = new Vector2Int(1, 1); //���
        distance[5] = new Vector2Int(-1, 1); //�Ͽ�

        distance[6] = new Vector2Int(1, -1); //����
        distance[7] = new Vector2Int(-1, -1); //����
    }

    private void Update()
    {
        if(manager_Turn.IsMyTurn != true || manager_Turn.isDone != true)
        {
            canControl = false;
        }
        else
        {
            canControl = true;
        }
        if(canControl == false) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000);
            foreach (var hit in hits)
            {
                if (hit.collider.GetComponent<Tile>() != null)
                {
                    clickTile = hit.collider.GetComponent<Tile>();
                    break;
                }
            }

            if (clickTile != null)
            {
                CheckClickTileInArea();
            }
        }
    }

    public void StartPlayerTurn(int range)
    {
        FindTileInRange_Four(nowTile, range);
    }

    #region Ÿ�� Ž��
    private void CheckTileObject()
    {
        foreach(var t in tiles)
        {
            if(t != null)
            {
                t.CheckObject();
            }
        }
    }
    //4����
    public void FindTileInRange_Four(Tile startTile, int range) //������ / ����
    {
        moveArea.Clear();
        startCoord = startTile.coord;

        CheckTileObject();

        for (int i = 0; i < 4; ++i)
        {
            for(int j = 0; j < range; ++j)
            {
                Vector2Int nowCoord = startCoord + distance[i] * (j + 1);
                Tile findTile = GetTile(nowCoord);

                if(findTile != null)
                {
                    moveArea.Add(findTile);
                }   
            }
        }
        ShowArea(moveArea);
    }
    
    //8����
    public void FindTileInRange_Eight(Tile startTile, int range) //������ / ����
    {
        moveArea.Clear();
        if(startTile == null) { startTile = nowTile; }
        startCoord = startTile.coord;

        CheckTileObject();

        for (int i = startCoord.x - range; i <= startCoord.x + range; ++i)
        {
            for(int j = startCoord.y - range; j <= startCoord.y + range; ++j)
            {
                Vector2Int nowCoord = new Vector2Int(i, j);
                Tile findTile = GetTile(nowCoord);

                if(findTile != null)
                {
                    moveArea.Add(findTile);
                }   
            }
        }
        moveArea.Remove(nowTile);
        ShowArea(moveArea);
    }

    //�밢��
    public void FindTileInRange_Cross()
    {
        moveArea.Clear();
        startCoord = nowTile.coord;

        CheckTileObject();

        //���������� �밢�� �������� �� Ž�� -> �� ���� Ÿ���� ������ Ž�� ���� ���� ��������
        int i = 0;
        while(i < 4)
        {
            int j = 0;
            while(j < LineCount)
            {
                if(j >= LineCount) { i++; }
                Vector2Int nowCoord = startCoord + distance[i + 4] * (j + 1);
                Tile findTile = GetTile(nowCoord);

                if (findTile != null && findTile.tileType == TileType.possible)
                {
                    moveArea.Add(findTile);
                    j++;
                }
                else
                {
                    break;
                }
            }
            i++;
        }
        ShowArea(moveArea);
    }
    //4���� ��
    public void FindTileInRange_FourLong()
    {
        moveArea.Clear();
        startCoord = nowTile.coord;

        CheckTileObject();

        //���������� 4�������� �� Ž�� -> �� ���� Ÿ���� ������ Ž�� ���� ���� ��������
        int i = 0;
        while (i < 4)
        {
            int j = 0;
            while (j < LineCount)
            {
                if (j >= LineCount) { i++; }
                Vector2Int nowCoord = startCoord + distance[i] * (j + 1);
                Tile findTile = GetTile(nowCoord);

                if (findTile != null && findTile.tileType == TileType.possible)
                {
                    moveArea.Add(findTile);
                    j++;
                }
                else
                {
                    break;
                }
            }
            i++;
        }
        ShowArea(moveArea);
    }
    #endregion

    public Tile GetTile(Vector2Int coord)
    {
        if(coord.x < 0 || coord.x >= LineCount + 2 || coord.y < 0 || coord.y >= LineCount ) { return null; } //���� ��
        if(tiles[coord.x, coord.y].gameObject.activeSelf == false || tiles[coord.x, coord.y].tileType != TileType.possible) { return null; }

        return tiles[coord.x, coord.y];
    }

    public void HideArea()
    {
        foreach (var t in moveArea)
        {
            t.HideArea();
        }
    }

    public void ShowArea(List<Tile> tileInArea)
    {
        foreach (var tile in tileInArea)
        {
            tile.ShowArea();
        }
    }

    //Ŭ���� Ÿ���� ���� ���� �ִ°�?
    public void CheckClickTileInArea()
    {
        if (useItem) //������ ��� ��
        {
            if (moveArea.Contains(clickTile) == false)
            {
                HideArea();
                FindTileInRange_Four(nowTile, player.moveRange);
                useItem = false;
                return;
            }
            HideArea();
            manager_Item.UseItem();
            //selectedItem.UseItem();
            useItem = false;
            return;
        }
        if(useAction)   //�׼� ��� 
        {
            //if (moveArea.Contains(clickTile) == false)
            //{
            //    HideArea();
            //    FindTileInRange_Four(nowTile, player.moveRange);
            //    manager_Action.ActDone();
            //    useAction = false;
            //    return;
            //}
            if (moveArea.Contains(clickTile) == false) { return; }
            manager_Action.ActDone();
            useAction = false;
        }
        if(moveArea.Contains(clickTile) == true)
        {
            if(useItem == true) { useItem = false; }
            HideArea();
            MovePlayerPosition();            
        }
    }

    public void SelectItem(Item item)
    {
        useItem = true;
        //selectedItem = item;
    }

    #region ������

    //������ ���
    public void UseItem_Four(int r)
    {
        useItem = true;
        HideArea();
        FindTileInRange_Four(nowTile, r);
    }
    
    public void UseItem_Eight(int r)
    {
        useItem = true;
        HideArea();
        FindTileInRange_Eight(nowTile, r);
    }


    public void SetObjectPosition(GameObject obj)
    {
        //GameObject newObejct = Instantiate(obj);
        Vector3 pos = clickTile.GetPosition();
        float yPos = obj.transform.localScale.y + clickTile.transform.localScale.y;
        obj.transform.position = new Vector3(pos.x, yPos, pos.z);

        clickTile.ChangeTileState(TileType.impossible);
        player.UseEnergy(); //������ ���

        manager_Turn.EndPlayerTurn();
    }

    //�÷��̾� �̵�
    public void MovePlayerPosition()
    {
        player.SetPosition(clickTile.GetPosition());
        nowTile = clickTile;
        //������ ��� -> �÷��̾��ʿ���
    }

    //�÷��̾� �����̵�
    public void SetPlayerPosition()
    {
        HideArea();
        player.TeleportPlayer(tiles[0, 0].GetPosition());
        nowTile = tiles[0, 0];
        //������ ��� -> �÷��̾��ʿ���
        useItem = false;
    }
    #endregion

    public void CancelAct()
    {
        HideArea();
        FindTileInRange_Four(nowTile, player.moveRange);
        manager_Action.ActDone();
        useAction = false;
    }
}