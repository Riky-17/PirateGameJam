using UnityEngine;

public abstract class StatBoostItem : PickableItem
{
    [SerializeField] protected float boostAmount = 1.5f;
    [SerializeField] protected float boostDuration = 3f;
}
