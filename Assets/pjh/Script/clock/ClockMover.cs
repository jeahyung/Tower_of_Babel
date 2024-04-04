using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockMover : MonoBehaviour
{
    [SerializeField] private RotateClock rotate;

    // Start is called before the first frame update
    void Start()
    {
        //rotate = GetComponent<RotateClock>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 �������� ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ����ĳ��Ʈ�� ���� Ŭ�� �˻�
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // Ŭ���� ������Ʈ�� �ڱ� �ڽ��� ���� true�� ��ȯ
                rotate.Moving();
               
            }
        }
    }
}
