using UnityEngine;
using UnityEngine.UI;

public class BalancedSliderController : MonoBehaviour
{
    public RectTransform middleBar; // Reference to the middle bar (small rectangle)
    public float moveSpeed = 0.01f; // Speed at which the bar moves
    public float minPosition = 0f; // Minimum position (leftmost)
    public float maxPosition = 1f; // Maximum position (rightmost)

    private float currentPosition = 0.5f; // Start at the middle

    void Update()
    {
        // Move the bar left (Z) or right (X) based on input
        if (Input.GetKey(KeyCode.Z))
        {
            currentPosition -= moveSpeed * Time.deltaTime; // Move left
        }
        if (Input.GetKey(KeyCode.X))
        {
            currentPosition += moveSpeed * Time.deltaTime; // Move right
        }

        // Clamp the position to ensure it doesn't move out of bounds
        currentPosition = Mathf.Clamp(currentPosition, minPosition, maxPosition);

        // Update the middle bar's position
        middleBar.anchorMin = new Vector2(currentPosition, middleBar.anchorMin.y);
        middleBar.anchorMax = new Vector2(currentPosition, middleBar.anchorMax.y);
    }
}
