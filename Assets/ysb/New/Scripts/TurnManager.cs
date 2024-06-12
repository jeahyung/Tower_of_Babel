using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Map manager_map;
    private PlayerMovement player;
    private ItemManager manager_Item;
    private SAManager manager_Action;

    [SerializeField] private float delayTime = 1f;
    [SerializeField]private UI_Turn ui_turn;

    public MobManager manager_Mob;

    public bool isPlayerTurn = false;
    public bool isEnemyTurn = true;

    public bool isDone = true;

    int bounusTurn = 0; //���ʽ� ��
    [SerializeField]int turnCount = 0;

    public int TurnCount => turnCount;
    public bool IsMyTurn => isPlayerTurn;

    private void Awake()
    {
        manager_map = FindObjectOfType<Map>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        manager_Item = player.GetComponent<ItemManager>();

        //manager_Mob = FindObjectOfType<MobManager>();
        manager_Action = FindObjectOfType<SAManager>();

        if(ui_turn == null)
        {
            ui_turn = FindObjectOfType<UI_Turn>();
        }
    }

    #region ���� ����
    public void SetMobManager(MobManager mob)
    {
        manager_Mob = mob;
    }

    public List<Tile> ShowMobTile()
    {
        return manager_Mob.ShowRook();
    }
    #endregion

    //���� ����
    public void StartGame()
    {
        manager_Mob = StageManager.instance.mob;
        player.SetControl(false);
        ui_turn.ResetObj();
        StartPlayerTurn();
    }
    public void EndGame()
    {
        StopAllCoroutines();
        //manager_map.ResetTile();
        isPlayerTurn = false;
        isEnemyTurn = false;
        isDone = true;
        ui_turn.ResetUI();

        turnCount = 0;
        player.SetControl(true);
    }
    public bool IsLastTile()
    {
        return manager_map.IsLastTile();
    }

    #region player turn
    public void StartPlayerTurn()
    {
        if (StageManager.instance.isPlaying == false) { return; }    //���� ���� ����
        if (isEnemyTurn == true || player.TurnEnd() == false || IsLastTile() == true) { return; }

        StartCoroutine(PlayerTurn());
    }
    private IEnumerator PlayerTurn()
    {
        isPlayerTurn = true;
        isDone = true;

        ui_turn.ShowImg(0);
        yield return new WaitForSeconds(delayTime);
        ui_turn.HideImg(0);

        player.SetUseEnergy();  //������ ����

        if (manager_Action == null) { manager_Action = FindObjectOfType<SAManager>(); }
        if (manager_Action.usedKing == true)
        {
            manager_Action.BonusUse();
        }
        else
        {
            ItemInventory.instance.ChangeItem();    //������ ü����

            manager_Action.SetActionBtn(true);  //�׼� ��ư Ȱ��ȭ
            manager_map.StartPlayerTurn(player.moveRange);
        }
        Debug.Log("player turn");
    }
    #endregion

    #region end player turn
    public void EndPlayerTurn()
    {
        if (StageManager.instance.isPlaying == false) { return; }    //���� ���� ����

        Debug.Log("player turn end");
        StartCoroutine(EndPlayer());        
    }
    private IEnumerator EndPlayer()
    {
        isPlayerTurn = false;
        isDone = true;

        if (UpgradeManager.instance.getBonusTurn() > 0)
        {
            UpgradeManager.instance.getBonusTurn(-1);
            StartPlayerTurn();

            yield break;
        }
        ui_turn.RotateObj(turnCount + 1);    //�� ui
        yield return new WaitForSeconds(delayTime);

        manager_Action.SetActionBtn(false);
        StartEnemyTurn();
    }
    #endregion

    #region enemy turn
    public void StartEnemyTurn()
    {
        if (StageManager.instance.isPlaying == false) { return; }    //���� ���� ����
        if (isPlayerTurn == true || isEnemyTurn == true || IsLastTile() == true) { return; }
        //isEnemyTurn = true;

        //manager_Mob.ActMob();
        //Debug.Log("���� ��");
        StartCoroutine(EnemyTurn());
    }
    private IEnumerator EnemyTurn()
    {
        isEnemyTurn = true;

        ui_turn.ShowImg(1);
        yield return new WaitForSeconds(delayTime);
        ui_turn.HideImg(1);

        manager_Mob.ActMob();
    }

    public void EndEnemyTurn()
    {
        if (StageManager.instance.isPlaying == false) { return; }    //���� ���� ����

        if (isEnemyTurn == false) { return; }
        //isEnemyTurn = false;
        //Debug.Log("���� �� end");

        //manager_Item.RemoveObj();

        //turnCount++;

        //ui_turn.RotateObj(turnCount + 1);    //�� ui
        //StartPlayerTurn();
        StartCoroutine(EndEnemy());
    }
    private IEnumerator EndEnemy()
    {
        isEnemyTurn = false;

        ui_turn.RotateObj(++turnCount + 1);    //�� ui
        yield return new WaitForSeconds(delayTime);

        manager_Item.RemoveObj();

        //turnCount++;
        StartPlayerTurn();
    }
    #endregion

    public void KingReset()
    {
        Debug.Log("KingReset!@@!@!@!@!@!@12");
        manager_Action.usedKing = false;
    }
}
