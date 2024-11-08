using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMob : MonoBehaviour
{
    private Map map;
   // private Tile tile;
    //public Tile[,] tiles;
    public List<Tile> bTiles;
    public int LineCount;   //�� ������ -> �÷��� ��ġ�� ������ ����(���� ����Ʈ�� �ٰ��� ����)
                            // public int endLine;
    public int burningCnt;

   
    public int columnCount; // Ÿ���� �� ���� (LineCount)

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
        // 2���� �迭�� 1���� ����Ʈ�� ��ȯ
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

        // n�� ��ü Ÿ�� �������� ū ���, ��ü Ÿ���� ��ȯ
        if (n > allTiles.Count)
        {
            Debug.LogWarning("Requested number of tiles exceeds available tiles. Returning all tiles.");       
        }

        // n���� ������ Ÿ���� ����
        int attempt = 0;  // ���� ��ġ: ���� ���� ������
        for (int k = 0; k < n; k++)
        {
            bool validTileFound = false;
            while (!validTileFound && attempt < 100)  // �õ� Ƚ�� ����
            {
                int randomIndex = Random.Range(0, allTiles.Count);

                // ������ Ÿ���� TileType.impossible�� �ƴϸ� ����
                if (allTiles[randomIndex].tileType != TileType.impossible && map.nowTile.coord != allTiles[randomIndex].coord)
                {
                    selectedTiles.Add(allTiles[randomIndex]);
                    allTiles.RemoveAt(randomIndex);  // �ߺ� ������ ���� ������ Ÿ���� ����
                    validTileFound = true;       
                }
                attempt++;
            }

            // ��ȿ�� Ÿ���� ã�� ������ �� ��� �α�
            if (!validTileFound)
            {
                Debug.LogWarning("Could not find a valid tile to select after multiple attempts.");
                break;  // �� �̻� ��ȿ�� Ÿ���� ������ �� ������ ����
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
                    //tile.TileBurning(tile); // �ڱ� �ڽ��� ���ڷ� �����ϸ� TileBurning ȣ��
                    //�߰� �����ʼ�
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
                //tile.TileBurnOff(selectedTiles); // �ڱ� �ڽ��� ���ڷ� �����ϸ� TileBurning ȣ��
                //�߰� ���� �ʼ�------------------------------------------------------------------------------------------------------
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
