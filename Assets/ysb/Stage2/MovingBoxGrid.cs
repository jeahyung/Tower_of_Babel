using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoxGrid : MonoBehaviour
{
    public MovingBoxManager manager_Box;
    public Vector3 direction;

    private void Awake()
    {
        if(manager_Box == null) { manager_Box = FindObjectOfType<MovingBoxManager>(); }
    }

    public void SetWord(WordData data)
    {
        if(data != null)
        {
            manager_Box.SetBoxData(data, direction);
        }
    }
}
