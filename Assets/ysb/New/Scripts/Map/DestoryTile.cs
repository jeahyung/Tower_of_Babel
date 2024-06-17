using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryTile : MonoBehaviour
{
    private CamMovement cam;
    private Map map;

    [SerializeField] private GameObject effect;
    [SerializeField] private int tileCount = 5; //∂≥±º ≈∏¿œ
    private void Awake()
    {
        cam = FindObjectOfType<CamMovement>();
        map = GetComponent<Map>();

        effect.SetActive(false);
    }

    public void DropTile()
    {
        cam.SetMove(false);
        effect.SetActive(true);
        StartCoroutine(Drop());
    }
    private IEnumerator Drop()
    {
        int i = 0;
        while(i < tileCount)
        {
            Tile tile = SelectTile();
            if(tile != null) { tile.DropTile(); }
            i++;

            yield return new WaitForSeconds(0.5f);
        }
        map.DropAllTile();
        StageManager.instance.ShowResult(); //∞·∞˙√¢
    }

    public Tile SelectTile()
    {
        int y_rand = Random.Range(0, map.LineCount);
        int x_rand = Random.Range(1, map.LineCount + 1);
        Vector2Int coord = new Vector2Int(x_rand, y_rand);

        return map.GetTile_NonePlayerTile(coord);
    }
}
