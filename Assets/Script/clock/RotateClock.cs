using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateClock : MonoBehaviour
{
    public float rotationSpeed = 3f;
    private float[] values = new float[] { 18.0f, 54.0f, 90.0f, 126.0f, 162.0f };
    private int i = 0;
  //  private int sideCnt;
    private int clockDegree;

    private float targetRotation = 0f;
    [SerializeField] private bool mover = true;
  
    void Start()
    {
        mover = true;
       // sideCnt = 5;
        clockDegree = 180 / 5;
    }

    


    public void Moving()
    {
        if (mover)
        {
            targetRotation = clockDegree * i;
            i++;
            StartCoroutine(RotateXAxis());
        }
        else
        {
            targetRotation = clockDegree * i;
            i--;
            StartCoroutine(ReRotateXAxis());
        }

    }

 
    //public void moveClock()
    //{

    //        float step = rotationSpeed * Time.deltaTime;
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(30, 0, 0), step);



    //}
    private IEnumerator RotateXAxis()
    {
   
        if (i == 5)
        {
            mover = false;
        }
    
        float rotationSpeed = 10.0f; // ���ϴ� ȸ�� �ӵ��� ����
      
        // ���� ȸ�� ����
        float currentRotation = transform.eulerAngles.x;

        while (Mathf.Abs(transform.eulerAngles.x - targetRotation) > 0.1)
        {
            // X �� ȸ���� ������ ��ȭ
            currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);

            // ȸ�� ����
            transform.rotation = Quaternion.Euler(currentRotation, 0, 0);
                
            yield return null;
        }
     
    }

    private IEnumerator ReRotateXAxis()
    {

   
        //float targetRotation = values[i] + 1f;
        if(i == 0)
        {
            mover = true;
        }
      

        float rotationSpeed = 10.0f; // ���ϴ� ȸ�� �ӵ��� ����

        // ���� ȸ�� ����
        float currentRotation = transform.eulerAngles.x;

        while (Mathf.Abs(currentRotation - targetRotation) > 0.1)
        {
            // X �� ȸ���� ������ ��ȭ
            currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);

            // ȸ���� �����մϴ�.
            transform.rotation = Quaternion.Euler(currentRotation, 0, 0);

            yield return null;
        }

    }
}
