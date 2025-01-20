using UnityEngine;

public class EnemyGrenadeLauncher : EnemyWeapon
{
    public override void Shoot(Vector3 dir, Quaternion aimRotation)
    {
        Instantiate(bulletPrefab, transform.position + dir, aimRotation);
    }
}
