using System;
using UnityEngine;

[Serializable]
public abstract class BossAttack
{
    protected Boss boss;
    protected PlayerMovement player;
    protected Transform shootingPoint;
    protected BulletSO bullet;
    protected Enemy enemies;
    
    public bool IsAttackDone => isAttackDone;
    protected bool isAttackDone = false;

    public BossAttack(Boss boss, PlayerMovement player, Transform shootingPoint, BulletSO bullet)
    {
        this.boss = boss;
        this.player = player;
        this.shootingPoint = shootingPoint;
        this.bullet = bullet;
    }
    public BossAttack(Boss boss, PlayerMovement player, Transform shootingPoint, BulletSO bullet, Enemy enemies)
    {
        this.boss = boss;
        this.player = player;
        this.shootingPoint = shootingPoint;
        this.bullet = bullet;
        this.enemies = enemies;
    }

    public virtual void InitAttack() { isAttackDone = false; }

    public abstract void Attack();
    protected void InitBullet(Bullet bullet) => bullet.Init(this.bullet.speed, boss.gameObject.layer, this.bullet.damage * boss.DamageMultiplier);
}
