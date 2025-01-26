using UnityEngine;

public class CarpetBoom : BossAttack
{
    public CarpetBoom(Boss boss, PlayerMovement player, Transform shootingPoint, BulletSO bullet) : base(boss, player, shootingPoint, bullet) { }

    const float CAMERA_MAX_HEIGHT = 30f / 2f;
    const float CAMERA_MAX_WIDTH = 30 / 2;
    //how long is going to last
    float droppingTime = 7;
    float droppingTimer;

    //gap between grenades 
    float droppingRate = .5f;
    float droppingRater;

    Vector2 dir;
    public override void InitAttack()
    {
        base.InitAttack();
        droppingRater = droppingRate;
        droppingTimer = 0;

        dir = new(boss.CenterPoint.x - boss.transform.position.x, 0);
        dir = dir.normalized;

    }

    public override void Attack()
    {
        boss.TakeAim();
        boss.transform.position = new Vector2(boss.transform.position.x, CAMERA_MAX_HEIGHT - 7);
        Vector2 droppingPo = new Vector2(boss.transform.position.x, boss.transform.position.y - 3);
        if (boss.transform.position.x >= CAMERA_MAX_WIDTH - 4) 
            dir = -dir;
        else if (boss.transform.position.x <= -CAMERA_MAX_WIDTH + 4)
        {
            dir = new(boss.CenterPoint.x - boss.transform.position.x, 0);
            dir = dir.normalized;
        }
            
        droppingTimer += Time.deltaTime;
        if (droppingTimer < droppingTime)
        {
            boss.AddForceBoss(dir,5,1);
            if(droppingRater >= droppingRate)
            {
                droppingRater = 0;
                Bullet grenade = boss.InstantiateBullet(bullet.bulletPrefab, droppingPo, Quaternion.identity);
                InitBullet(grenade);
                boss.DestroyBullet(grenade, 7f);
            }
            droppingRater += Time.deltaTime;
        }
        if (droppingTimer >= droppingTime)
        {
            isAttackDone = true;
            droppingTimer = 0;
            return;
        }
    }
}
