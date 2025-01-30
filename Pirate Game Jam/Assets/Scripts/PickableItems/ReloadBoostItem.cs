public class ReloadBoostItem : StatBoostItem
{
    public override void Effect(IItemPicker target)
    {
        if(target is PlayerMovement player)
        {
            player.ReloadBoost(boostAmount, boostDuration);
            Destroy(gameObject);
        }
    }
}
