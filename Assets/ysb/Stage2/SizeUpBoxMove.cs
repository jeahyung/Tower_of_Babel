using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeUpBoxMove : BoxMove
{

    public Transform[] checkPoint;
    public Vector3 tempPoint;

    // ============================================Àå¾Ö¹° Å½Áö
    private bool CheckLeftObstacle()
    {
        range = offset / 2;

        RaycastHit[] hit = Physics.RaycastAll(checkPoint[0].position, direction, range + 1f);
        foreach (var h in hit)
        {
            if(h.collider.gameObject == this.gameObject) { continue; }
            if (h.collider.gameObject == checkPoint[0].gameObject) { continue; }
            if (h.collider != null)
            {
                return true;
            }
        }
        return false;
    }    
    private bool CheckLRightObstacle()
    {
        range = offset / 2;

        RaycastHit[] hit = Physics.RaycastAll(checkPoint[1].position, direction, range + 1);
        foreach (var h in hit)
        {
            if(h.collider.gameObject == this.gameObject) { continue; }
            if (h.collider.gameObject == checkPoint[1].gameObject) { continue; }
            if (h.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    protected override bool FindObstacle()
    {
        if(CheckLeftObstacle() == true || CheckLRightObstacle() == true)
        {
            return true;
        }
        return false;
    }


    // ============================================ºí·Ï Å½Áö

    private bool CheckLeftBlock()
    {
        range = offset / 2;

        RaycastHit[] hit = Physics.RaycastAll(checkPoint[0].position, direction, range + 1, 1 << LayerMask.NameToLayer("Object"));
        foreach (var h in hit)
        {
            if (h.collider.gameObject == this.gameObject) { continue; }
            if (h.collider.gameObject == checkPoint[0].gameObject) { continue; }
            if (h.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckLRightBlock()
    {
        range = offset / 2;

        RaycastHit[] hit = Physics.RaycastAll(checkPoint[1].position, direction, range + 1, 1 << LayerMask.NameToLayer("Object"));
        foreach (var h in hit)
        {
            if (h.collider.gameObject == this.gameObject) { continue; }
            if (h.collider.gameObject == checkPoint[1].gameObject) { continue; }
            if (h.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 CheckBlockY(float range)
    {
        GameObject nearBlock = null;


        RaycastHit[] hit = Physics.RaycastAll(checkPoint[0].position, direction, range, 1 << LayerMask.NameToLayer("Object"));

        foreach (var h in hit)
        {
            if (h.collider.gameObject == gameObject) { continue; }
            if (nearBlock == null)
            {
                nearBlock = h.collider.gameObject;
                continue;
            }
            if (h.collider != null)
            {
                if (Vector3.Distance(transform.position, nearBlock.transform.position)
                    > Vector3.Distance(transform.position, h.collider.transform.position))
                {
                    nearBlock = h.collider.gameObject;
                }
            }
        }

        if (nearBlock != null)
        {
            Vector3 interval = checkPoint[0].position - transform.position;
            return (nearBlock.transform.position - interval);
        }
        return transform.position;
    }
    
    private Vector3 CheckBlockX(float range)
    {
        GameObject nearBlock = null;


        RaycastHit[] hit = Physics.RaycastAll(transform.position, direction, range, 1 << LayerMask.NameToLayer("Object"));

        foreach (var h in hit)
        {
            if (h.collider.gameObject == gameObject) { continue; }
            if (nearBlock == null)
            {
                nearBlock = h.collider.gameObject;
                continue;
            }
            if (h.collider != null)
            {
                if (Vector3.Distance(transform.position, nearBlock.transform.position)
                    > Vector3.Distance(transform.position, h.collider.transform.position))
                {
                    nearBlock = h.collider.gameObject;
                }
            }
        }

        if (nearBlock != null)
        {
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
                if (h.collider.CompareTag("Wall"))
                {
                    return h.point;
                }
            }
        }
        return transform.position;
    }


    public override void MoveBox(WordData word, Vector3 dir)
    {
        direction = dir;
        if (wordData.wordId == word.wordId)
        {
            canMove = !FindObstacle();
            if (canMove == false) { return; }

            if (isOneMove == true)
            {
                if (CheckBlock(offset) != transform.position) { return; }
                StartCoroutine(MoveOne());
            }
            else
            {
                if (CheckBlock(20) == transform.position)
                {
                    tPoint = CheckWall(20) - ((direction * offset) / 2);
                }
                else
                {
                    if(direction.x != 0)
                    {
                        tPoint = CheckBlockX(20) - ((direction * blockSize) * offset);
                    }
                    else
                    {
                        tPoint = CheckBlockY(20) - ((direction * blockSize) * offset);
                    }

                }

                Debug.Log(tPoint);
                StartCoroutine(MoveToEnd());
            }
        }
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
