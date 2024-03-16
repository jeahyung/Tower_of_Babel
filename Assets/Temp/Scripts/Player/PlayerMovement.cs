    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigid;
    private MeshRenderer mesh;

    private TurnManager manager_Turn;
    private EnergySysteam energySysteam;

    private bool isControl = true;
    private bool canMove = true;
    public float smoothTime = 0.2f;

    public int moveRange = 1;


    private void Awake()
    {
        manager_Turn = GetComponent<TurnManager>();
        energySysteam = GetComponent<EnergySysteam>();

        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        moveRange += UpgradeManager.instance.bonusRange;
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
    public void UseEnergy(int i = 1)
    {
        energySysteam.UseEnergy(i);
    }
    //순간이동
    public void TeleportPlayer(Vector3 target)
    {
        if (target == Vector3.zero) { return; }
        if (canMove == false) { return; }

        UseEnergy();

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

        Vector3 pos = new Vector3(target.x, transform.position.y, target.z);
        manager_Turn.isDone = false;
        StartCoroutine(PlayerMove(pos));
    }

    private IEnumerator PlayerMove(Vector3 target)
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

        manager_Turn.EndPlayerTurn();
    }
}
