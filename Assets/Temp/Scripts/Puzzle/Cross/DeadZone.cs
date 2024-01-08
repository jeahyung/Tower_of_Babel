using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField]
    //private CrossPuzzleManager mgr;
    private CrossSelectPuzzleManager mgr;
    private Vector3 spawnPos;
    private void Awake()
    {
        //mgr = GetComponentInParent<CrossPuzzleManager>();
        mgr = GetComponentInParent<CrossSelectPuzzleManager>();
        spawnPos = transform.GetChild(0).position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            mgr.ResetPuzzle();
            collision.transform.position = spawnPos;
        }
    }
}
