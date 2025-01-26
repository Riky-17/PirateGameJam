using System;
using UnityEngine;

[Serializable]
public abstract class BossAttack
{
    protected Boss boss;
    protected PlayerMovement player;
    protected Transform shootingPoint;
    protected BulletSO bullet;
    
    public bool IsAttackDone => isAttackDone;
    protected bool isAttackDone = false;

    public BossAttack(Boss boss, PlayerMovement player, Transform shootingPoint, BulletSO bullet)
    {
        this.boss = boss;
        this.player = player;
        this.shootingPoint = shootingPoint;
        this.bullet = bullet;
    }

    public virtual void InitAttack() { isAttackDone = false; }

    public abstract void Attack();
    protected void InitBullet(Bullet bullet) => bullet.Init(this.bullet.speed, boss.gameObject, this.bullet.damage * boss.DamageMultiplier);
}
