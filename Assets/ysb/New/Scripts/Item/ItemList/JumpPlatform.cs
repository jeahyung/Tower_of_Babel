using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    Map map;
    [Header("������")] //�̰� ���� ������ �޾ƿ��� �������� ����
    [SerializeField] private int startX;
    [SerializeField] private int startY;
    private void Start()
    {
        map = FindObjectOfType<Map>();
        Tile curTile = map.GetTile(map.tiles[startX, startY].coord);

        Vector3 pos = new Vector3(curTile.GetPosition().x,
            curTile.GetPosition().y + 2, curTile.GetPosition().z);
        transform.position = pos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) { map.PickUpJump(); }
    }
}
