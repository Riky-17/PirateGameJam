using UnityEngine;

public abstract class LivingGunBossGun : MonoBehaviour
{
    SpriteRenderer sr;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    public abstract void Shoot();

    public void ChangeWeaponColor(Color color) => sr.color = color;
    public Color WeaponColorSprite() => sr.color;
}
