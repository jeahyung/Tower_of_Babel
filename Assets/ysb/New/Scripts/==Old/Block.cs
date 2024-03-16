using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    protected CheckBox[] boxes; //인접 블록 체크 박스들
    protected Block[] nearBlocks; //인접 블록들

    protected SpriteRenderer imgBox;    //범위 표시 이미지

    //여부 확인
    private bool canMove = true; //해당 블록으로 움직일 수 있는가?
    private bool isNear = false; //해당 블록이 인접해 있는가?

    public bool IsNear => isNear;
    public bool CanMove => canMove;
    

    private void Awake()
    {
        boxes = GetComponentsInChildren<CheckBox>();
        nearBlocks = new Block[4];

        imgBox = GetComponentInChildren<SpriteRenderer>();
        HideMyRangeBox();
    }

    protected virtual void Start()
    {
        //인접 블록을 가져옮
        for (int i = 0; i < boxes.Length; ++i)
        {
            nearBlocks[i] = boxes[i].GetBlock();
        }
    }

    public void SetBlockState(bool b)
    {
        isNear = b;
    }

    //범위 보여주기
    public void ShowMyRangeBox()
    {
        imgBox.enabled = true;
    }
    
    //범위 보여주기 끝
    public void HideMyRangeBox()
    {
        imgBox.enabled = false;
    }


    public void ShowMoveRange()
    {
        foreach (Block block in nearBlocks)
        {
            if (block != null)
            {
                if(block.canMove == false) { continue; }
                block.ShowMyRangeBox();
                block.SetBlockState(true);
            }
        }
    }
    
    public void HideMoveRange()
    {
        foreach (Block block in nearBlocks)
        {
            if (block != null)
            {
                block.HideMyRangeBox();
                block.SetBlockState(false);
            }
        }
    }


    public Vector3 GetPosition()
    {
        if(canMove == false)
        {
            return Vector3.zero;
        }
        return new Vector3(transform.position.x, 0, transform.position.z);
    }
}
