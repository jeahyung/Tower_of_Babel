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

    public Tile clickTile = null;

    private Tile previousTile = null;   //넉백을 위한
    public Tile nowTile;

    public Vector2Int startCoord;   //시작점
    public Vector2Int[] distance;

    public bool useAction = false;
    public bool useItem = false;
    private ItemManager manager_Item;

    public bool canControl = true;

    public Tile TempTile = null;
    public Tile playerTile = null;
    public Tile temp = null;
    //public Tile endtile;
    private int mapCount = 0;
    public bool isKing = false;

    int backCount = 1;
    public Tile[] backTiles = new Tile[2];

    public bool isJump = false;
    private MobManager mob = null;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        manager_Turn = player.GetComponent<TurnManager>();
        manager_Item = FindObjectOfType<ItemManager>();
        manager_Action = FindObjectOfType<SAManager>();
        mob = FindObjectOfType<MobManager>();

        tiles = new Tile[LineCount + 2, LineCount];
        for(int i = 0; i < LineCount + 2; ++i)  //스타트/엔드 지점 고려
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

        distance = new Vector2Int[8];   //방향
        distance[0] = Vector2Int.up;
        distance[1] = Vector2Int.down;

        distance[2] = Vector2Int.right;
        distance[3] = Vector2Int.left;

        distance[4] = new Vector2Int(1, 1); //상우
        distance[5] = new Vector2Int(-1, 1); //하우

        distance[6] = new Vector2Int(1, -1); //상좌
        distance[7] = new Vector2Int(-1, -1); //하좌
    }

    private void Start()
    {
        ResetTile();
    }

    private void Update()
    {
        if(StageManager.instance.isPlaying == false) { return; }    //게임 시작 여부

        if(manager_Turn.IsMyTurn != true || manager_Turn.isDone != true)
        {
            canControl = false;
        }
        else
        {
            canControl = true;
        }
        if(canControl == false) { return; }

        //UI상에 있을 때는 클릭 못 하도록
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

    #region 타일 탐색
    #region 범위표시
    public void ShowMobRange(List<Tile> mTiles)
    {
        //HideArea();
        ShowMobArea(mTiles);
    }
    public void HideMobRange(List<Tile> mTiles)
    {
        foreach (var t in mTiles)
        {
            t.HideArea();
        }
    }
    public void AgainShowTile()
    {
        if(moveArea.Count == 0) { FindTileInRange_Four(nowTile, 1); }
        ShowArea(moveArea);
    }
    void ShowMobArea(List<Tile> tileInArea)
    {
        foreach (var tile in tileInArea)
        {
            //mapCount++;
            if(moveArea.Contains(tile) == true || tile == nowTile) { tile.ShowMobRedArea(); }
            else { tile.ShowMobArea(); }
        }
    }
    #endregion

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
    //4방향
    public void FindTileInRange_Four(Tile startTile, int range) //시작점 / 범위
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
    
    //8방향
    public void FindTileInRange_Eight(Tile startTile, int range) //시작점 / 범위
    {
        moveArea.Clear();
        if(startTile == null) { startTile = nowTile; }
        startCoord = startTile.coord;
        isKing = true;

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

    //대각선
    public void FindTileInRange_Cross()
    {
        moveArea.Clear();
        startCoord = nowTile.coord;

        CheckTileObject();

        //시작점에서 대각선 방향으로 쭉 탐색 -> 못 가는 타일이 있으면 탐색 종료 다음 방향으로
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
    //4방향 쭉
    public void FindTileInRange_FourLong()
    {        
        moveArea.Clear();
        startCoord = nowTile.coord;
        isKing = true;
        CheckTileObject();

        //시작점에서 4방향으로 쭉 탐색 -> 못 가는 타일이 있으면 탐색 종료 다음 방향으로
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

    public bool CanSpawn(Vector2Int coord)
    {
        if(tiles[coord.x, coord.y] == nowTile) { return false; }

        return true;
    }
    public Tile GetTileForSpawn(Vector2Int coord)
    {
        if (coord.x < 0 || coord.x >= LineCount + 2 || coord.y < 0 || coord.y >= LineCount) { return null; } //범위 밖
        if (tiles[coord.x, coord.y].gameObject.activeSelf == false) { return null; }
        return tiles[coord.x, coord.y];
    }
    public Tile GetTile(Vector2Int coord)
    {
        if(coord.x < 0 || coord.x >= LineCount + 2 || coord.y < 0 || coord.y >= LineCount ) { return null; } //범위 밖
        if(tiles[coord.x, coord.y].gameObject.activeSelf == false || tiles[coord.x, coord.y].tileType != TileType.possible) { return null; }

        return tiles[coord.x, coord.y];
    }

    public Tile GetTile_NonePlayerTile(Vector2Int coord)
    {
        if (coord.x < 0 || coord.x >= LineCount + 2 || coord.y < 0 || coord.y >= LineCount) { return null; } //범위 밖
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

    //이거 끝지점?
    public bool IsLastTile()
    {
        if(nowTile == tiles[LineCount + 1, LineCount - 1]) { return true; }
        return false;
    }

    public void HideArea()
    {
        foreach (var t in moveArea)
        {
            if(t == null) { continue; }
            t.HideArea();
        }
    }

    public void ShowArea(List<Tile> tileInArea)
    {
        foreach (var tile in tileInArea)
        {          
            if(tile != null)
            {
                mapCount++;
                tile.ShowArea();
            }
        }
    }

    //클릭한 타일이 범위 내에 있는가?
    public void CheckClickTileInArea()
    {
        if (useItem) //아이템 사용 시
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
            Debug.Log("iiiii");
            return;
        }
        if(useAction)   //액션 사용 
        {
            if (moveArea.Contains(clickTile) == false) { return; }
            manager_Action.ActDone();
            //HideArea();

            //if (manager_Action.usedKing == true) { MovePlayerPosition(); }
            //else { MovePlayerPosition_Continue(); }
            if(manager_Action.getActState == 0 || manager_Action.getActState == 1) { MovePlayerPosition_Continue(); }
            else { MovePlayerPosition(); }

            //manager_Action.SetActionBtn(false);
            //manager_Action.CheckActionCount();
            //previousTile = nowTile; //이전 타일 갱신
            useAction = false;
            //return;
            Debug.Log("aaaaaaa");

        }
        else
        {
            if (moveArea.Contains(clickTile) == false) { return; }
            
            MovePlayerPosition();
            Debug.Log("ddd^^^^^^^^^^^^^^^dd");
        }
        if(moveArea.Contains(clickTile) == true)
        {
            if(useItem == true) { useItem = false; }
            HideArea();

            //MovePlayerPosition();
            Debug.Log("ddd^^^^^^^^^^^^^^^");

            manager_Action.SetActionBtn(false);
            manager_Action.CheckActionCount();
            previousTile = nowTile; //이전 타일 갱신
            isJump = false;
        }
    }

    private int CalculateJumpCount()
    {
        int xCount = Mathf.Abs(clickTile.coord.y - nowTile.coord.y);
        int yCount = Mathf.Abs(clickTile.coord.x - nowTile.coord.x);

        int count = xCount > 0 ? xCount : yCount;
        return count;
    }


    //플레이어 이동

    public int HowRotate(Tile t)    //얼마나 회전해야 하는가?
    {
        int nextX = t.coord.y;
        int nextZ = t.coord.x;

        int curX = nowTile.coord.y;
        int curZ = nowTile.coord.x;

        int difX = nextX - curX;
        int difZ = nextZ - curZ;

        //left
        if(difX < 0)    //되돌아오는 거 고려해서
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
        else//정면 or 후면
        {
            if(difZ > 0) { return 1; }
            else { return 8; }
        }
    }

    public void MovePlayerPosition()
    {
        player.SetPosition(clickTile.GetPosition(), HowRotate(clickTile));
        SetBackTile();

        nowTile = clickTile;    //현재 타일 갱신
        playerTile = nowTile;

        //에너지 사용 -> 플레이어쪽에서
        Debug.Log("player move");
    }

    public void MovePlayerPosition_Continue()
    {
        player.SetPosition_Continue(CalculateJumpCount(), clickTile.GetPosition(), HowRotate(clickTile));
        SetBackTile();
        nowTile = clickTile;    //현재 타일 갱신
        playerTile = nowTile;
        Debug.Log("player move_continue");
    }

    public void SelectItem(Item item)
    {
        useItem = true;
        //selectedItem = item;
    }

    #region 아이템

    //아이템 사용
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
        moveArea.AddRange(mob.ShowMob_key1());
        //moveArea.AddRange(manager_Turn.ShowRookTile());
        ShowArea(moveArea);
    }
    public void UseItem_Key2()
    {
        useItem = true;
        HideArea();

        moveArea.Clear();
        moveArea.AddRange(mob.ShowMob_key2());
        //moveArea.AddRange(manager_Turn.ShowRookTile());
        ShowArea(moveArea);
    }

    public void UseItem_Rope()
    {
        useItem = true;
        HideArea();

        moveArea.Clear();
        moveArea.AddRange(manager_Turn.ShowMobTile());
        ShowArea(moveArea);
    }

    public void SelectItem_Clock()
    {
        useItem = true;
        HideArea();
        moveArea.Clear();
        moveArea.Add(backTiles[0]);
        moveArea.Add(backTiles[1]);
        ShowArea(moveArea);
    }

    public Rook UseKey()
    {
        return clickTile.rook;
    }

    public Mob UseRope()
    {
        return clickTile.mob;
    }

    //아이템 사용이 취소됐을때
    public void CancelItem()
    {
        useItem = false;
        HideArea();
        FindTileInRange_Four(nowTile, player.moveRange);
    }


    public void SetObjectPosition(GameObject obj)
    {
        if (player.UseEnergy() == false)    //에너지 사용  
        {
            return;
        }

        //GameObject newObejct = Instantiate(obj);
        Vector3 pos = clickTile.GetPosition();
        float yPos = obj.transform.localScale.y + 2 + clickTile.transform.position.y;
        obj.transform.position = new Vector3(pos.x, yPos, pos.z);

        temp = playerTile;
        playerTile = clickTile;
        TempTile = playerTile;
        //clickTile.ChangeTileState(TileType.impossible);
        //player.UseEnergy(); //에너지 사용

        manager_Turn.EndPlayerTurn();
    }

    //플레이어 순간이동
    public bool SetPlayerPosition(int i)
    {
        if(backTiles[i].tileType != TileType.possible)
        {
            useItem = false;
            HideArea();
            FindTileInRange_Four(nowTile, 1);
            return false;
        }
        HideArea();
        int x = backTiles[i].coord.x;
        int y = backTiles[i].coord.y;
        player.TeleportPlayer(tiles[x,y].GetPosition());

        Tile temp = backTiles[i];
        SetBackTile();
        nowTile = temp;
        playerTile = nowTile;
        //에너지 사용 -> 플레이어쪽에서
        useItem = false;
        return true;
    }
    public bool SetPlayerPosition2()
    {
        if(clickTile.tileType != TileType.possible)
        {
            useItem = false;
            HideArea();
            FindTileInRange_Four(nowTile, 1);
            return false;
        }
        HideArea();

        player.TeleportPlayer(clickTile.GetPosition());
        SetBackTile();
        nowTile = clickTile;
        playerTile = nowTile;
        useItem = false;
        return true;
    }

    //추적 변경
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


    //피격
    public void TakeDamage(Tile mTile)
    {
        if(nowTile == mTile)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Player_Hit);      
            player.TakeDamage();
            //playerTile = nowTile;
        }
    }
    public Tile CheckNearTile(Tile tile = null)
    {
        if(tile == null) { tile = nowTile; }

        //탐색 순서(아래-위-좌-우)
        for (int i = 3; i > 0; --i)
        {
            Vector2Int nowCoord = tile.coord + distance[i];
            Tile findTile = GetTile(nowCoord);

            if (findTile != null)
            {
                SetBackTile();
                nowTile = findTile;
                return findTile;
            }
        }
        //8방향으로 다시 탐색
        for (int i = 0; i < 8; ++i)
        {
            Vector2Int nowCoord = tile.coord + distance[i];
            Tile findTile = GetTile(nowCoord);

            if (findTile != null)
            {
                SetBackTile();
                nowTile = findTile;
                return findTile;
            }
        }
        //없으면 2범위 내에서 탐색
        for (int i = 3; i > 0; --i)
        {
            Vector2Int nowCoord = tile.coord + distance[i] * 2;
            Tile findTile = GetTile(nowCoord);

            if (findTile != null)
            {
                SetBackTile();
                nowTile = findTile;
                return findTile;
            }
        }
        if (previousTile != null && GetTile(previousTile.coord) != null)
        {
            SetBackTile();
            nowTile = previousTile;
            return previousTile;    //이전 타일로 갈 수 있으면 이전 타일로 ㄱㄱ
        }
        return nowTile;
    }


    //타일 상태 초기화
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

        for (int i = 0; i < LineCount + 2; ++i)  //스타트/엔드 지점 고려
        {
            for (int j = 0; j < LineCount; ++j)
            {
                tiles[i, j].HideArea();
            }
        }

        HideArea();
        nowTile = tiles[0, 0];
        backCount = 1;
    }


    public void DropAllTile()
    {
        
        for (int i = 0; i < LineCount + 2; ++i)  //스타트/엔드 지점 고려
        {
            for (int j = 0; j < LineCount; ++j)
            {
                tiles[i, j].DropTile();
            }
        }
       
    }

    public void SetPlayerTile()
    {
        playerTile = TempTile;
    }

    public void RestPlayerTile()
    {
        playerTile = temp;
        Debug.Log(playerTile.coord.x);
        Debug.Log(playerTile.coord.y);

    }

    public void SetBackTile()
    {
        if(backCount >= 1) { backCount--; return; }
        if(backTiles[0] == null)
        {
            backTiles[0] = nowTile;
            return;
        }
        backTiles[1] = backTiles[0];
        backTiles[0] = nowTile;

    }

    public void PickUpJump()
    {
        isJump = true;
    }
    public void FindJumpTile()
    {
        Tile curTile = nowTile;
        moveArea.Clear();
        for(int i = 0; i < 4; ++i)
        {
            Tile newTile = GetTile(curTile.coord + distance[i] * 2);
            moveArea.Add(newTile);
        }
        ShowArea(moveArea);
        isJump = false;
    }
    public void GoHome()
    {
        HideArea();
        player.TeleportPlayer(tiles[0, 0].GetPosition());

        Tile temp = tiles[0, 0];
        SetBackTile();
        nowTile = temp;
        playerTile = nowTile;
        //에너지 사용 -> 플레이어쪽에서
    }
}
