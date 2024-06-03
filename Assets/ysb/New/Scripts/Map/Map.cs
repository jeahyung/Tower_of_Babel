using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    private PlayerMovement player;
    private TurnManager manager_Turn;
    private SAManager manager_Action;

    public int LineCount;
    public Transform[] tileLines;

    public Tile[,] tiles;
    public List<Tile> moveArea = new List<Tile>();

    private Tile clickTile = null;

    private Tile previousTile = null;   //�˹��� ����
    public Tile nowTile;

    public Vector2Int startCoord;   //������
    public Vector2Int[] distance;

    public bool useAction = false;
    public bool useItem = false;
    private ItemManager manager_Item;

    public bool canControl = true;

    public Tile playerTile = null;
    //public Tile endtile;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        manager_Turn = player.GetComponent<TurnManager>();
        manager_Item = FindObjectOfType<ItemManager>();
        manager_Action = FindObjectOfType<SAManager>();


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
        //endtile = tiles[LineCount + 1, LineCount - 1];
        //endtile.HideArea();
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

    private void Start()
    {
        ResetTile();
    }

    private void Update()
    {
        if(StageManager.instance.isPlaying == false) { return; }    //���� ���� ����

        if(manager_Turn.IsMyTurn != true || manager_Turn.isDone != true)
        {
            canControl = false;
        }
        else
        {
            canControl = true;
        }
        if(canControl == false) { return; }

        //UI�� ���� ���� Ŭ�� �� �ϵ���
        if (EventSystem.current.IsPointerOverGameObject() == true) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            clickTile = null;
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
            else
            {
                if(useItem == true)
                {
                    CancelItem();
                }
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

    public Tile GetTile_NonePlayerTile(Vector2Int coord)
    {
        if (coord.x < 0 || coord.x >= LineCount + 2 || coord.y < 0 || coord.y >= LineCount) { return null; } //���� ��
        if (tiles[coord.x, coord.y].gameObject.activeSelf == false) { return null; }

        if (tiles[coord.x, coord.y] == nowTile)
        {
            for(int i = 0; i < 4; ++i)
            {
                Tile tile = GetTile_NonePlayerTile(nowTile.coord + distance[i]);
                if(tile != null) { return tile; }
            }
        }
        return tiles[coord.x, coord.y];
    }

    //�̰� ������?
    public bool IsLastTile()
    {
        if(nowTile == tiles[LineCount + 1, LineCount - 1]) { return true; }
        return false;
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
            if (moveArea.Contains(clickTile) == false) { return; }
            manager_Action.ActDone();
            HideArea();
            MovePlayerPosition_Continue();
            manager_Action.SetActionBtn(false);
            manager_Action.CheckActionCount();
            previousTile = nowTile; //���� Ÿ�� ����
            useAction = false;
            return;

        }
        if(moveArea.Contains(clickTile) == true)
        {
            if(useItem == true) { useItem = false; }
            HideArea();
            MovePlayerPosition();
            manager_Action.SetActionBtn(false);
            manager_Action.CheckActionCount();
            previousTile = nowTile; //���� Ÿ�� ����
        }
    }

    private int CalculateJumpCount()
    {
        int xCount = Mathf.Abs(clickTile.coord.y - nowTile.coord.y);
        int yCount = Mathf.Abs(clickTile.coord.x - nowTile.coord.x);

        int count = xCount > 0 ? xCount : yCount;
        return count;
    }


    //�÷��̾� �̵�

    public int HowRotate(Tile t)    //�󸶳� ȸ���ؾ� �ϴ°�?
    {
        int nextX = t.coord.y;
        int nextZ = t.coord.x;

        int curX = nowTile.coord.y;
        int curZ = nowTile.coord.x;

        int difX = nextX - curX;
        int difZ = nextZ - curZ;

        //left
        if(difX < 0)    //�ǵ��ƿ��� �� ����ؼ�
        {
            if(difZ > 0)            //45
            {
                return 2;
            }
            else if(difZ == 0)            //90
            {
                return 3;
            }
            else            //135
            {
                return 4;
            }
        }
        else if(difX > 0)//right
        {
            if (difZ > 0)            //45
            {
                return 7;
            }
            else if (difZ == 0)            //90
            {
                return 6;
            }
            else            //135
            {
                return 5;
            }
        }
        else//���� or �ĸ�
        {
            if(difZ > 0) { return 1; }
            else { return 8; }
        }
    }

    public void MovePlayerPosition()
    {
        player.SetPosition(clickTile.GetPosition(), HowRotate(clickTile));
        nowTile = clickTile;    //���� Ÿ�� ����
        playerTile = nowTile;
        //������ ��� -> �÷��̾��ʿ���
        Debug.Log("player move");
    }

    public void MovePlayerPosition_Continue()
    {
        player.SetPosition_Continue(CalculateJumpCount(), clickTile.GetPosition(), HowRotate(clickTile));
        nowTile = clickTile;    //���� Ÿ�� ����
        playerTile = nowTile;

        Debug.Log("player move_continue");
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

    public void UseItem_Key()
    {
        useItem = true;
        HideArea();

        moveArea.Clear();
        moveArea.AddRange(manager_Turn.ShowMobTile());
        ShowArea(moveArea);
    }

    public Rook UseKey()
    {
        return clickTile.rook;
    }

    //������ ����� ��ҵ�����
    public void CancelItem()
    {
        useItem = false;
        HideArea();
        FindTileInRange_Four(nowTile, player.moveRange);
    }


    public void SetObjectPosition(GameObject obj)
    {
        if (player.UseEnergy() == false)    //������ ���  
        {
            return;
        }

        //GameObject newObejct = Instantiate(obj);
        Vector3 pos = clickTile.GetPosition();
        float yPos = obj.transform.localScale.y / 2 + clickTile.transform.localScale.y;
        obj.transform.position = new Vector3(pos.x, yPos, pos.z);

        playerTile = clickTile;

        //clickTile.ChangeTileState(TileType.impossible);
        //player.UseEnergy(); //������ ���

        manager_Turn.EndPlayerTurn();
    }

    //�÷��̾� �����̵�
    public void SetPlayerPosition()
    {
        HideArea();
        player.TeleportPlayer(tiles[0, 0].GetPosition());
        nowTile = tiles[0, 0];
        playerTile = nowTile;
        //������ ��� -> �÷��̾��ʿ���
        useItem = false;
    }

    //���� ����
    public void ChangePlayerTile(Tile t)
    {
        playerTile = t;
    }
    #endregion

    public void CancelAct()
    {
        HideArea();
        FindTileInRange_Four(nowTile, player.moveRange);
        manager_Action.ActCancel();
        useAction = false;
    }

    public void BonusActOfKing()
    {
        manager_Action.UseAction_Bonus();
    }


    //�ǰ�
    public void TakeDamage(Tile mTile)
    {
        if(nowTile == mTile)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Player_Hit);
            player.TakeDamage();        
        }
    }
    public Tile CheckNearTile(Tile tile = null)
    {
        if(tile == null) { tile = nowTile; }

        //Ž�� ����(�Ʒ�-��-��-��)
        for (int i = 3; i > 0; --i)
        {
            Vector2Int nowCoord = tile.coord + distance[i];
            Tile findTile = GetTile(nowCoord);

            if (findTile != null)
            {
                nowTile = findTile;
                return findTile;
            }
        }
        //������ 2���� ������ Ž��
        for (int i = 3; i > 0; --i)
        {
            Vector2Int nowCoord = tile.coord + distance[i] * 2;
            Tile findTile = GetTile(nowCoord);

            if (findTile != null)
            {
                nowTile = findTile;
                return findTile;
            }
        }
        if (previousTile != null && GetTile(previousTile.coord) != null)
        {
            nowTile = previousTile;
            return previousTile;    //���� Ÿ�Ϸ� �� �� ������ ���� Ÿ�Ϸ� ����
        }
        return nowTile;
    }


    //Ÿ�� ���� �ʱ�ȭ
    public void ResetTile()
    {
        canControl = true;
        useItem = false;
        useAction = false;

        clickTile = null;
        previousTile = null;
        moveArea.Clear();

        for (int i = 1; i < LineCount + 1; ++i)
        {
            for(int j = 0; j < LineCount; ++j)
            {
                tiles[i, j].tileType = TileType.possible;
            }
        }

        GameObject startPoint = tiles[0, 0].gameObject;
        startPoint.SetActive(false);
        startPoint.SetActive(true);

        GameObject endPoint = tiles[LineCount + 1, LineCount - 1].gameObject;
        endPoint.SetActive(false);
        endPoint.SetActive(true);

        for (int i = 0; i < LineCount + 2; ++i)  //��ŸƮ/���� ���� ���
        {
            for (int j = 0; j < LineCount; ++j)
            {
                tiles[i, j].HideArea();
            }
        }

        nowTile = tiles[0, 0];
    }


    public void DropAllTile()
    {
        for (int i = 0; i < LineCount + 2; ++i)  //��ŸƮ/���� ���� ���
        {
            for (int j = 0; j < LineCount; ++j)
            {
                tiles[i, j].DropTile();
            }
        }
    }
}
