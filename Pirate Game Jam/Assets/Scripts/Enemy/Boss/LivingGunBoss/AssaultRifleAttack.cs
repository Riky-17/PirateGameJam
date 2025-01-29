using UnityEngine;

public class AssaultRifleAttack : BossAttack
{
    public AssaultRifleAttack(LivingGunBoss boss, PlayerMovement player, LivingGunBossGun assaultRifleBoss) : base(boss, player, null) 
    { 
        bossGun = boss; 
        this.assaultRifleBoss = assaultRifleBoss; 
    }

    const float CAMERA_MAX_HEIGHT = 12 / 2f;

    LivingGunBoss bossGun;
    LivingGunBossGun assaultRifleBoss;

    int dashAmount = 5;
    int dashCount;

    bool isHighEnough = false;
    
    Vector2 dashTarget;
    float DashDist => (dashTarget - (Vector2)boss.transform.position).magnitude;

    float shootingTime = 1;
    float shootingTimer;

    float fireRateTime = .1f;
    float fireRateTimer;

    public override void InitAttack()
    {
        base.InitAttack();
        bossGun.canAutoAttack = false;
        bossGun.canMove = false;
        bossGun.StopAngularVelocity();
        boss.RotateGun(Quaternion.Euler(0, 180, 0));
        isHighEnough = false;
        dashTarget = dashTarget = new(boss.transform.position.x, Random.Range(1, CAMERA_MAX_HEIGHT));
        shootingTimer = 0;
        fireRateTimer = fireRateTime;
        dashCount = 0;
    }

    public override void Attack()
    {
        if(dashCount >= dashAmount)
        {
            isAttackDone = true;
            bossGun.canAutoAttack = true;
            bossGun.canMove = true;
            return;
        }

        if(isHighEnough)
        {
            if (shootingTimer >= shootingTime)
            {
                shootingTimer = 0;
                fireRateTimer = fireRateTime;
                isHighEnough = false;

                if(dashCount % 2 != 0)
                    dashTarget = new(boss.transform.position.x, Random.Range(1, CAMERA_MAX_HEIGHT));
                else
                    dashTarget = new(boss.transform.position.x, Random.Range(-CAMERA_MAX_HEIGHT, -1));

                dashCount++;
                return;
            }
            else
            {
                shootingTimer+= Time.deltaTime;

                if(fireRateTimer >= fireRateTime)
                {
                    fireRateTimer = 0;
                    assaultRifleBoss.ShootBullet();
                    return;
                }
                else
                    fireRateTimer+= Time.deltaTime;
            }
        }
        else
        {
            if(DashDist < .1f)
                isHighEnough = true;
            else
            {
                Vector2 moveDir = dashTarget - (Vector2)boss.transform.position;
                moveDir = moveDir.normalized;
                boss.AddForceBoss(moveDir, 20, 15);
            }
        }
    }
}
