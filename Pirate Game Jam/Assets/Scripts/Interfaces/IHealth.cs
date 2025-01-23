public interface IHealth
{
    float MaxHealth { get; set;}
    float Health { get; set;}

    public void Heal(float healAmount);
    public void Damage(float damageAmount);

    void Die();
}
