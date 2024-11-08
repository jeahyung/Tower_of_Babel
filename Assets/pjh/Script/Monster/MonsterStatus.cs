using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
     //x, y�� �ٲ���. x = y�� / y = x�� / 1 = ������,�� / -1 = ����, �Ʒ�          

    public Map map;
    public Tile curTile;  //���� ��ġ�� Ÿ��
    public Tile startTile;


    public int moveCount = 2;   //�����̴� ĭ ��
    public int visualRange = 2;

    public bool isEnd = true;  //�ൿ�� �����ߴ°�?
    public bool isDone = false;

    public List<Tile> range;    //������ �� �ִ� ����

    private void Start()
    {
        map = FindObjectOfType<Map>();
    }

   
        public void Patrol(Tile tile, Vector2Int moveDir)
        {
            curTile = tile;

            Vector2Int nextCoord = curTile.coord + moveDir;
            Tile nextTile = map.GetTile(nextCoord);

            if (nextTile == null || range.Contains(nextTile) == false)
            {
                moveDir = new Vector2Int(-moveDir.x, -moveDir.y);
                nextCoord = curTile.coord + moveDir;
                nextTile = map.GetTile(nextCoord);
            }
            EffectManage.Instance.PlayEffect("Monster_Move", this.transform.position);
            transform.forward = new Vector3(moveDir.y, 0, moveDir.x);

            tile.tileType = TileType.possible;
            tile.mob = null;
            StartCoroutine(MoveMob(nextTile));
        }

    private IEnumerator MoveMob(Tile nextTile)
    {
        //if(nextTile.tileType == TileType.impossible)
        //{
        //    Debug.Log("TileType.impossible choose Warring");
        //    yield break;
        //}
        if (nextTile == null)
        {
            //count = moveCount;
            isEnd = true;
            isDone = true;
            curTile.tileType = TileType.impossible;

            //manager_Mob.CheckMobAction();
            yield break;
        }

        float ypos = transform.position.y;
        Vector3 nextPos = new Vector3(nextTile.transform.position.x, ypos, nextTile.transform.position.z);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Move);

        map.TakeDamage(nextTile);

        while (Vector3.Distance(transform.position, nextPos) >= 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, 8f * Time.deltaTime);
            yield return null;
        }
        transform.position = nextPos;
        curTile = nextTile;
        nextTile.tileType = TileType.impossible;
        nextTile.mob = this.GetComponent<Mob>();

        yield return new WaitForSeconds(0.5f);

        //if (--count > 0)
        //{
        //    Act(); // ���� �ൿ
        //    yield break;
        //}

       // count = moveCount;
        isEnd = true;
        isDone = true;

        //manager_Mob.CheckMobAction();
    }


    class Chase
    {

    }

    class ReturnToStartPoint
    {

    }
}