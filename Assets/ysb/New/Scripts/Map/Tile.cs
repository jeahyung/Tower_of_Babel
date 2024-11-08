using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TileType { none = -1, possible = 0, impossible = 1, }
public class Tile : MonoBehaviour
{
    public TileType tileType;

    public Vector2Int coord;

    //public SpriteRenderer rend;

    public GameObject effectPrefab;
    public ParticleSystem flame;

    //public Map map;
    public int flameCnt = 1;
    public int flameHP = 0;
    public Rook rook;   //ÇØÃ¼¸¦ À§ÇÑ
    public Mob mob; //로프
    [SerializeField] private float dropSpeed = 0.5f;

  //  public List<Tile> burnedTile = new List<Tile>();

    private void Start()
    {
        //map = FindObjectOfType<Map>();
        HideArea();
        if(tileType == TileType.none)
        {
            gameObject.SetActive(false);
        }
        if(flame != null)
        {
            flame.Stop();

        }
    //    flameCnt =1;
    }

    public void SetTileCoord(int i, int j)
    {
        //coord = new Coord(i, j);
        coord = new Vector2Int(i, j);

        //x = coord.coord.x;
        //y = coord.coord.y;
    }

    public void ShowArea()
    {
        //if(tileType != TileType.possible) { return; }        
        ShowEffect();
      //  rend.enabled = false;

    }
    public void HideArea()
    {
       // rend.enabled = false;
        HideEffect();
    }

    public Vector3 GetPosition()
    {
        //if(canMove == false)
        //{
        //    return Vector3.zero;
        //}
        return new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public void ChangeTileState(TileType tp)
    {
        tileType = tp;
    }

    public void CheckObject()
    {
        if(tileType != TileType.possible)
        {
            RaycastHit[] hit = Physics.RaycastAll(transform.position, Vector3.up, 1 << LayerMask.NameToLayer("Object"));
            foreach(var h in hit)
            {
                if(h.collider != null)
                {
                    //Debug.Log(h.collider.name);
                    return;
                }
            }

            tileType = TileType.possible;
        }
    }
    private void ShowEffect()
    {
        if (effectPrefab != null)
        {
            effectPrefab.SetActive(true);
        }
    }


    private void HideEffect()
    {   
        if (effectPrefab != null)
        {
            effectPrefab.SetActive(false);
        }
        else if (effectPrefab == null)
        {
            effectPrefab = transform.GetChild(2).gameObject;
            effectPrefab.SetActive(false);
            Debug.Log("null effect");
        }
    }

    public void DropTile()
    {        
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Game_Over_Broken);
        GetComponent<Collider>().enabled = false;
        transform.DOMoveY(-10, dropSpeed);
    }

    public void TileBurning(Tile tile)
    {
        if(flame == null)
        {
            return;
        }
       
      //  burnedTile.Add(tile);

        tile.flameHP = flameCnt;
        tile.flame.Play();
    }

    public void TileBurnOff(List<Tile> tiles)
    {
        /*
        //if (flame == null)
        //{
        //    Debug.Log("TileBurnOff return...");

        //    return;
        //}
        //Debug.Log("TileBurnOff Doing...");

        //foreach (Tile tile in burnedTile)
        //{
        //    tile.flameHP--;
        //    Debug.Log("TileBurnOff Doing...");
        //    if (tile.flameHP <= 0)
        //    {
        //        flame.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        //    }
        //}


        //if (flameHP <= 0)
        //{
        //    flame.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        //}
        //else
        //    return;
        */

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].flameHP--;
            Debug.Log("TileBurnOff flameHP");
            if (tiles[i].flameHP <= 0)
            {
                tiles[i].flame.Stop();//true, ParticleSystemStopBehavior.StopEmitting
                Debug.Log("TileBurnOff");
                tiles.RemoveAt(0);
                i--;

            }
        }

    }

}
