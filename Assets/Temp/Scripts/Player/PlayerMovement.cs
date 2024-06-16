using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerMovement : MonoBehaviour
{
    #region 기본 클래스
    private Map map;
    private Rigidbody rigid;
    private Animator anim;

    private TurnManager manager_Turn;
    private EnergySystem energySysteam;
    #endregion

    //Move
    Vector3 startPos, endPos;
    float endZ, endX;    //z, x, height
    public float h = 1f;
    
    int jumpCount = 0;  //점프 횟수

    //Roate
    Vector3 to, from;

    public float timer;
    public float timeToFloor;    //땅에 닫기까지 걸리는 시간


    public bool isControl = false;
    private bool canMove = true;
    private bool isDamaged = false;
    public float smoothTime = 0.2f;

    public float moveSpeed = 3f;
    public int moveRange = 1;

    public int degree_back = 0;   //되돌올 때 회전할 각

    public bool TurnEnd()
    {
        if(isDamaged == true) { return false; }
        return true;
    }
    private void Awake()
    {
        map = FindObjectOfType<Map>();
        manager_Turn = GetComponent<TurnManager>();
        energySysteam = GetComponent<EnergySystem>();

        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        moveRange += UpgradeManager.instance.getBonusRange();
    }

    public void SetControl(bool b)
    {
        isControl = b;
        rigid.velocity = Vector3.zero;
    }

    #region 에너지 사용
    //에너지 소모량 설정
    public void SetUseEnergy()
    {
        energySysteam.useEnergy = 1;
    }
    //에너지 사용
    public bool UseEnergy()
    {
        return energySysteam.UseEnergy();
    }
    #endregion

    #region 순간이동
    public void TeleportPlayer(Vector3 target)
    {
        if (target == Vector3.zero || canMove == false) { return; }

        Vector3 pos = new Vector3(target.x, transform.position.y, target.z);
        manager_Turn.isDone = false;
        StartCoroutine(Teleport(pos));
    }
    private IEnumerator Teleport(Vector3 target)
    {
        canMove = false;
        //애니메이션 or 이펙트 재생
        yield return new WaitForSeconds(0.5f);

        transform.position = target;
        canMove = true;

        manager_Turn.EndPlayerTurn();
    }
    #endregion


    //플레이어 턴 종료
    public void EndPlayerTurn()
    {
        degree_back = 0;
        canMove = true;
        anim.SetInteger("isRotate", 0);
        manager_Turn.EndPlayerTurn();

        rt = -1;
    }



    #region Rotate

    //회전
    private void RotatePlayer(int rot)
    {
        degree_back = Mathf.Abs(rot - 9);
        to = new Vector3(0, 0, 0);
        switch (rot)
        {
            case 1:
                StartJump();
                return;
            case 2:
                from = new Vector3(0, -45, 0);    //left 45
                break;
            case 3:
                from = new Vector3(0, -90, 0);    //left 90
                break;
            case 4:
                from = new Vector3(0, -135, 0);    //left 135
                break;
            case 5:
                from = new Vector3(0, 135, 0);    //right 135
                break;
            case 6:
                from = new Vector3(0, 90, 0);    //right 90
                break;
            case 7:
                from = new Vector3(0, 45, 0);    //right 45
                break;
            case 8:
                from = new Vector3(0, 180, 0);    //후면
                break;
            default:
                from = new Vector3(0, transform.rotation.y, 0);
                return;
        }
        anim.SetInteger("isRotate", rot);
        anim.SetTrigger("StartRotate");
        float speed = from.y == 0 ? 0.5f : 0.5f * Mathf.Abs(from.y / 45);
        transform.DORotate(from, speed).OnComplete(() => EndRotate());

    }

    private void RotateBack()
    {
        from = new Vector3(0, 0, 0);
        anim.SetInteger("isRotate", degree_back);

        anim.SetTrigger("StartRotate");
        transform.DORotate(from, 1f).OnComplete(() => EndPlayerTurn());
    }
    public void EndRotate()
    {
        transform.DOKill();
        anim.SetInteger("isRotate", 0);
        anim.SetTrigger("EndRotate");
        StartJump();

    }
    #endregion

    #region Move
    #region SetTarget
    public void SetPosition(Vector3 target, int rot)
    {
        if(target == Vector3.zero || canMove == false) { return; }
        canMove = false;
        h = 0f;

        manager_Turn.isDone = false;
        rigid.useGravity = false;   //중력을 끈다.

        //시작점
        startPos = transform.position;
        //끝점
        endX = target.x - transform.position.x;
        endZ = target.z - transform.position.z;
        endPos = startPos + new Vector3(endX, 0, endZ);

        //에너지 사용
        if(UpgradeManager.instance.getNoneEnergy() == false) { if(UseEnergy() == false) { return; } }
        //회전
        RotatePlayer(rot);
    }

    //여러 번 점프
    Vector3 tg;
    int rt;

    public void SetPosition_Continue(int count, Vector3 target, int rot)
    {
        if (target == Vector3.zero || canMove == false) { return; }
        canMove = false;
        h = 2f;

        manager_Turn.isDone = false;
        rigid.useGravity = false;   //중력을 끈다.

        //시작점
        startPos = transform.position;
        //끝점
        endX = target.x - transform.position.x;
        endZ = target.z - transform.position.z;
        endPos = startPos + new Vector3(endX, 0, endZ);

        //에너지 사용
        if (UpgradeManager.instance.getNoneEnergy() == false) { if (UseEnergy() == false) { return; } }
        //회전
        RotatePlayer(rot);
    }
    //public void SetPosition_Continue(int count, Vector3 target, int rot)
    //{
    //    jumpCount = count;
    //    if (jumpCount <= 0) 
    //    {
    //        jumpCount = 0;
    //        if (degree_back != 8) { RotateBack(); }
    //        else
    //        {
    //            if (manager_Turn.IsLastTile() == false)  //마지막 타일이 아닐때만 턴 넘김
    //            {
    //                anim.SetTrigger("EndRotate");
    //                EndPlayerTurn();
    //            }
    //        }
    //        return;
    //    }
    //    tg = target;
    //    rt = rt == rot ? 0 : rot;   //방향이 같다면 회전X

    //    manager_Turn.isDone = false;
    //    rigid.useGravity = false;   //중력을 끈다.

    //    //시작점
    //    startPos = transform.position;
    //    //끝점
    //    endX = (target.x - transform.position.x) / jumpCount;
    //    endZ = (target.z - transform.position.z) / jumpCount;
    //    endPos = startPos + new Vector3(endX, 0, endZ);

    //    //Debug.Log("1111111");

    //    if (canMove == true)
    //    {
    //        if (UpgradeManager.instance.getNoneEnergy() == false) { if (UseEnergy() == false) { return; } }
    //        RotatePlayer(rot);
    //        canMove = false;
    //    }
    //    if(rt == 0)
    //    {
    //        StartJump();
    //    }
    //}
    #endregion

    public void StartJump()
    {
        anim.SetTrigger("StartJump");
        //anim.SetBool("isJump", true);
    }

    public void StartMove()
    {
        if (h > 0) { StartCoroutine(MoveToEnd_Long()); Debug.Log("jjjjjjump"); }
        else { StartCoroutine(MoveToEnd()); Debug.Log("nnnnnnnnonjump"); }
    }

    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
        var mid = Vector3.Lerp(start, end, t);
        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
    protected IEnumerator MoveToEnd_Long()
    {
        timer = 0;
        while (Vector3.Distance(transform.position, endPos) >= 0.2f)//transform.position.y >= startPos.y)
        {
            timer += Time.deltaTime;
            Vector3 tempPos = Parabola(startPos, endPos, h, timer);
            transform.position = tempPos;
            yield return null;//new WaitForEndOfFrame();
        }
        transform.position = endPos;
        rigid.useGravity = true;

        if (degree_back != 8)
        {
            RotateBack();
        }
        else
        {
            if (manager_Turn.IsLastTile() == false)  //마지막 타일이 아닐때만 턴 넘김
            {
                EndPlayerTurn();
            }
        }
        canMove = true;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Player_Step);
    }

    protected IEnumerator MoveToEnd()
    {
        timer = 0;
        //anim.SetBool("isJump", false);
        while(Vector3.Distance(transform.position, endPos) >= 0.2f) //(transform.position.y >= startPos.y)
        {
            timer += Time.deltaTime;
            //Vector3 tempPos = Parabola(startPos, endPos, h, timer);
            //transform.position = tempPos;
            Vector3 pos = Vector3.Lerp(startPos, endPos, timer);
            transform.position = pos;
            yield return new WaitForEndOfFrame();
        }

        transform.position = endPos;
        rigid.useGravity = true;
        if (jumpCount > 0)
        {
            jumpCount--;
            SetPosition_Continue(jumpCount, tg, rt); //다시 점프(회전 갱신)
        }
        else
        {
            if (degree_back != 8)
            {
                RotateBack();
            }
            else
            {
                if (manager_Turn.IsLastTile() == false)  //마지막 타일이 아닐때만 턴 넘김
                {
                    EndPlayerTurn();
                }
            }
        }
        canMove = true;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Player_Step);
    }
    #endregion

    public void MoveToStartPoint(Vector3 target)
    {
        if(isControl == false) { return; }
        SetControl(false);
        StartCoroutine(MoveToPoint(target));
    }

    IEnumerator MoveToPoint(Vector3 point)
    {
        while (Vector3.Distance(transform.position, point) >= 0.5f)
        {
            Vector3 direction = point - transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, point, ref direction, smoothTime);
            anim.SetBool("isRun", true);
            yield return null;
        }
        transform.position = new Vector3(point.x, transform.position.y, point.z);
        anim.SetBool("isRun", false);
    }


    #region 피격
    public void TakeDamage()
    {
        //if(isDamaged == true) { return; }
        isDamaged = true;

        int per = 0;
        if(UpgradeManager.instance.GetSANum() == 0) { per = 2; }
        else if(UpgradeManager.instance.GetSANum() == 1) { per = 3; }
        else { per = 1; }

        int dmg = 3 * per;

        anim.SetTrigger("isDamaged");
        energySysteam.UseEnergy(dmg);
        DamagedMove(map.CheckNearTile());
    }
    public void DamagedMove(Tile tile)
    {
        if(tile == null) { return; }
        //if(canMove == false) { return; }
        canMove = false;
        h = 1.5f;

        Vector3 target = tile.transform.position;
        Vector3 pos = new Vector3(target.x, transform.position.y, target.z);
        manager_Turn.isDone = false;

        //회전
        degree_back = map.HowRotate(tile);
        //RotatePlayer_Pos();

        map.nowTile = tile;
        map.playerTile = tile;

        rigid.useGravity = false;
        startPos = transform.position;
        endX = target.x - transform.position.x;
        endZ = target.z - transform.position.z;
        endPos = startPos + new Vector3(endX, 0, endZ);

        //넉백
        StartCoroutine(MoveBack());
        //StartCoroutine(MoveBack(pos, tile));
    }
    private IEnumerator MoveBack()
    {
        timer = 0;
        while (Vector3.Distance(transform.position, endPos) >= 0.2f)
        {
            timer += Time.deltaTime;
            Vector3 tempPos = Parabola(startPos, endPos, h, timer);
            transform.position = tempPos;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPos;
        rigid.useGravity = true;

        isDamaged = false;
        canMove = true;

        //if (manager_Turn.IsLastTile() == false)  //마지막 타일이 아닐때만 턴 넘김
        //{
        //    manager_Turn.StartPlayerTurn();
        //}
    }
    private IEnumerator MoveBack(Vector3 target, Tile tile)
    {
        canMove = false;
        while (Vector3.Distance(transform.position, target) >= 0.05f)
        {
            Vector3 direction = target - transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref direction, smoothTime);
            yield return null;
        }
        canMove = true;
        transform.position = target;

        isDamaged = false;
        manager_Turn.StartPlayerTurn();
    }
    #endregion

    public void UseItem()
    {
        anim.SetTrigger("isUse");
    }
}
