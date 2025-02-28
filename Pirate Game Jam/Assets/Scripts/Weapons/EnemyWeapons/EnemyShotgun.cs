using System;
using UnityEngine;

public class EnemyShotgun : EnemyWeapon
{
    public static Action<Transform> onShotgunPump;

    int bulletMaxAngles = 30;

    public override void Shoot(PlayerMovement player, Quaternion aimRotation)
    {
        this.player = player;
        isShooting = true;
        anim.Play("Shoot");

        for (int bulletAngle = bulletMaxAngles; bulletAngle >= -bulletMaxAngles; bulletAngle-= 15)
        {
            Quaternion bulletRotation = aimRotation * Quaternion.Euler(0, 0, bulletAngle);
            Bullet bullet = Instantiate(bulletSO.bulletPrefab, shootingPoint.position, bulletRotation);
            InitBullet(bullet);
            onShotFired?.Invoke(transform);
        }

        onShotgunPump?.Invoke(transform);
    }

    public override void Idle()
    {
        anim.Play("Idle");
    }
}
