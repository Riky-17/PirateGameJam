using UnityEngine;

[CreateAssetMenu(fileName = "New pickUpItem", menuName = "ScriptableObject/PickUpItems")]
public class PickUpItemSO : ScriptableObject 
{
    public PickableItem item; 
}
