using UnityEngine;
using UnityEngine.UI;

public class BalancedSliderController : MonoBehaviour
{
    public Slider balanceSlider; // Reference to the slider
    public RectTransform redFill; // Reference to the red fill
    public RectTransform blueFill; // Reference to the blue fill

    private float sliderWidth; // The width of the slider's fill area

    void Start()
    {
        // Get the width of the slider's Fill Area (the RectTransform of the Fill)
        sliderWidth = balanceSlider.GetComponent<RectTransform>().rect.width;

        // Set the slider to start at the middle (50/50)
        balanceSlider.value = 0.5f;

        // Update the fill visuals initially
        UpdateFills();
    }

    public void ChangeBalance(float amount)
    {
        // Adjust the slider's value
        balanceSlider.value = Mathf.Clamp(balanceSlider.value + amount, 0f, 1f);

        // Update the fills based on the new value
        UpdateFills();
    }

    private void UpdateFills()
    {
        // Calculate the width for each fill based on the slider value
        float redWidth = balanceSlider.value * sliderWidth;
        float blueWidth = (1 - balanceSlider.value) * sliderWidth;

        // Update the size of the fills (ensure they stay within the slider)
        redFill.sizeDelta = new Vector2(redWidth, redFill.sizeDelta.y);
        blueFill.sizeDelta = new Vector2(blueWidth, blueFill.sizeDelta.y);
    }
}
