using UnityEngine;

public class EnemyAssaultRifle : EnemyWeapon
{
    float sprayAngle = 20;

    // how quickly should the enemy shoot
    public float FireRate { get; private set; } = .1f;
    [HideInInspector] public float fireRateTimer;

    // for how long should the enemy shoot
    public float FireAmount { get; private set; } = 1.5f;
    [HideInInspector] public float fireTimer;

    public override void Shoot(PlayerMovement player, Quaternion aimRotation)
    {
        this.player = player;
        isShooting = true;
        anim.SetTrigger("isShooting");
        float inaccuracy = Random.Range(-sprayAngle, sprayAngle);
        Quaternion bulletRotation = aimRotation * Quaternion.Euler(0, 0, inaccuracy);
        Bullet bullet = Instantiate(bulletSO.bulletPrefab, shootingPoint.position, bulletRotation);
        InitBullet(bullet);
        onShotFired?.Invoke(transform);
    }

    public void ResetTimers()
    {
        fireRateTimer = 0;
        fireTimer = 0;
        isShooting = false;
    }

    public override void Idle()
    {
        anim.Play("Idle");
    }
}
