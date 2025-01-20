using UnityEngine;

public class EnemyMarksman : EnemyWeapon
{
    public override void Shoot(Vector3 dir, Quaternion aimRotation)
    {
        isShooting = true;
        Bullet bullet = Instantiate(bulletSO.bulletPrefab, transform.position + dir, aimRotation);
        bullet.Init(bulletSO.speed, gameObject.layer, bulletSO.damage);
    }
}
