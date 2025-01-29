using System;
using UnityEngine;

public abstract class LivingGunBossGun : MonoBehaviour
{
    SpriteRenderer sr;

    Boss boss;

    [SerializeField] protected BulletSO bulletSO;
    [SerializeField] protected Transform shootingPoint;

    [SerializeField] float fireRateBaseTime;
    protected float FireRateTime => fireRateBaseTime * boss.FireRateMultiplier;
    protected float fireRateTimer;

    public void InitGun(Boss boss)
    {
        sr = GetComponent<SpriteRenderer>();
        this.boss = boss;
        ResetFireRate();
    }

    public void ResetFireRate() => fireRateTimer = FireRateTime;

    public abstract void Shoot();
    public  void ShootBullet() => ShootBullet(shootingPoint.position, transform.rotation);
    public abstract void ShootBullet(Vector2 pos, Quaternion rotation);

    protected void InitBullet(Bullet bullet) => bullet.Init(bulletSO.speed, boss.gameObject, bulletSO.damage * boss.DamageMultiplier);
    public void ChangeWeaponColor(Color color) => sr.color = color;
    public Color WeaponColorSprite() => sr.color;
}
