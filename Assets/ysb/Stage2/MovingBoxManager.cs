using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class MovingBoxManager : MonoBehaviour
{
    public Vector2Int size; //���� ũ��
    public Transform center;

    public Vector2 minPoint;
    public Vector2 maxPoint;
    
    public List<BoxMove> boxes;

    public List<int> cntNum = new List<int>();

    public bool isOneMove = true;

    public void Chage()
    {
        isOneMove = !isOneMove;
    }
    private void Awake()
    {
        boxes = new List<BoxMove>();
        boxes.AddRange(GetComponentsInChildren<BoxMove>());

        for (int i = 0; i < boxes.Count; ++i)
        {
            cntNum.Add(boxes[i].wordData.wordId);//wordData.wordId
        }


            minPoint = new Vector2(center.position.x - (size.x / 2), center.position.y - (size.y / 2));
        maxPoint = new Vector2(center.position.x + (size.x / 2), center.position.y + (size.y / 2));
    }

    public void SetBoxData(WordData word, Vector3 dir)
    {
        for(int i = 0; i < boxes.Count; ++i)
        {
            boxes[i].isOneMove = isOneMove; //��ĭ�� �����ϱ�? ������ �����ϱ�?
            boxes[i].MoveBox(word, dir);
        }
    }


}