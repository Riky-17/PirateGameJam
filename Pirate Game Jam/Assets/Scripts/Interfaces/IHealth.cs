using UnityEngine;

public interface IHealth
{
    float Health { get; set;}

    public void Damage(int damageAmount);

    void Die();
}
