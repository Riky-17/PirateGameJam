using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    [SerializeField] protected BulletSO bulletSO;

    public float shootingTimer = 1.5f;

    public abstract void Shoot(Vector3 dir, Quaternion aimRotation); 
}
