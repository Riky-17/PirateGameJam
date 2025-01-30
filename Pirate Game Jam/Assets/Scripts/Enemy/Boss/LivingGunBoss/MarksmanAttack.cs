using UnityEngine;

public class MarksmanAttack : BossAttack
{
    public MarksmanAttack(LivingGunBoss boss, PlayerMovement player, LivingGunBossGun marksmanBoss) : base(boss, player, null)
    {
        bossGun = boss;
        this.marksmanBoss = marksmanBoss;
    }

    const float CAMERA_MAX_HEIGHT = 17 / 2f;
    const float CAMERA_MAX_WIDTH = 30 / 2f;

    LivingGunBoss bossGun;
    LivingGunBossGun marksmanBoss;

    float angle = 180;
    float currentAngle;

    float rotationSpeed = 3;

    float backFireRateTime = .1f;
    float backFireRateTimer;

    Vector2 finalDir;

    float delay = 1.5f;
    float delayTimer;

    float attackDuration = 6;
    float attackDurationTimer;

    float fireRateTime = .2f;
    float fireRateTimer;

    public override void InitAttack()
    {
        base.InitAttack();
        bossGun.canAutoAttack = false;
        bossGun.canMove = false;
        bossGun.StopAngularVelocity();
        currentAngle = angle;
        backFireRateTimer = backFireRateTime;
        finalDir = new(1, -1);
        finalDir = finalDir.normalized;
        delayTimer = 0;
        attackDurationTimer = 0;
        fireRateTimer = fireRateTime;
    }

    public override void Attack()
    {
        if(currentAngle > 0)
        {
            float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad);

            Vector2 shootDir = new(y, x);
            Vector3 forward = Vector3.forward;
            Vector3 upward = Vector3.Cross(forward, shootDir);
            boss.RotateGun(Quaternion.Euler(0, 180, 0) * Quaternion.LookRotation(forward, upward));
            currentAngle-= rotationSpeed * 30 * Time.deltaTime;

            if (backFireRateTimer >= backFireRateTime)
            {
                backFireRateTimer = 0;
                marksmanBoss.ShootBullet();
            }
            else
                backFireRateTimer+= Time.deltaTime;

            return;
        }
        else
        {
            if(delayTimer >= delay)
            {
                bossGun.canMove = true;
                bossGun.canAutoAttack = true;

                if(attackDurationTimer < attackDuration)
                {
                    attackDurationTimer+= Time.deltaTime;

                    if(fireRateTimer >= fireRateTime)
                    {
                        fireRateTimer = 0;
                        float y = Random.Range(-CAMERA_MAX_HEIGHT, CAMERA_MAX_HEIGHT);
                        Vector2 pos = new(boss.CenterPoint.x - CAMERA_MAX_WIDTH + 5, y);
                        Quaternion rotation = Quaternion.Euler(0, 0, 0);
                        marksmanBoss.ShootBullet(pos, rotation);
                        return;
                    }
                    else
                    {
                        fireRateTimer+= Time.deltaTime;
                        return;
                    }
                }
                else
                {
                    isAttackDone = true;
                    return;
                }
            }
            else
            {
                delayTimer+= Time.deltaTime;
                return;
            }
        }
    }
}
