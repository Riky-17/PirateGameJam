using UnityEngine;

public interface IHealth
{
    float MaxHealth { get; set;}
    float Health { get; set;}

    public void Damage(float damageAmount);

    void Die();
}
