using UnityEngine;

[CreateAssetMenu(fileName = "NewBullet", menuName = "ScriptableObject/Bullet")]
public class BulletSO : ScriptableObject
{
    public Bullet bulletPrefab;
    public float speed = 3f;
    public float damage = 10f;
}
