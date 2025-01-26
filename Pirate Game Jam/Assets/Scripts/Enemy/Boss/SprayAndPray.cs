using Unity.VisualScripting;
using UnityEngine;

public class SprayAndPray : BossAttack
{
    public SprayAndPray(Boss boss, PlayerMovement player, Transform shootingPoint, BulletSO bullet) : base(boss, player, bullet) {}

    const float CAMERA_MAX_WIDTH = 30 / 2;

    Vector2 dir;
    int slamCount;

    float fireRateTime = .1f;
    float fireRateTimer;

    //trigonometry fields
    int angDegSpray = 180;
    float currentDegAngle;

    float CosAngSpray => Mathf.Cos(currentDegAngle * Mathf.Deg2Rad);
    float SinAngSpray => Mathf.Sin(currentDegAngle * Mathf.Deg2Rad);

    float rotationSpeed = 90f;

    public override void InitAttack()
    {
        base.InitAttack();
        boss.Stop();
        slamCount = 0;
        fireRateTimer = fireRateTime;
        
        dir = new(boss.CenterPoint.x - boss.transform.position.x, 0);
        dir = dir.normalized;
    }

    public override void Attack()
    {
        if(slamCount >= 2 && boss.transform.position.x >= boss.CenterPoint.x - .5f && boss.transform.position.x <= boss.CenterPoint.x + .5f)
        {
            isAttackDone = true;
            return;
        }

        boss.AddForceBoss(dir, 15, 2);
        if(boss.transform.position.x < boss.CenterPoint.x - CAMERA_MAX_WIDTH && dir.x < 0 || boss.transform.position.x > boss.CenterPoint.x + CAMERA_MAX_WIDTH && dir.x > 0)
        {
            slamCount++;
            dir = -dir;
        }

        //rotate
        if((currentDegAngle <= 0 && Mathf.Sign(rotationSpeed) == 1) || (currentDegAngle >= angDegSpray && Mathf.Sign(rotationSpeed) == -1))
            rotationSpeed = -rotationSpeed;
        
        currentDegAngle-= rotationSpeed * Time.deltaTime;

        Vector2 shootDir = new(CosAngSpray, SinAngSpray);
        Vector3 upwards = Vector3.Cross(Vector3.forward, shootDir);
        Vector3 forward = Vector3.forward;

        boss.RotateGun(Quaternion.LookRotation(forward, upwards));

        if(fireRateTimer >= fireRateTime)
        {
            fireRateTimer = 0;
            Bullet bulletToShoot = boss.InstantiateBullet(bullet.bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            InitBullet(bulletToShoot);
            boss.DestroyBullet(bulletToShoot, 1.5f);
        }
        else
            fireRateTimer+= Time.deltaTime;
    }
}
