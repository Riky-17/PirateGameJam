using UnityEngine;

public class CarpetBoom : BossAttack
{
    public CarpetBoom(Boss boss, PlayerMovement player, BulletSO bullet) : base(boss, player, bullet) { }

    const float CAMERA_MAX_HEIGHT = 17 / 2f;
    const float CAMERA_MAX_WIDTH = 30 / 2f;
    
    bool isHigh = false;

    int SlamsAmount = 3;
    int slamsCount;

    //gap between grenades 
    float droppingRate = .5f;
    float droppingRater;

    Vector2 dir;

    public override void InitAttack()
    {
        base.InitAttack();
        boss.Stop();

        slamsCount = 0;

        droppingRater = droppingRate;

        dir = new(boss.CenterPoint.x - boss.transform.position.x, 0);
        dir = dir.normalized;

        isHigh = false;
    }

    public override void Attack()
    {
        boss.TakeAim();
        if(!isHigh)
        {
            if(boss.transform.position.y >= CAMERA_MAX_HEIGHT - 1.5f)
            {
                isHigh = true;
                return;
            }

            boss.AddForceBoss(Vector2.up, boss.Speed, 2);
            return;
        }

        if(slamsCount >= SlamsAmount)
        {
            isAttackDone = true;
            return;
        }

        if((boss.transform.position.x < boss.CenterPoint.x - CAMERA_MAX_WIDTH && dir.x < 0) || (boss.transform.position.x > boss.CenterPoint.x + CAMERA_MAX_WIDTH && dir.x > 0))
        {
            slamsCount++;
            dir = -dir;
        }

        boss.AddForceBoss(dir, 8, 5);

        if(droppingRater >= droppingRate)
        {
            droppingRater = 0;
            Quaternion grenadeRot = Quaternion.LookRotation(Vector3.forward, Vector2.right);
            Bullet grenade = boss.InstantiateBullet(bullet.bulletPrefab, (Vector2)boss.transform.position + Vector2.down * 3, grenadeRot);
            InitBullet(grenade);
            boss.DestroyBullet(grenade, 1.5f);
        }
        else
            droppingRater+= Time.deltaTime;
    }
}
