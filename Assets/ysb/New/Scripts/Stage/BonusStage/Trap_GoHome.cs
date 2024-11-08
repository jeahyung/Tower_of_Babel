using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_GoHome : MonoBehaviour
{
    Map map;
    private void Awake()
    {
        map = FindObjectOfType<Map>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            map.GoHome();
        }
    }
}
