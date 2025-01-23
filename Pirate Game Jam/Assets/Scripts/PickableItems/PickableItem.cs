using UnityEngine;

public abstract class PickableItem : MonoBehaviour
{
    public abstract void Effect(IItemPicker target);

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent(out IItemPicker picker))
            picker.PickItem(this);
    }
}
