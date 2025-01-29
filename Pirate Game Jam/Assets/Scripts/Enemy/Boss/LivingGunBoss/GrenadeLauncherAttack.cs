using UnityEngine;

public class GrenadeLauncherAttack : BossAttack
{
    public GrenadeLauncherAttack(LivingGunBoss boss, PlayerMovement player, LivingGunBossGun grenadeLauncherBoss) : base(boss, player, null) 
    { 
        bossGun = boss; 
        this.grenadeLauncherBoss = grenadeLauncherBoss; 
    }

    const float CAMERA_MAX_HEIGHT = 17 / 2f;

    LivingGunBoss bossGun;
    LivingGunBossGun grenadeLauncherBoss;

    bool hasShotUp = false;

    bool hasWaited = false;

    float waitTime = .5f;
    float waitTimer;

    float grenadeDelay = 1.5f;
    float grenadeDelayTimer;

    float fireRateTime = .2f;
    float fireRateTimer;

    float attackDuration = 3f;
    float attackDurationTimer;

    public override void InitAttack()
    {
        base.InitAttack();
        bossGun.canAutoAttack = false;
        bossGun.canMove = false;
        bossGun.StopAngularVelocity();
        hasShotUp = false;
        hasWaited = false;
        waitTimer = 0;
        grenadeDelayTimer = 0;
        fireRateTimer = fireRateTime;
        attackDurationTimer = 0;
    }

    public override void Attack()
    {
        if(!hasShotUp)
        {
            boss.RotateGun(Quaternion.Euler(0, 0, 90));
            grenadeLauncherBoss.ShootBullet();
            hasShotUp = true;
            return;
        }
        else
        {
            if (!hasWaited)
            {
                if(waitTimer >= waitTime)
                {
                    bossGun.canAutoAttack = true;
                    bossGun.canMove = true;
                    hasWaited = true;
                }
                else
                {
                    waitTimer+= Time.deltaTime;
                    return;
                }
            }
            else
            {
                if(grenadeDelayTimer >= grenadeDelay)
                {
                    if(attackDurationTimer >= attackDuration)
                    {
                        isAttackDone = true;
                        return;
                    }
                    else
                        attackDurationTimer+= Time.deltaTime;

                    if(fireRateTimer >= fireRateTime)
                    {
                        float xOffset = Random.Range(-1f, 1f);
                        Vector2 pos = new(player.transform.position.x + xOffset, CAMERA_MAX_HEIGHT);
                        Quaternion rotation = Quaternion.Euler(0, 0, -90);
                        grenadeLauncherBoss.ShootBullet(pos, rotation);
                        fireRateTimer = 0;
                    }
                    else
                    {
                        fireRateTimer+= Time.deltaTime;
                        return;
                    }
                }
                else
                {
                    grenadeDelayTimer+= Time.deltaTime;
                    return;
                }
            }
        }
    }
}
