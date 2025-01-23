using UnityEngine;

public class DamageBoostItem : StatBoostItem
{
    public override void Effect(IItemPicker target)
    {
        if(target is PlayerMovement player)
        {
            player.DamageBoost(boostAmount, boostDuration);
            Destroy(gameObject);
        }
        else if(target is Enemy enemy)
        {
            enemy.DamageBoost(boostAmount, boostDuration);
            Destroy(gameObject);
        }
    }
}
