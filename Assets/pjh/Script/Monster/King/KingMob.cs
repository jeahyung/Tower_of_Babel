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

        for (int i = map.moveArea.Count - 1; i >= 0; i--) // �������� ��ȸ�Ͽ� �����ϰ� ����
        {
            foreach (Tile tileA in bTiles)
            {
                if (tileA.coord == bTiles[i].coord) // ��ǥ ��
                {
                    bTiles.RemoveAt(i); // b ����Ʈ���� Ÿ�� ����
                    break; // �ߺ� ���� �� �� �̻� ���� �ʿ� ����
                }
            }
        }


    }

    //public void FindTileInRange_Four(Tile startTile, int range) //������ / ����
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
