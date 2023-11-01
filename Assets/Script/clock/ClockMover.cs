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
            // 마우스 포인터의 스크린 좌표를 월드 좌표로 변환
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 레이캐스트를 통해 클릭 검사
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                // 클릭한 오브젝트가 자기 자신일 때만 true를 반환
                rotate.Moving();
               
            }
        }
    }
}
