using UnityEngine;

public class Obstacle : MonoBehaviour, IHealth
{
    public float MaxHealth { get; set; } = 30;
    public float Health { get; set; }

    void Awake() => Health = MaxHealth;

    //obstacles don't heal
    public void Heal(float healAmount) {}

    public void Damage(float damageAmount)
    {
        Health -= damageAmount;
        if(Health <= 0)
            Die();
    }

    //TODO
    public void Die()
    {
        Destroy(gameObject);
    }
}
