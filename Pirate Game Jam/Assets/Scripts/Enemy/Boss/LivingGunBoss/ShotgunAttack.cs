using UnityEngine;

public class ShotgunAttack : BossAttack
{
    public ShotgunAttack(LivingGunBoss boss, PlayerMovement player, LivingGunBossGun shotgunBoss) : base(boss, player, null) 
    { 
        bossGun = boss; 
        this.shotgunBoss = shotgunBoss; 
    }

    const float CAMERA_MAX_HEIGHT = 17 / 2f;
    const float CAMERA_MAX_WIDTH = 30 / 2f;

    LivingGunBoss bossGun;
    LivingGunBossGun shotgunBoss;

    float shootWaitTime = .5f;
    float shootWaitTimer;
    
    float shotBehindTime = .75f;
    float shotBehindTimer;

    bool hasShotUp = false;
    bool hasShotBehind = false;

    bool shotFromAbove = false;

    float bulletAboveTime = 1;
    float bulletAboveTimer;

    float bulletBehindTime = .3f;
    float bulletBehindTimer;

    public override void InitAttack()
    {
        base.InitAttack();
        bossGun.canMove = false;
        bossGun.canAutoAttack = false;
        bossGun.StopAngularVelocity();
        shootWaitTimer = 0;
        bulletAboveTimer = 0;
        bulletBehindTimer = 0;
        hasShotUp = false;
        hasShotBehind = false;
        shotFromAbove = false;
    }

    public override void Attack()
    {
        if(!hasShotUp)
        {
            boss.RotateGun(Quaternion.Euler(0, 180, 90));
            shotgunBoss.ShootBullet();
            hasShotUp = true;
            return;
        }
        else
        {
            if(!hasShotBehind)
            {
                if(shootWaitTimer >= shootWaitTime)
                {
                    boss.RotateGun(Quaternion.Euler(0, 0, 0));
                    shotgunBoss.ShootBullet();
                    hasShotBehind = true;
                    return;
                }
                else
                {
                    shootWaitTimer += Time.deltaTime;
                    return;
                }
            }
            else
            {
                if(shotBehindTimer >= shotBehindTime)
                {
                    bossGun.canAutoAttack = true;
                    bossGun.canMove = true;
                }
                else
                    shotBehindTimer+= Time.deltaTime;

                if(!shotFromAbove)
                {
                    if(bulletAboveTimer >= bulletAboveTime)
                    {
                        shotFromAbove = true;
                        Vector2 pos = new(player.transform.position.x, CAMERA_MAX_HEIGHT);
                        Quaternion rotation = Quaternion.Euler(0, 0, -90);
                        shotgunBoss.ShootBullet(pos, rotation);
                        return;
                    }
                    else
                    {
                        bulletAboveTimer += Time.deltaTime;
                        return;
                    }
                }
                else
                {
                    if(bulletBehindTimer >= bulletBehindTime)
                    {
                        Vector2 pos = new(boss.CenterPoint.x - CAMERA_MAX_WIDTH + 5, player.transform.position.y);
                        Quaternion rotation = Quaternion.Euler(0, 0, 0);
                        shotgunBoss.ShootBullet(pos, rotation);
                        isAttackDone = true;
                        return;
                    }
                    else
                    {
                        bulletBehindTimer+= Time.deltaTime;
                        return;
                    }
                }
            }
        }
    }
}
