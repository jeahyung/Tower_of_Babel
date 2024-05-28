using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    Transform player;
    Vector3 targetPos;

    [SerializeField] private float speed = 0.2f;
    public Vector3Int offset;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, speed);
    }
}
