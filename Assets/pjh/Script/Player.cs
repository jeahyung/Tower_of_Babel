using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
   

    [SerializeField] private GameObject cameraArm;

  
    private Rigidbody rb;
    private Vector3 moveDirection;
    // 이동 속도
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotateSpeed;

   

    private bool isMove;
   

    // 달리기 활성화 여부
    private bool isSprinting;
    // 앉기 활성화 여부

   

   

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    
    }



    private void Move() // 카메라가 바라보는 방향을 활용한 움직임
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // 입력 감지
        isMove = moveInput.magnitude != 0; // 움직이고 있는지 확인
        if (isMove)
        {
           
            // 카메라가 앞을 보고 있을때, 오른쪽을 보고 있을때를 평면화해서 저장, 이후 입력값으로 방향 결정
            Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);

            // 왼쪽 쉬프트를 누른채로 있으면 2배 이동속도가 증가함
            // 떄면 다시 원래 속도로 돌아감.
            if (Input.GetKey(KeyCode.LeftShift) && !isSprinting)
            {
               
                isSprinting = true;
                speed = speed * 2f;
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && isSprinting)
            {
                
                isSprinting = false;
                speed = speed / 2f;
            }
            // 구한 방향을 토대로 이동
            //transform.position += moveDir * Time.deltaTime * speed;        
            moveDirection = moveDir * speed;
        }
        else
        {
            
            moveDirection = Vector3.zero;
        }
    }
    private void FixedUpdate()
    {
        // Rigidbody의 속도를 변경하여 이동
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    
}