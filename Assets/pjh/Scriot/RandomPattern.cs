using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RandomPattern : MonoBehaviour
{
    /*
     ChooseRandomNumber(); ȣ�� �� sum ������� �� ����
     */
    public int[] arr = new int[8];
    
    private bool stop = false;
    private int sum = 0;

    void Start()
    {
        
    }

    public int Number()
    {
        ChooseRandomNumber();
  
        return sum;
    }

    public void ClearArrPattern()
    {
        Array.Clear(arr, 0, arr.Length);
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    ChooseRandomNumber();
        //    Debug.Log("----");
        //    Debug.Log(sum);
        //}
         if (Input.GetKeyDown(KeyCode.Q))
        {
            Number();
            //    Debug.Log("----");
            //    Debug.Log(sum);
        }
    }

    //���� ���� �ʱ�ȭ �ʿ�
    private void ChooseRandomNumber()
    {
        //�������� ��� ȣ��Ǿ� ���ư��� �Լ��� ȿ���� ������ �����ʿ��ѵ� 
        //�̰� ���� �� ��4 �� ȣ��Ǹ� �̿��� �߰�ȣ���� ����. ���� ����
        
        int randomIndex = UnityEngine.Random.Range(1, 9); // 1 ~8����
       
        CheckStopBool();

        if (arr[randomIndex-1] == 0)
        {
            arr[randomIndex-1] = randomIndex;
            Debug.Log(randomIndex);
            sum = randomIndex;
        }
        else if (stop)
        {
            ChooseRandomNumber();
        }
        else
        {
            ClearArrPattern();
            Debug.Log("��� ������ �� ���Դ�");
            sum = -1;
        }

        
    }

    private void CheckStopBool()
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == 0)
            {
                stop = true;
                break;
            }
            else
            {
                stop = false;

            }
        }
    }

}
