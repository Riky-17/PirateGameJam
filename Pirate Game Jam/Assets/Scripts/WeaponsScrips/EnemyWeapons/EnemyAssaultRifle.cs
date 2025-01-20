using UnityEngine;

public class EnemyAssaultRifle : EnemyWeapon
{
    float sprayAngle = 20;

    // how quickly should the enemy fire for
    public float FireRate { get; private set; } = .1f;
    [HideInInspector] public float fireRateTimer;

    // for how long should the enemy fire for
    public float FireAmount { get; private set; } = 1.5f;
    [HideInInspector] public float fireTimer;

    public override void Shoot(Vector3 dir, Quaternion aimRotation)
    {
        float inaccuracy = Random.Range(-sprayAngle, sprayAngle);
        Quaternion bulletRotation = aimRotation * Quaternion.Euler(0, 0, inaccuracy);
        Instantiate(bulletPrefab, transform.position + dir, bulletRotation);
    }

    public void ResetTimers()
    {
        fireRateTimer = 0;
        fireTimer = 0;
    }
}
