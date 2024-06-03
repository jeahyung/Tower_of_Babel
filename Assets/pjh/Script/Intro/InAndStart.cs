using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InAndStart : MonoBehaviour
{
    [SerializeField] private IntroCam cam;
   // string sn = "\"Assets\\ysb\\New\\Scenes/Proto_4X4.unity";
    void Start()
    {
        
    }

  
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거에 들어오면 MoveDownY 함수를 호출합니다.
        if (other.CompareTag("Player"))
        {
            TriggerFunctions();
        }
    }

    public void TriggerFunctions()
    {
        StartCoroutine(TriggerFunctionsWithDelay());
    }

    private IEnumerator TriggerFunctionsWithDelay()
    {
        cam.MoveCam(2);
        //yield return null;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Proto_4X4");
    }
}
