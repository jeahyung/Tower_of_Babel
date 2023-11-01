    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerMove : MonoBehaviour
{
    Rigidbody rigid;
    MeshRenderer mesh;
    private bool canMove = true;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(canMove == false) { return; }
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        rigid.velocity = new Vector3(x * 5, rigid.velocity.y, z * 5);
    }
    public void StopMoving(bool b)
    {
        canMove = b;
        mesh.enabled = b;
    }
}
