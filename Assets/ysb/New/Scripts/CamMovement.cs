using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    
    Transform player;
    Vector3 targetPos;

    [SerializeField] private float speed = 0.2f;
    public Vector3Int offset;

    public Vector3Int minRange;
    public Vector3Int maxRange;

    private bool canMove = true;
    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public void SetMove(bool m)
    {
        canMove = m;
    }

    private void Update()
    {
        if(canMove == false) { return; }
        targetPos = player.position + offset;
        if(targetPos.y < 6) { targetPos.y = 6; }

        if (targetPos.z < minRange.z) { targetPos.z = minRange.z; }
        else if(targetPos.z > maxRange.z) { targetPos.z = maxRange.z; }

        if(targetPos.x < minRange.x) { targetPos.x = minRange.x; }
        else if(targetPos.x > minRange.y) { targetPos.x = minRange.y; }

        transform.position = targetPos;//Vector3.Lerp(transform.position, targetPos, speed);
    }
}
