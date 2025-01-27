using UnityEngine;

public abstract class LivingGunBossGun : MonoBehaviour
{
    public SpriteRenderer Sr { get; private set; }

    void Awake() => Sr = GetComponent<SpriteRenderer>();

    public abstract void Shoot();
}
