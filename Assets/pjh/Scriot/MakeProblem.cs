using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeProblem : MonoBehaviour
{

    //  public List<GameObject> gameObjects = new List<GameObject>();
    //�� ������ �ϳ��� ���������� ���� ����Ʈ�� �����Ѵ�.
    //�Ͽ� ������ ������ ���� ������ ��õǴ� ���
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
