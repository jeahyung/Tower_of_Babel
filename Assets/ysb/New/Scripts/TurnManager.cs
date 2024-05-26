using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Map manager_map;
    private PlayerMovement player;
    private ItemManager manager_Item;
    private SAManager manager_Action;

    
    [SerializeField]private UI_Turn ui_turn;

    public MobManager manager_Mob;

    public bool isPlayerTurn = false;
    public bool isEnemyTurn = true;

    public bool isDone = true;

    int bounusTurn = 0; //���ʽ� ��
    int turnCount = 0;

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

    public void SetMobManager(MobManager mob)
    {
        manager_Mob = mob;
    }

    public void StartGame()
    {
        manager_Mob = StageManager.instance.mob;
        player.SetControl(false);
        StartPlayerTurn();
    }
    public void EndGame()
    {
        //manager_map.ResetTile();
        isPlayerTurn = false;
        isEnemyTurn = false;
        isDone = false;
        ui_turn.ResetUI();

        turnCount = 0;
        player.SetControl(true);
    }

    public void StartPlayerTurn()
    {
        if (StageManager.instance.isPlaying == false) { return; }    //���� ���� ����

        if (isEnemyTurn == true || player.TurnEnd() == false) { return; }
        isPlayerTurn = true;
        isDone = true;

        player.SetUseEnergy();
        if(manager_Action == null) { manager_Action = FindObjectOfType<SAManager>(); }
        if (manager_Action.usedKing == true)
        {
            manager_Action.BonusUse();
        }
        else
        {
            manager_Action.SetActionBtn(true);  //�׼� ��ư Ȱ��ȭ
            manager_map.StartPlayerTurn(player.moveRange);
        }
        Debug.Log("player turn");
    }

    public void EndPlayerTurn()
    {
        if (StageManager.instance.isPlaying == false) { return; }    //���� ���� ����

        Debug.Log("player turn end");
        isPlayerTurn = false;
        isDone = true;

        if(UpgradeManager.instance.getBonusTurn() > 0)
        {
            UpgradeManager.instance.getBonusTurn(-1);
            StartPlayerTurn();

            return;
        }

        ItemInventory.instance.ChangeItem();    //������ ü����

        manager_Action.SetActionBtn(false);
        ui_turn.RotateObj(turnCount + 1);    //�� ui
        StartEnemyTurn();
    }

    public void StartEnemyTurn()
    {
        if (StageManager.instance.isPlaying == false) { return; }    //���� ���� ����

        if (isPlayerTurn == true || isEnemyTurn == true) { return; }
        isEnemyTurn = true;

        manager_Mob.ActMob();
        Debug.Log("���� ��");
    }

    public void EndEnemyTurn()
    {
        if (StageManager.instance.isPlaying == false) { return; }    //���� ���� ����

        if (isEnemyTurn == false) { return; }
        isEnemyTurn = false;
        Debug.Log("���� �� end");

        manager_Item.RemoveObj();

        turnCount++;

        ui_turn.RotateObj(turnCount + 1);    //�� ui
        StartPlayerTurn();
    }
}
