using UnityEngine;

public class EnemyMarksman : EnemyWeapon
{
    public override void Shoot(PlayerMovement player, Quaternion aimRotation)
    {
        this.player = player;
        isShooting = true;
        anim.Play("Shoot");
        Bullet bullet = Instantiate(bulletSO.bulletPrefab, shootingPoint.position, aimRotation);
        InitBullet(bullet);
        onShotFired?.Invoke(transform);
    }

    public override void Idle()
    {
        anim.Play("Idle");
    }
}
