using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemObject : MonoBehaviour
{
    public Map map;
    private ItemInventory inven;
    public Item item;
    //public Sprite img;

    [Header("������")] //�̰� ���� ������ �޾ƿ��� �������� ����
    [SerializeField] private int startX;
    [SerializeField] private int startY;
    private PlayerMovement player;


    private void Awake()
    {
        item = GetComponent<Item>();
        inven = FindObjectOfType<ItemInventory>();
        player = FindObjectOfType<PlayerMovement>();
    }
    private void Start()
    {
        map = FindObjectOfType<Map>();

        Tile curTile = map.GetTile(map.tiles[startX, startY].coord);

        Vector3 pos = new Vector3(curTile.GetPosition().x,
            curTile.GetPosition().y + 2.8f, curTile.GetPosition().z);
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bool canGet = inven.PickUpItem(item);
            if(canGet == true)
            {
                ScoreManager.instance.Score_ItemGet();
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Player_Itemget);
                player.ShowEffect(1);
                this.gameObject.SetActive(false);
                Invoke("Hide", 1f);
            }
        }
    }

    private void Hide()
    {
        player.HideEffect(1);
    }
}
