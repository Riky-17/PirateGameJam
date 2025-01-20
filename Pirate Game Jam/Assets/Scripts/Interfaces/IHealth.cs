using UnityEngine;

public interface IHealth
{
    float Health { get; set;}

    public void Damage(float damageAmount);

    void Die();
}
