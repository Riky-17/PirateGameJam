using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalancedSliderController : MonoBehaviour
{
    public Slider slider;
    public static BalancedSliderController Instance;
    int currentValue;


    [SerializeField] private TMP_Text currentXP;
    [SerializeField] private TMP_Text maxXP;
    [SerializeField] private TMP_Text currentLevelTxt;
    private int currentLevel = 1;
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

        maxXP.text = slider.maxValue.ToString();
        currentXP.text = slider.value.ToString();

    }


    void incNeccesaryExp(float experience)
    {
        //seting value back to 0
        slider.value = 0;
        currentXP.text = slider.value.ToString();
        slider.maxValue += experience;

        //increasing current lvl
        currentLevel++;
        currentLevelTxt.text = currentLevel.ToString();

        //increasing max val
        maxXP.text = slider.maxValue.ToString();
    }

    public void increasingSliderValue(int experience)
    {

        slider.value += experience;
        currentXP.text = slider.value.ToString();

        if (slider.value >= slider.maxValue)
        {        
            incNeccesaryExp((float)(slider.maxValue * 0.20));
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
