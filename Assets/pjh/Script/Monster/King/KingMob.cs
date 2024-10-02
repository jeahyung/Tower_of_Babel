using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMob : MonoBehaviour
{
    private Map map;
    public Tile tile;
   
    public List<Tile> bTiles;

    void Start()
    {
        map = FindObjectOfType<Map>();
    }

    void Update()
    {
        
    }


    public void AvoidOverlap()
    {
        //map.moveArea

        for (int i = map.moveArea.Count - 1; i >= 0; i--) // 역순으로 순회하여 안전하게 제거
        {
            foreach (Tile tileA in bTiles)
            {
                if (tileA.coord == bTiles[i].coord) // 좌표 비교
                {
                    bTiles.RemoveAt(i); // b 리스트에서 타일 제거
                    break; // 중복 제거 후 더 이상 비교할 필요 없음
                }
            }
        }


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
