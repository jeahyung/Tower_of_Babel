using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RandomPattern : MonoBehaviour
{
    /*
     ChooseRandomNumber(); 호출 후 sum 사용으로 맵 구현
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

    //보스 이후 초기화 필요
    private void ChooseRandomNumber()
    {
        //쓸데없이 계속 호출되어 돌아가는 함수는 효율이 거지라 개선필요한데 
        //이건 패턴 중 총4 번 호출되며 이외의 추가호출은 없다. 개선 보류
        
        int randomIndex = UnityEngine.Random.Range(1, 9); // 1 ~8까지
       
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
            Debug.Log("모든 패턴이 다 나왔다");
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
