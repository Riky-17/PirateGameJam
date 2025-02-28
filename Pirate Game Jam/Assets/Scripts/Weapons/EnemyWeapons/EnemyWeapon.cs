using System;
using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    public static Action<Transform> onShotFired;

    protected Animator anim;

    protected GameObject attachedEnemy;
    protected PlayerMovement player;
    protected bool isShooting;

    [SerializeField] protected BulletSO bulletSO;
    [HideInInspector] public float damageMultiplier = 1;

    public Transform shootingPoint;

    public float shootingTimer;
    public float lastShot;

    public abstract void Shoot(PlayerMovement player, Quaternion aimRotation);
    public abstract void Idle();
    protected void InitBullet(Bullet bullet) => bullet.Init(bulletSO.speed, attachedEnemy, bulletSO.damage * damageMultiplier);

    void Awake()
    {
        anim = GetComponent<Animator>();
        attachedEnemy = transform.parent.gameObject;
    }

    public void SpriteRotation(Quaternion aimRot) => transform.rotation = aimRot;

    public void Reset() => transform.rotation = attachedEnemy.transform.rotation;
}
