using UnityEngine;

public class EnemyGrenadeLauncher : EnemyWeapon
{
    public override void Shoot(Vector3 dir, Quaternion aimRotation)
    {
        isShooting = true;
        anim.Play("Shoot");
        Bullet bullet = Instantiate(bulletSO.bulletPrefab, shootingPoint.position + dir, aimRotation);
        bullet.Init(bulletSO.speed, gameObject.layer, bulletSO.damage);
    }

    public override void Idle()
    {
        anim.Play("Idle");
    }
}
