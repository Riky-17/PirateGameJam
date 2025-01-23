using UnityEngine;
using UnityEngine.UI;

public class BalancedSliderController : MonoBehaviour
{
    public RectTransform middleBar; // Reference to the middle bar (small rectangle)
    public Slider slider;
    public static BalancedSliderController Instance;
    int currentValue;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        slider = GetComponent<Slider>();
        slider.value = 0f; //satart with no side 
    }

    public void AddingGoodSide(byte points)
    {
        points *= 10;
        if (ObjectivesManager.Instance.accompObjective1)
        {
            slider.value = currentValue + points;
            currentValue = (int)slider.value;
        }
    }
    public void AddingEvilSide(byte points)
    {
        points *= 10;
        if (ObjectivesManager.Instance.accompObjective2 || ObjectivesManager.Instance.accompObjective3)
        {
            slider.value = currentValue - points;
            currentValue = -(int)slider.value;
        }
    }

    private void FixedUpdate()
    {
       
    }
    void Update()
    {
        // Move the bar left (Z) or right (X) based on input
        //if (Input.GetKey(KeyCode.Z))
        //{
        //    currentPosition -= moveSpeed * Time.deltaTime; // Move left
        //}
        //if (Input.GetKey(KeyCode.X))
        //{
        //    currentPosition += moveSpeed * Time.deltaTime; // Move right
        //}

        //// Clamp the position to ensure it doesn't move out of bounds
        //currentPosition = Mathf.Clamp(currentPosition, minPosition, maxPosition);

        //// Update the middle bar's position
        //middleBar.anchorMin = new Vector2(currentPosition, middleBar.anchorMin.y);
        //middleBar.anchorMax = new Vector2(currentPosition, middleBar.anchorMax.y);
    }
}
