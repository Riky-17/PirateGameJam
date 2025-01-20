using UnityEngine;

public class EnemyShotgun : EnemyWeapon
{
    int bulletMaxAngles = 30;

    public override void Shoot(Vector3 dir, Quaternion aimRotation)
    {
        for (int bulletAngle = bulletMaxAngles; bulletAngle >= -bulletMaxAngles; bulletAngle-= 15)
        {
            Quaternion bulletRotation = aimRotation * Quaternion.Euler(0, 0, bulletAngle);
            Instantiate(bulletPrefab, transform.position, bulletRotation);
        }
    }
}
