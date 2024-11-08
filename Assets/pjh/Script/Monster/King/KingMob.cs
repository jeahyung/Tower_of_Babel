using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMob : MonoBehaviour
{
    private Map map;
   // private Tile tile;
    //public Tile[,] tiles;
    public List<Tile> bTiles;
    public int LineCount;   //맵 사이즈 -> 플레이 위치상 마지막 라인(엔드 포인트의 줄과는 별개)
                            // public int endLine;
    public int burningCnt;

   
    public int columnCount; // 타일의 열 개수 (LineCount)

    public List<Tile> allTiles;
    public List<Tile> selectedTiles;

    void Start()
    {
        map = FindObjectOfType<Map>();
        //tile = FindObjectOfType<Tile>();
        allTiles = new List<Tile>();
        selectedTiles = new List<Tile>();        
        
    }

    public void SelectRandomTiles(int n)
    {       
        // 2차원 배열을 1차원 리스트로 변환
        for (int i = 1; i < LineCount+1; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                if (map.tiles[i, j] != null)
                {
                    allTiles.Add(map.tiles[i, j]);
                }
            }
        }

        // n이 전체 타일 개수보다 큰 경우, 전체 타일을 반환
        if (n > allTiles.Count)
        {
            Debug.LogWarning("Requested number of tiles exceeds available tiles. Returning all tiles.");       
        }

        // n개의 랜덤한 타일을 선택
        int attempt = 0;  // 안전 장치: 무한 루프 방지용
        for (int k = 0; k < n; k++)
        {
            bool validTileFound = false;
            while (!validTileFound && attempt < 100)  // 시도 횟수 제한
            {
                int randomIndex = Random.Range(0, allTiles.Count);

                // 선택한 타일이 TileType.impossible이 아니면 선택
                if (allTiles[randomIndex].tileType != TileType.impossible && map.nowTile.coord != allTiles[randomIndex].coord)
                {
                    selectedTiles.Add(allTiles[randomIndex]);
                    allTiles.RemoveAt(randomIndex);  // 중복 방지를 위해 선택한 타일은 제거
                    validTileFound = true;       
                }
                attempt++;
            }

            // 유효한 타일을 찾지 못했을 때 경고 로그
            if (!validTileFound)
            {
                Debug.LogWarning("Could not find a valid tile to select after multiple attempts.");
                break;  // 더 이상 유효한 타일을 선택할 수 없으면 종료
            }
        }


    }

    public void KingAct()
    {
        SelectRandomTiles(burningCnt);

        if (AvoidOverlap())
        {
            foreach (Tile tile in selectedTiles)
            {
                if (tile != null)
                {
                    //------------------------------------------------------------------------------------------------------
                    //tile.TileBurning(tile); // 자기 자신을 인자로 전달하며 TileBurning 호출
                    //추가 수정필수
                                            //------------------------------------------------------------------------------------------------------
                }
            }
        }
        else
        {
            selectedTiles.Clear();
            KingAct();
        }
    }

    public void BurnOff()
    {        
        foreach (Tile tile in selectedTiles)
        {
            if (tile != null)
            {
                //------------------------------------------------------------------------------------------------------
                //tile.TileBurnOff(selectedTiles); // 자기 자신을 인자로 전달하며 TileBurning 호출
                //추가 수정 필수------------------------------------------------------------------------------------------------------
                break;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            KingAct();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            BurnOff();
        }
    }

    public bool AvoidOverlap()
    {
        foreach (Tile tile in selectedTiles)
        {
            if (tile == map.moveArea[0])
            {
                return false;
            }
        }

        return true;

    }




    //public void FindTileInRange_Four(Tile startTile, int range) //시작점 / 범위
    //{
    //    moveArea.Clear();
    //    startCoord = startTile.coord;

    //    CheckTileObject();

    //    for (int i = 0; i < 4; ++i)
    //    {
    //        for (int j = 0; j < range; ++j)
    //        {
    //            Vector2Int nowCoord = startCoord + distance[i] * (j + 1);
    //            Tile findTile = GetTile(nowCoord);

    //            if (findTile != null)
    //            {
    //                moveArea.Add(findTile);
    //            }
    //        }
    //    }
    //    ShowArea(moveArea);
    //}



}
