using UnityEngine;

public class StatBoostItem : PickableItem
{
    [SerializeField] float speedBoostAmount = 5f;
    [SerializeField] float speedBoostDuration = 3f;

    public override void Effect(IItemPicker target)
    {
        if(target is PlayerMovement player)
            player.SpeedBoost(speedBoostAmount, speedBoostDuration);
        else if(target is Enemy enemy)
            enemy.SpeedBoost(speedBoostAmount, speedBoostDuration);
    }
}
