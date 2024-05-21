using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    [SerializeField] private Transform player;

    private void Start()
    {
        if (player == null) { player = GameObject.FindGameObjectWithTag("Player").transform; }
        player.position = transform.position;
    }
}
