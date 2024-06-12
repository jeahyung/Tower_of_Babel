using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTile : MonoBehaviour
{
    // Start is called before the first frame update
    public Tile tile;  

    void Start()
    {
        tile.HideArea();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveTile()
    {
        tile.HideArea();
    }
}
