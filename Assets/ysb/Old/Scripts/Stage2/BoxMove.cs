using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour
{
    public float blockSize = 1;

    public WordData wordData;
    public Vector3 direction;
    public float moveSpeed;

    public Vector3 tPoint;
    public float offset;

    public bool canMove = false; //������ �� �ִ°�?
    public bool nextObstacle = false;   //���� ��ֹ��� �ִ°�?
    private bool isOneMove = true;

    public float range;

    private SpriteRenderer rend;
    [SerializeField]
    private Sprite wordImg;

    //����� 2�谡 �Ǹ� �߽����� +2 �̵� / ����� 3�谡 �Ǹ� �߽����� + 4�̵�

    private void Awake()
    {
        //�ӽ�
        wordImg = wordData.wordImg;

        rend = GetComponentInChildren<SpriteRenderer>();
        rend.sprite = wordImg;
        offset = 4;
    }

    public bool CanMove(WordData word)
    {
        if(wordData == word)
        {
            return true;
        }
        return false;
    }
    //protected virtual bool FindObstacle()
    //{
    //    //���� ����
    //    range = offset / 2;

    //    RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, range + 1f);
    //    foreach (var h in hit)
    //    {
    //        if (h.collider.gameObject == this.gameObject) { continue; }
    //        if (h.collider != null)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    protected Vector3 CheckBlock(float range)
    {
        GameObject nearBlock = null;


        RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, range, 1 << LayerMask.NameToLayer("Object"));

        foreach(var h in hit)
        {
            if (h.collider.gameObject == gameObject) { continue; }
            if(nearBlock == null)
            {
                nearBlock = h.collider.gameObject;
                continue;
            }
            if (h.collider != null)
            {
                if(Vector3.Distance(transform.position, nearBlock.transform.position) 
                    > Vector3.Distance(transform.position, h.collider.transform.position))
                {
                    nearBlock = h.collider.gameObject;
                }
            }
        }

        if(nearBlock != null) { 
            return (nearBlock.transform.position);
        }
        return transform.position;
    }    
    private Vector3 CheckWall(float range)
    {
        RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, range);
        foreach (var h in hit)
        {
            if (h.collider.gameObject == gameObject) { continue; }
            if (h.collider != null)
            {
                if(h.collider.CompareTag("Wall"))
                {
                    return h.point;
                }
            }
        }
        return transform.position;
    }

    protected virtual bool FindObstacle()
    {
        //���� ����
        range = offset / 2;

        RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, range + 1f);
        foreach (var h in hit)
        {
            if (h.collider.gameObject == this.gameObject) { continue; }
            if (h.collider != null && h.collider.CompareTag("Wall"))
            {
                return true;
            }
            if(h.collider.TryGetComponent(out BoxMove box))
            {
                if(box.nextObstacle == true) { return true; }
            }
        }
        return false;
    }

    private bool CheckNextWall(float range)
    {
        RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, range);
        foreach (var h in hit)
        {
            if (h.collider.gameObject == gameObject) { continue; }
            if (h.collider != null)
            {
                if (h.collider.CompareTag("Wall"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public virtual void MoveBox(WordData word, Vector3 dir)
    {
        direction = dir;

        nextObstacle = CheckNextWall(offset / 2 + 1f);

        canMove = !FindObstacle();
        if (canMove == false) { return; }

        if (CheckBlock(offset) != transform.position) { return; }
        StartCoroutine(MoveOne());
        //if (isOneMove == true)
        //{
            
        //}
        //else
        //{
        //    if (CheckBlock(20) == transform.position)
        //    {
        //        tPoint = CheckWall(20) - ((direction * offset) / 2) - new Vector3((blockSize - 1), 0, 0) * 4;
        //    }
        //    else
        //    {
        //        tPoint = CheckBlock(20) - ((direction * blockSize) * offset);
        //    }

        //    Debug.Log(tPoint);
        //    StartCoroutine(MoveToEnd());
        //}
    }

    public virtual void MoveBox(Vector3 dir)
    {
        if(canMove == false) { return; }
        direction = dir;
        StartCoroutine(MoveOne());
    }

    IEnumerator MoveOne()
    {
        tPoint = transform.position + (direction * offset);
        while (true)
        {
            if (Vector3.Distance(transform.position, tPoint) <= 0.1f)
            {
                transform.position = tPoint;
                yield break;
            }
            transform.position = Vector3.MoveTowards(transform.position, tPoint, 1);
            yield return null;
        }
    }

    IEnumerator MoveToEnd()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, tPoint) <= 0.1f)
            {
                transform.position = tPoint;
                yield break;
            }
            transform.position = Vector3.MoveTowards(transform.position, tPoint, 1);
            yield return null;
        }
    }
}
