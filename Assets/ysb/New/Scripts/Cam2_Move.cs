using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam2_Move : MonoBehaviour
{
    //�߰� - ����� 1��ķ
    //public PlayerMovement pm;
    Vector3 targetPos;

    public Vector2Int minRange;
    public Vector2Int maxRange;

    public bool isMove = false;
    [SerializeField]private float speed = 15f;

    private void Update()
    {
        if(isMove == false) { return; }

        float xPos = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        float zPos = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
        
        targetPos = transform.position + new Vector3(xPos, 0, zPos);
        if (targetPos.z < minRange.y) { targetPos.z = minRange.y; }
        else if (targetPos.z > maxRange.y) { targetPos.z = maxRange.y; }

        if (targetPos.x < minRange.x) { targetPos.x = minRange.x; }
        else if (targetPos.x > maxRange.x) { targetPos.x = maxRange.x; }

        transform.position = targetPos;//Vector3.Lerp(transform.position, targetPos, speed);
    }
}
