    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Map map;
    private Rigidbody rigid;
    private Animator anim;

    private MeshRenderer mesh;

    private TurnManager manager_Turn;
    private EnergySystem energySysteam;

    private bool isControl = true;
    private bool canMove = true;
    private bool isDamaged = false;
    public float smoothTime = 0.2f;

    public float moveSpeed = 3f;
    public int moveRange = 1;

    Transform body;
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
        mesh = GetComponent<MeshRenderer>();

        body = transform.GetChild(0);
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

    //에너지 사용
    public void UseEnergy()
    {
        energySysteam.UseEnergy();
    }
    //순간이동
    public void TeleportPlayer(Vector3 target)
    {
        if (target == Vector3.zero) { return; }
        if (canMove == false) { return; }

        //UseEnergy();

        Vector3 pos = new Vector3(target.x, transform.position.y, target.z);
        manager_Turn.isDone = false;
        StartCoroutine(Teleport(pos));
    }
    private IEnumerator Teleport(Vector3 target)
    {
        canMove = false;
        //애니메이션 or 이펙트 재생
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        transform.position = target;

        manager_Turn.EndPlayerTurn();
    }

    //이동
    public void SetPosition(Vector3 target)
    {
        if(target == Vector3.zero) { return; }
        if(canMove == false) { return; }

        UseEnergy();

        movePos = new Vector3(target.x, transform.position.y, target.z);
        manager_Turn.isDone = false;

        Vector3 direction = (target - transform.position).normalized;
        body.transform.forward = direction;

        anim.SetTrigger("isJump");

        //StartCoroutine(PlayerMove(pos));
    }

    public void StartMove()
    {
        StartCoroutine(PlayerMove(movePos));
    }
    private IEnumerator PlayerMove(Vector3 target)
    {
        canMove = false;
        Vector3 direction = (target - transform.position).normalized;
        Vector3 speed = direction * moveSpeed;

        while (Vector3.Distance(transform.position, target) >= 0.05f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target, ref speed, smoothTime);
            yield return null;
        }
        canMove = true;
        transform.position = target;

        manager_Turn.EndPlayerTurn();
    }

    public void MoveToStartPoint(Vector3 target)
    {
        if(isControl == false) { return; }
        SetControl(false);
        StartCoroutine(MoveToPoint(target));
    }

    IEnumerator MoveToPoint(Vector3 point)
    {
        while (Vector3.Distance(transform.position, point) >= 0.1f)
        {
            Vector3 direction = point - transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, point, ref direction, smoothTime);
            yield return null;
        }
        transform.position = new Vector3(point.x, transform.position.y, point.z);
    }

    public void SetUseEnergy()
    {
        energySysteam.useEnergy = 1;
    }


    #region 피격
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
        Vector3 target = tile.transform.position;
        Vector3 pos = new Vector3(target.x, transform.position.y, target.z);
        manager_Turn.isDone = false;

        map.nowTile = tile;
        StartCoroutine(MoveBack(pos, tile));
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
