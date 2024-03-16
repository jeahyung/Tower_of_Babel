using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoxManager : MonoBehaviour
{
    public Vector2Int size; //퍼즐 크기
    public Transform center;

    public Vector2 minPoint;
    public Vector2 maxPoint;
    
    public List<BoxMove> boxes;
    public bool isOneMove = true;

    public List<BoxMove> moveBoxes;

    public void Chage()
    {
        isOneMove = !isOneMove;
    }
    private void Awake()
    {
        boxes = new List<BoxMove>();
        moveBoxes = new List<BoxMove>();
        boxes.AddRange(GetComponentsInChildren<BoxMove>());

        minPoint = new Vector2(center.position.x - (size.x / 2), center.position.y - (size.y / 2));
        maxPoint = new Vector2(center.position.x + (size.x / 2), center.position.y + (size.y / 2));
    }

    public void SetBoxData(WordData word, Vector3 dir)
    {
        for(int i = 0; i < boxes.Count; ++i)
        {
            //boxes[i].isOneMove = isOneMove; //한칸만 움직일까? 끝까지 움직일까?
            boxes[i].MoveBox(word, dir);
        }
    }

    public void MoveBox(WordData word, Vector3 dir, List<BoxMove> box)
    {
        //moveBoxes.Clear();
        moveBoxes = box;

        foreach(var b in moveBoxes)
        {
            //움직일 수 있는 블록이 있는가?
            if(b == null) { continue; }
            if(b.CanMove(word) == true) 
            {
                foreach (var mb in moveBoxes)
                {
                    if (mb != null)
                    {
                        mb.MoveBox(dir);//MoveBox(word, dir);
                    }
                }
                break;
            }
        }        
    }


}
