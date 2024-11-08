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

    //public Map map;

    public Rook rook;   //ÇØÃ¼¸¦ À§ÇÑ
    public Mob mob; //로프
    [SerializeField] private float dropSpeed = 0.5f;

    private ParticleSystem sfx;
    private Color sfxBaseColor = new Color(1f, 0.9f, 0.45f, 1f);
    private Color sfxMobColor = new Color(0.4f, 0.99f, 0.99f, 1f);
    private Color sfxRedColor = Color.red;
    private void Start()
    {
        //map = FindObjectOfType<Map>();
        HideArea();
        if(tileType == TileType.none)
        {
            gameObject.SetActive(false);
        }
        sfx = effectPrefab.GetComponent<ParticleSystem>();
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
        ParticleSystem.MainModule main = sfx.main;
        main.startColor = sfxBaseColor;
        ShowEffect();
        //rend.enabled = false;

    }
    public void HideArea()
    {
        //rend.enabled = false;
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

    public void InitTile()
    {
        tileType = TileType.possible;
        mob = null;
        rook = null;
    }

    public void ShowMobArea()
    {
        ParticleSystem.MainModule main = sfx.main;
        main.startColor = sfxMobColor;
        ShowEffect();
        //rend.enabled = false;
    }
    
    public void ShowMobRedArea()
    {
        ParticleSystem.MainModule main = sfx.main;
        main.startColor = sfxRedColor;
        ShowEffect();
        //rend.enabled = false;
    }
}
