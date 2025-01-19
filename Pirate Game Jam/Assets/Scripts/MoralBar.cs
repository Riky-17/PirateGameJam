using UnityEngine;
using UnityEngine.UI;

public class BalancedSliderController : MonoBehaviour
{
    public RectTransform redFill; // Reference to the red fill
    public RectTransform blueFill; // Reference to the blue fill
    public float fillSpeed = 0.01f; // Speed at which the bar changes
    private float totalWidth; // Total width of the bar area
    private float redFillAmount = 0.5f; // Starting value for the red fill (50%)
    private bool isLocked = false; // To lock the bar once fully filled by one color

    void Start()
    {
        // Get the width of the entire bar area (total width)
        totalWidth = redFill.rect.width + blueFill.rect.width;

        // Initially set the fill to be 50/50
        UpdateFillBars();
    }

    void Update()
    {
        if (!isLocked)
        {
            if (Input.GetKey(KeyCode.Z)) // Press Z to make red expand
            {
                AdjustFillAmount(-fillSpeed); // Increase red fill
            }
            if (Input.GetKey(KeyCode.X)) // Press X to make blue expand
            {
                AdjustFillAmount(fillSpeed); // Increase blue fill
            }
        }
    }

    void AdjustFillAmount(float amount)
    {
        // Adjust the red fill amount (and implicitly blue fill amount)
        redFillAmount = Mathf.Clamp(redFillAmount + amount, 0f, 1f); // Keep the value between 0 and 1
        UpdateFillBars();
    }

    void UpdateFillBars()
    {
        // Update the size of the red and blue fills based on the percentage
        float redWidth = redFillAmount * totalWidth; // Red's width
        float blueWidth = (1f - redFillAmount) * totalWidth; // Blue's width

        // Set the width for the red and blue fills
        redFill.sizeDelta = new Vector2(redWidth, redFill.sizeDelta.y);
        blueFill.sizeDelta = new Vector2(blueWidth, blueFill.sizeDelta.y);

        // Lock if one color has completely taken over
        if (redFillAmount == 0f || redFillAmount == 1f)
        {
            isLocked = true;
        }
    }
}
