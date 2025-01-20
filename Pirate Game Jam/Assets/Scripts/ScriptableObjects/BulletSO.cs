using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "Bullet SO")]
public class BulletSO : ScriptableObject
{
    public Bullet bulletPrefab;
    public float speed = 3f;
    public float damage = 10f;
}
