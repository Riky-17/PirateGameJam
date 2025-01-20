using UnityEngine;

public class EnemyShotgun : EnemyWeapon
{
    int bulletMaxAngles = 30;

    public override void Shoot(Vector3 dir, Quaternion aimRotation)
    {
        isShooting = true;
        anim.Play("Shoot");

        for (int bulletAngle = bulletMaxAngles; bulletAngle >= -bulletMaxAngles; bulletAngle-= 15)
        {
            Quaternion bulletRotation = aimRotation * Quaternion.Euler(0, 0, bulletAngle);
            Bullet bullet = Instantiate(bulletSO.bulletPrefab, shootingPoint.position, bulletRotation);
            bullet.Init(bulletSO.speed, gameObject.layer, bulletSO.damage);
        }
    }

    public override void Idle()
    {
        anim.Play("Idle");
    }
}
