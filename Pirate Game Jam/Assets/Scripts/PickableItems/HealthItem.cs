using UnityEngine;

public class HealthItem : PickableItem
{
    [SerializeField] float healAmount;

    public override void Effect(IItemPicker target)
    {
        if(target is IHealth alive && alive.Health < alive.MaxHealth)
        {
            alive.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
