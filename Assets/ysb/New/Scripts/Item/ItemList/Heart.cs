using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private int amount = 10;
    public Map map;


    [Header("Ω√¿€¡°")] 
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
        if (other.CompareTag("Player"))
        {
            other.GetComponent<EnergySystem>().SetEnergy(amount);

            ScoreManager.instance.Score_ItemGet();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Player_Itemget);
            this.gameObject.SetActive(false);
        }
    }
}
