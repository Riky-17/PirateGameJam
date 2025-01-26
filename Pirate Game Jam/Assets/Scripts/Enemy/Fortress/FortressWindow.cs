using UnityEngine;

public class FortressWindow
{
    public Transform Transform => transform;
    Transform transform;
    protected BulletSO bulletSO;
    
    protected float fireRateBaseTime;
    float fireRateMultiplier = 1;
    protected float FireRateTime => fireRateBaseTime * fireRateMultiplier;
    protected float fireRateTimer;

    protected Boss boss;

    public bool IsSpecialAttacking => isSpecialAttacking;
    public bool isSpecialAttacking = false;

    public FortressWindow(Boss boss, Transform transform, BulletSO bulletSO)
    {
        this.boss = boss;
        this.transform = transform;
        this.transform.rotation = Quaternion.Euler(0, 180, 0);
        this.bulletSO = bulletSO;
    }

    public void TakeAim(PlayerMovement player)
    {
        Vector2 shootDir = (player.transform.position - transform.position).normalized;
        Vector2 upward = Vector3.Cross(Vector3.forward, shootDir);
        Vector3 forward = Vector3.forward;

        transform.rotation = Quaternion.LookRotation(forward, upward);
    }

    public virtual void Shoot()
    {
        Bullet bullet = boss.InstantiateBullet(bulletSO.bulletPrefab, transform.position + transform.right, transform.rotation);
        InitBullet(bullet);
    }

    public void Overclock(float overclock) => fireRateMultiplier = overclock;

    protected void InitBullet(Bullet bullet) => bullet.Init(bulletSO.speed, boss.gameObject, bulletSO.damage);
}
