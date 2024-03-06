using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeProblem : MonoBehaviour
{

    //  public List<GameObject> gameObjects = new List<GameObject>();
    //한 문제를 하나의 프리팹으로 만들어서 리스트에 저장한다.
    //하여 일정한 패턴을 가진 문제가 출시되는 방식
    List<int> intList = new List<int>();

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            OtherProblem();
        }
    }

    public void OtherProblem()
    {

    }
}
