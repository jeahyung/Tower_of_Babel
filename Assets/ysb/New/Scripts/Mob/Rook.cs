using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : MonoBehaviour
{
    public Map map;
    public GameObject effect;
    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    public Tile curTile = null;
    private void Start()
    {
        map = FindObjectOfType<Map>();

        curTile = map.GetTile(map.tiles[startX, startY].coord);
        curTile.tileType = TileType.impossible;
        curTile.rook = this;

        Vector3 pos = new Vector3(curTile.GetPosition().x, 
            curTile.GetPosition().y + 3, curTile.GetPosition().z);
        transform.position = pos;
        HideEffect();
    }
    public void ResetMob()
    {
        curTile.tileType = TileType.impossible;
    }

    public Tile ShowRookTile()
    {
        return curTile;
    }
    public void OpenRook()
    {
        curTile.tileType = TileType.possible;
        ShowEffect();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Destroy);
        Invoke("HideObject",1f);
    }

    private void HideObject()
    {
        gameObject.SetActive(false);
    }

    private void ShowEffect()
    {
        if (effect != null)
        {
            effect.SetActive(true);
        }       
    }


    private void HideEffect()
    {
        if (effect != null)
        {
            effect.SetActive(false);
        }
    }
}
