using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoxGrid : MonoBehaviour
{
    public List<BoxMove> boxes;
    public CheckBox_Old[] checkBoxes;
    public MovingBoxManager manager_Box;
    public Vector3 direction;

    //추가
    public WordData word;
    public MovingBoxPuzzleManager manager_puzzle;//추가

    private void Awake()
    {
        boxes = new List<BoxMove>();

        if (manager_Box == null) { manager_Box = FindObjectOfType<MovingBoxManager>(); }
        manager_puzzle = FindObjectOfType<MovingBoxPuzzleManager>();//추가점
    }

    public void SetWord(WordData data)
    {
        if (data != null)
        {
            CheckBoxes();
            manager_Box.MoveBox(data, direction, boxes);//SetBoxData(data, direction);
        }
    }

    public void CheckBoxes()
    {
        boxes.Clear();
        foreach(var box in checkBoxes)
        {
            boxes.Add(box.CheckBlock(direction));
        }
    }

    //추가
    public void SetWord()
    {
        CheckBoxes();
        manager_Box.MoveBox(word, direction, boxes);
        manager_puzzle.isAct = false;
    }
}
