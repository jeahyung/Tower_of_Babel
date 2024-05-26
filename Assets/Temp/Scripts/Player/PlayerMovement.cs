using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerMovement : MonoBehaviour
{
    #region �⺻ Ŭ����
    private Map map;
    private Rigidbody rigid;
    private Animator anim;

    private TurnManager manager_Turn;
    private EnergySystem energySysteam;
    #endregion

    public Vector3 startPos, endPos;

    public float timer;
    public float timeToFloor;    //���� �ݱ���� �ɸ��� �ð�

    public float endZ;
    public float endX;

    public float h;

    private bool isControl = true;
    private bool canMove = true;
    private bool isDamaged = false;
    public float smoothTime = 0.2f;

    public float moveSpeed = 3f;
    public int moveRange = 1;

    public int degree_back = 0;   //�ǵ��� �� ȸ���� ��

    PlayerAnim body;
    Vector3 movePos;
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

        anim.SetInteger("isTurn", degree_back);
        body = GetComponentInChildren<PlayerAnim>();
    }
    private void Start()
    {
        moveRange += UpgradeManager.instance.getBonusRange();
    }
    void Update()
    {
        if(isControl == false) { return; }
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        rigid.velocity = new Vector3(x * 5, rigid.velocity.y, z * 5);
    }

    public void SetControl(bool b)
    {
        isControl = b;
        rigid.velocity = Vector3.zero;
    }

    #region ������ ���
    //������ �Ҹ� ����
    public void SetUseEnergy()
    {
        energySysteam.useEnergy = 1;
    }
    //������ ���
    public void UseEnergy()
    {
        energySysteam.UseEnergy();
    }
    #endregion

    #region �����̵�
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
        //�ִϸ��̼� or ����Ʈ ���
        yield return new WaitForSeconds(0.5f);

        transform.position = target;
        canMove = true;

        manager_Turn.EndPlayerTurn();
    }
    #endregion


    //�÷��̾� �� ����
    public void EndPlayerTurn()
    {
        //anim.applyRootMotion = false;
        anim.SetInteger("isTurn", 10);
        degree_back = 0;

        //body.CanJump = true; //���� �����ϵ���
        canMove = true;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));    //���� ����

        anim.SetBool("isEnd", true);
        manager_Turn.EndPlayerTurn();
    }

    //ȸ��
    public void RotatePlayer_Anim(int i)
    {
        degree_back = Mathf.Abs(i - 9);   //�ٽ� �ǵ��ƿ� �� ȸ���� ��
        //������ ���
        if (i == 1)
        {
            StartJump();    //�����̶�� �ٷ� �����Ѵ�.
            return;
        }
        //������ �������� ȸ���Ѵ�.
        anim.SetInteger("isTurn", i);
        anim.SetTrigger("isRotate");
    }
    public void RotateBack_Anim(int i)
    {
        degree_back = Mathf.Abs(i - 9);   //�ٽ� �ǵ��ƿ� �� ȸ���� ��
        //������ �������� ȸ���Ѵ�.
        anim.SetInteger("isTurn", i);
        anim.SetTrigger("isRotate");
    }
    public void RotatePlayer_Pos()
    {
        int rot = Mathf.Abs(degree_back - 9);
        switch(rot)
        {
            case 1:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));    //���� ����
                break;
            case 2:
                transform.rotation = Quaternion.Euler(new Vector3(0,-45, 0));    //left 45
                break;
            case 3:
                transform.rotation = Quaternion.Euler(new Vector3(0,-90, 0));    //left 90
                break;
            case 4:
                transform.rotation = Quaternion.Euler(new Vector3(0,-135, 0));    //left 135
                break;
            case 5:
                transform.rotation = Quaternion.Euler(new Vector3(0,135, 0));    //right 135
                break;
            case 6:
                transform.rotation = Quaternion.Euler(new Vector3(0, 90 ,0));    //right 90
                break;
            case 7:
                transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0));    //right 45
                break;
            case 8:
                transform.rotation = Quaternion.Euler(new Vector3(0, 179, 0));    //�ĸ�
                break;
        }
    }

    //�̵�
    public void SetPosition(Vector3 target, int rot)
    {
        if(target == Vector3.zero || canMove == false) { return; }
        canMove = false;
        anim.SetBool("isEnd", false);

        manager_Turn.isDone = false;

        rigid.useGravity = false;   //�߷��� ����.

        //������
        startPos = transform.position;
        //����
        endX = target.x - transform.position.x;
        endZ = target.z - transform.position.z;
        endPos = startPos + new Vector3(endX, 0, endZ);

        UseEnergy();    //������ ���    
        RotatePlayer_Anim(rot); //ȸ�� ���
    }

    public void StartJump()
    {
        //if(body.CanJump == false) { return; }
        //ȸ�� - �̵� ��������
        RotatePlayer_Pos();

        //���� ��� ���
        anim.SetBool("isJump", true);
        //anim.SetTrigger("isJump");
        //body.CanJump = false;
    }

    public void StartMove()
    {
        StartCoroutine(MoveToEnd());
    }

    protected static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
        var mid = Vector3.Lerp(start, end, t);
        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    protected IEnumerator MoveToEnd()
    {
        timer = 0;
        anim.SetBool("isJump", false);
        while (transform.position.y >= startPos.y)
        {
            timer += Time.deltaTime;
            Vector3 tempPos = Parabola(startPos, endPos, h, timer);
            transform.position = tempPos;
            yield return new WaitForEndOfFrame();
        }

        transform.position = endPos;
        rigid.useGravity = true;
        //canMove = true;


        if(degree_back != 8) { RotateBack_Anim(degree_back); }
        else{ EndPlayerTurn(); }

        //if (degree_back == 1) { RotatePlayer_Anim(1); }
        //else if (degree_back != 8) { RotatePlayer_Anim(degree_back);  }  //���� ���� �������� �پ��� ��
        //else { EndPlayerTurn(); }   //�������� �پ��� ���� �ٷ� �� ����
    }

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
            yield return null;
        }
        transform.position = new Vector3(point.x, transform.position.y, point.z);
    }


    #region �ǰ�
    public void TakeDamage()
    {
        if(isDamaged == true) { return; }
        isDamaged = true;

        int per = 0;
        if(UpgradeManager.instance.GetSANum() == 0) { per = 2; }
        else if(UpgradeManager.instance.GetSANum() == 1) { per = 3; }
        else { per = 1; }

        int dmg = 3 * per;

        energySysteam.UseEnergy(dmg);
        DamagedMove(map.CheckNearTile());
    }
    public void DamagedMove(Tile tile)
    {
        if(tile == null) { return; }
        if(canMove == false) { return; }
        canMove = false;

        Vector3 target = tile.transform.position;
        Vector3 pos = new Vector3(target.x, transform.position.y, target.z);
        manager_Turn.isDone = false;

        //ȸ��
        degree_back = map.HowRotate(tile);
        //RotatePlayer_Pos();

        map.nowTile = tile;

        rigid.useGravity = false;
        startPos = transform.position;
        endX = target.x - transform.position.x;
        endZ = target.z - transform.position.z;
        endPos = startPos + new Vector3(endX, 0, endZ);

        //�˹�
        StartCoroutine(MoveBack());
        //StartCoroutine(MoveBack(pos, tile));
    }
    private IEnumerator MoveBack()
    {
        timer = 0;
        while (transform.position.y >= startPos.y)
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
}
