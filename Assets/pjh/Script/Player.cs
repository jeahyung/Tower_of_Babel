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
    // �̵� �ӵ�
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotateSpeed;

   

    private bool isMove;
   

    // �޸��� Ȱ��ȭ ����
    private bool isSprinting;
    // �ɱ� Ȱ��ȭ ����

   

   

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    
    }



    private void Move() // ī�޶� �ٶ󺸴� ������ Ȱ���� ������
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // �Է� ����
        isMove = moveInput.magnitude != 0; // �����̰� �ִ��� Ȯ��
        if (isMove)
        {
           
            // ī�޶� ���� ���� ������, �������� ���� �������� ���ȭ�ؼ� ����, ���� �Է°����� ���� ����
            Vector3 lookForward = new Vector3(cameraArm.transform.forward.x, 0f, cameraArm.transform.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.transform.right.x, 0f, cameraArm.transform.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            transform.forward = Vector3.Lerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);

            // ���� ����Ʈ�� ����ä�� ������ 2�� �̵��ӵ��� ������
            // ���� �ٽ� ���� �ӵ��� ���ư�.
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
            // ���� ������ ���� �̵�
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
        // Rigidbody�� �ӵ��� �����Ͽ� �̵�
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    
}