using UnityEngine;

public class FireRateBoostItem : StatBoostItem
{
    public override void Effect(IItemPicker target)
    {
        if(target is PlayerMovement player)
        {
            player.FireRateBoost(boostAmount, boostDuration);
            Destroy(gameObject);
        }
        else if(target is Enemy enemy)
        {
            enemy.FireRateBoost(boostAmount, boostDuration);
            Destroy(gameObject);
        }
    }
}
