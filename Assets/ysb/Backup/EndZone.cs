using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    //[SerializeField]
    //private CrossPuzzleManager mgr;

    private CrossSelectPuzzleManager mgr;
    private void Awake()
    {
        mgr = GetComponentInParent<CrossSelectPuzzleManager>();  //GetComponentInParent<CrossPuzzleManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mgr.CompletePuzzle();
        }
    }
}