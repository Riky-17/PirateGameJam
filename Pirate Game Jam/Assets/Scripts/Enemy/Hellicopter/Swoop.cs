using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Swoop : BossAttack
{
    public Swoop(Boss boss, PlayerMovement player, BulletSO bullet, PickUpItemSO[] Items) : base(boss, player, bullet)
    {
        items = Items;
    }
    const float CAMERA_MAX_HEIGHT = 17 / 2f;
    const float CAMERA_MAX_WIDTH = 30 / 2f;

    PickUpItemSO[] items;

    bool isLeft = false;
    bool isUp = false;

    int SlamsAmount = 3;
    int slamsCount;

    float droppingRate = 0.6f;
    float droppingRater;

    float heliSpeed = 30;

    Vector2 dir;
    Vector2 dirY;

    public override void InitAttack()
    {
        base.InitAttack();
        boss.Stop();

        isLeft = false;
        isUp = false;

        slamsCount = 0;

        droppingRater = droppingRate;


        dir = new(boss.CenterPoint.x - boss.transform.position.x, 0);
        dir = dir.normalized;

        dirY = new(0, boss.CenterPoint.y - boss.transform.position.y);
        dirY = dirY.normalized;
    }
    public override void Attack()
    {
        boss.TakeAim();
      

        if (!isUp)
        {
            //bring him up
            if (boss.transform.position.y >= CAMERA_MAX_HEIGHT - 5f)
            {
                isUp = true;
                return;
            }

            boss.AddForceBoss(Vector2.up, boss.Speed * 1.5f, 5);

            return;

        }
        if (!isLeft && isUp)
        {
            //bring him left
            if (boss.transform.position.x <= boss.CenterPoint.x - CAMERA_MAX_WIDTH + 2)
            {
                isLeft = true;
                return;
            }
            boss.AddForceBoss(Vector2.left, boss.Speed * 1.5f, 5);
            return;
        }
        if (slamsCount >= SlamsAmount)
        {
            if ((boss.transform.position.x < boss.CenterPoint.x - CAMERA_MAX_WIDTH && dir.x < 0) || (boss.transform.position.x > boss.CenterPoint.x + CAMERA_MAX_WIDTH && dir.x > 0))
            {
                slamsCount++;
                dir = -dir;

            }
            boss.AddForceBoss(dir, heliSpeed, 5);
            isAttackDone = true;
            return;
        }

        if ((boss.transform.position.x < boss.CenterPoint.x - CAMERA_MAX_WIDTH && dir.x < 0) || (boss.transform.position.x > boss.CenterPoint.x + CAMERA_MAX_WIDTH && dir.x > 0))
        {
            slamsCount++;
            dir = -dir;

        }
        boss.AddForceBoss(dir, heliSpeed, 5);


        if (droppingRater >= droppingRate)
        {
            droppingRater = 0f;

            int randomItem = Random.Range(0, items.Length);
            Quaternion itemRot = Quaternion.LookRotation(Vector3.forward, Vector2.right);
            PickableItem  item = boss.InstantiateItem(items[randomItem].item, (Vector2)boss.transform.position + Vector2.down * 3, itemRot);

            //float x = Random.Range(-CAMERA_MAX_WIDTH + 2, CAMERA_MAX_WIDTH - 2);
            //Vector2 pos = new(x, CAMERA_MAX_HEIGHT + 3);
            //PickableItem item = boss.InstantiateItem(items[randomItem].item, pos, Quaternion.identity);

            if (boss is HellicopBoss hellicop)
                hellicop.itemsOnGame.Add(item);

        }
        else
            droppingRater += Time.deltaTime;

        //if ((boss.transform.position.y < boss.CenterPoint.y - CAMERA_MAX_WIDTH + 5 && dir.y < 0) || (boss.transform.position.y > boss.CenterPoint.y + CAMERA_MAX_HEIGHT -3 && dir.y > 0))
        //{
        //    dirY = -dirY;
        //}
        // boss.AddForceBoss(dirY, 10, 2);



    }


}
