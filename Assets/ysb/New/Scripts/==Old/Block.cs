using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    protected CheckBox[] boxes; //���� ��� üũ �ڽ���
    protected Block[] nearBlocks; //���� ��ϵ�

    protected SpriteRenderer imgBox;    //���� ǥ�� �̹���

    //���� Ȯ��
    private bool canMove = true; //�ش� ������� ������ �� �ִ°�?
    private bool isNear = false; //�ش� ����� ������ �ִ°�?

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
        //���� ����� ������
        for (int i = 0; i < boxes.Length; ++i)
        {
            nearBlocks[i] = boxes[i].GetBlock();
        }
    }

    public void SetBlockState(bool b)
    {
        isNear = b;
    }

    //���� �����ֱ�
    public void ShowMyRangeBox()
    {
        imgBox.enabled = true;
    }
    
    //���� �����ֱ� ��
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
