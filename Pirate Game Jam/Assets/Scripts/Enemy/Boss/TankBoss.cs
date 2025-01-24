using System.Collections.Generic;
using UnityEngine;

public class TankBoss : Boss
{
    [SerializeField] BulletSO grenade;
    [SerializeField] BulletSO bullet;
    [SerializeField] Enemy enemyToSpawn;
    [SerializeField] Transform cannonPivot;

    protected override void InitBoss()
    {
        attacks = new() 
        { 
            new GrenadeBarrage(this, player, shootingPoint, grenade), 
            new BulletRain(this, player, shootingPoint, bullet), 
            // new SignalFlare(this, player, shootingPoint, grenade, enemyToSpawn),
            new SprayAndPray(this, player, shootingPoint, bullet),
        };
    }

    public override void RotateGun(Quaternion rotation) => cannonPivot.rotation = rotation;
    public override void TakeAim()
    {
        Vector2 shootDir = (player.transform.position - transform.position).normalized;
        Vector3 upwards = Vector3.Cross(Vector3.forward, shootDir);
        Vector3 forward = Vector3.forward;

        if(transform.rotation.y % 360 != 0)
        {
            forward = -forward;
            upwards = -upwards;
        }

        cannonPivot.rotation = Quaternion.LookRotation(forward, upwards);
    }
}
