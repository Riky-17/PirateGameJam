public class BulletAmountBoostItem : StatBoostItem
{
    public override void Effect(IItemPicker target)
    {
        if(target is PlayerMovement player)
        {
            player.BulletAmountBoost(boostAmount, boostDuration);
            Destroy(gameObject);
        }
    }
}
