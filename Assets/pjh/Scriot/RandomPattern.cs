using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RandomPattern : MonoBehaviour
{
    
    public int[] arr = new int[8];
    private bool stop = false;

    void Start()
    {
        
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ChooseRandomNumber();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            ClearArrPattern();
        }
    }

    //���� ���� �ʱ�ȭ �ʿ�
    private void ChooseRandomNumber()
    {
        //�������� ��� ȣ��Ǿ� ���ư��� �Լ��� ȿ���� ������ �����ʿ��ѵ� 
        //�̰� ���� �� ��4 �� ȣ��Ǹ� �̿��� �߰�ȣ���� ����. ���� ����
        
        int randomIndex = UnityEngine.Random.Range(1, 9);
       
        CheckStopBool();

        if (arr[randomIndex-1] == 0)
        {
            arr[randomIndex-1] = randomIndex;
            Debug.Log(randomIndex);
        }
        else if (stop)
        {
            ChooseRandomNumber();
        }
        else
        {
            Debug.Log("��� ������ �� ���Դ�");
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

    public void ClearArrPattern()
    {
        Array.Clear(arr, 0, arr.Length);
    }
}
