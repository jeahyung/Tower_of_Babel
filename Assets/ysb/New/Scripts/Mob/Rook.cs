using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : MonoBehaviour
{
    public Map map;

    [Header("���� ������")] //�̰� ���� ������ �޾ƿ��� �������� ����
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
    }

    public Tile ShowRookTile()
    {
        return curTile;
    }
    public void OpenRook()
    {
        curTile.tileType = TileType.possible;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Destroy);
        gameObject.SetActive(false);
    }
}
