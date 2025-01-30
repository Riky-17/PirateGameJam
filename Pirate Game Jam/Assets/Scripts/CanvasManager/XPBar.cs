using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalancedSliderController : MonoBehaviour
{
    public Slider slider;
    public static BalancedSliderController Instance { get; private set; }
    float excessEXP;
    int currentValue;

    const string maxLevel = "MAX";
    public static Action onLevelUp;

    [SerializeField] private TMP_Text currentXP;
    [SerializeField] private TMP_Text maxXP;
    [SerializeField] private TMP_Text currentLevelTxt;
    private int currentLevel = 1;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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


    void IncreaseNecessaryExp(float experience)
    {
        if(currentLevel <= 5)
        {
            //seting value back to 0
            slider.value = 0;
            currentXP.text = slider.value.ToString();
            slider.maxValue += experience;

            //increasing current lvl
            currentLevel++;
            onLevelUp?.Invoke();
            currentLevelTxt.text = currentLevel.ToString();

            //Add manual increasing stats

            //increasing max val
            maxXP.text = slider.maxValue.ToString();
            
            if(excessEXP != 0)
            {
                float tempVar = excessEXP;
                excessEXP = 0;
                IncreasingSliderValueSilent((int)tempVar);
            }
        }
        else
        {
            slider.value = slider.maxValue;
            maxXP.text = maxLevel;
            currentLevelTxt.text = maxLevel;
            currentXP.text = slider.maxValue.ToString();
        }
        
    }

    public void IncreasingSliderValue(int experience)
    {
        GameManager.Instance.totalEXP+= experience;
        if(slider.value + experience > slider.maxValue)
            excessEXP = slider.value + experience - slider.maxValue;
            
        slider.value += experience;
        currentXP.text = slider.value.ToString();

        if (slider.value >= slider.maxValue)
        {        
            IncreaseNecessaryExp((float)(slider.maxValue * 0.20));
        }
    }

    // this one does not change the game manager value
    public void IncreasingSliderValueSilent(int experience)
    {
        if(slider.value + experience > slider.maxValue)
            excessEXP = slider.value + experience - slider.maxValue;
            
        slider.value += experience;
        currentXP.text = slider.value.ToString();

        if (slider.value >= slider.maxValue)
        {        
            IncreaseNecessaryExp((float)(slider.maxValue * 0.20));
        }
    }

    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.V)) // For testing purposes
        {
            IncreasingSliderValue(100);
        }
        */
    }
}
