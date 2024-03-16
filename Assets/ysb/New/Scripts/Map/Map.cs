using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private PlayerMovement player;
    private TurnManager manager_Turn;

    public int LineCount;
    public Transform[] tileLines;

    public Tile[,] tiles;
    public List<Tile> moveArea = new List<Tile>();

    private Tile clickTile = null;
    public Tile nowTile;
    public Vector2Int startCoord;   //������
    public Vector2Int[] distance;

    public bool useItem = false;
    private ItemManager manager_Item;

    public bool canControl = true;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        manager_Turn = player.GetComponent<TurnManager>();
        manager_Item = FindObjectOfType<ItemManager>();

        tiles = new Tile[LineCount + 2, LineCount];
        for(int i = 0; i < LineCount + 2; ++i)  //��ŸƮ/���� ���� ���
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

    //4����
    public void FindTileInRange_Four(Tile startTile, int range) //������ / ����
    {
        moveArea.Clear();
        startCoord = startTile.coord;

        for(int i = 0; i < 4; ++i)
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
    public void FindTileInRange_Eight(int range) //������ / ����
    {
        moveArea.Clear();
        startCoord = nowTile.coord;

        for(int i = startCoord.x - range; i <= startCoord.x + range; ++i)
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

    public Tile GetTile(Vector2Int coord)
    {
        if(coord.x < 0 || coord.x >= LineCount + 2 || coord.y < 0 || coord.y >= LineCount ) { return null; } //���� ��
        if(tiles[coord.x, coord.y].gameObject.activeSelf == false || tiles[coord.x, coord.y].tileType != TileType.possible) { return null; }

        return tiles[coord.x, coord.y];
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
                foreach (var t in moveArea)
                {
                    t.HideArea();
                }
                FindTileInRange_Four(nowTile, player.moveRange);
                useItem = false;
                return;
            }
            foreach (var t in moveArea)
            {
                t.HideArea();
            }
            manager_Item.UseItem();
            //selectedItem.UseItem();
            useItem = false;
            return;
        }
        if(moveArea.Contains(clickTile) == true)
        {
            if(useItem == true) { useItem = false; }
            foreach(var t in moveArea)
            {
                t.HideArea();
            }
            MovePlayerPosition();            
        }
    }

    public void SelectItem(Item item)
    {
        useItem = true;
        //selectedItem = item;
    }

    //������ ���
    public void UseItem_Four(int r)
    {
        useItem = true;
        foreach (var t in moveArea)
        {
            t.HideArea();
        }
        FindTileInRange_Four(nowTile, r);
    }
    
    public void UseItem_Eight(int r)
    {
        useItem = true;
        foreach (var t in moveArea)
        {
            t.HideArea();
        }
        FindTileInRange_Eight(r);
    }


    public void SetObjectPosition(GameObject obj)
    {
        //GameObject newObejct = Instantiate(obj);
        Vector3 pos = clickTile.GetPosition();
        float yPos = obj.transform.localScale.y + clickTile.transform.localScale.y;
        obj.transform.position = new Vector3(pos.x, yPos, pos.z);

        clickTile.ChangeTileState(TileType.impossible);

        manager_Turn.EndPlayerTurn();
    }

    //�÷��̾� �̵�
    public void MovePlayerPosition()
    {
        player.SetPosition(clickTile.GetPosition());
        nowTile = clickTile;
    }
}
