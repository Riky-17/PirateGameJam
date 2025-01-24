using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectivesManager : MonoBehaviour
{
    //Text
    [Header("UI")]
    [SerializeField] TMP_Text UIObjective1;
    [SerializeField] TMP_Text UIObjective2;
    [SerializeField] TMP_Text UIObjective3;

    //objectives 
    [Header("Objective1")]
    [SerializeField] private string objective1;
    [SerializeField] private byte missionObjective1;
    [Header("Objective2")]
    [SerializeField] private string objective2;
    [SerializeField] private byte missionObjective2;
    [Header("Objective3")]
    [SerializeField] private string objective3;
    [SerializeField] private byte missionObjective3;


    public static ObjectivesManager Instance { get; private set; }
    Dictionary<string, int> objectives = new Dictionary<string, int>();

    //necessaries amount for everything 
    private int progressObjective1 = 0;
    private int progressObjective2 = 0;
    private int progressObjective3 = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        UpdatingUI();

        //add random events for the dictionary here (later on) 

        UpdatingDictionaryObjectives();
    }
    void UpdatingUI()
    {
        UIObjective1.text = objective1;
        UIObjective2.text = objective2;
        UIObjective3.text = objective3;
    }
    public void killEnemy()
    {
        progressObjective1++;
        checkingObjectives();
    }
    public void KillCivilian()
    {
        progressObjective2++;
        checkingObjectives();
    }
    public void pickUpItem()
    {
        progressObjective3++;
        checkingObjectives();
    }
    void UpdatingDictionaryObjectives()
    {
        if (missionObjective1 != 0)
            objectives.Add(objective1, missionObjective1);

        if (missionObjective2 != 0)
            objectives.Add(objective2, missionObjective2);

        if (missionObjective3 != 0)
            objectives.Add(objective3, missionObjective3);

    }

    public void checkingObjectives()
    {
        if(progressObjective1 == missionObjective1)
        {
            BalancedSliderController.Instance.increasingSliderValue(missionObjective1 * 10);
        }
        if(progressObjective2 == missionObjective2)
        {
            BalancedSliderController.Instance.increasingSliderValue(missionObjective2 * 10);
        }
        if (progressObjective3 == missionObjective3)
        {
            BalancedSliderController.Instance.increasingSliderValue(missionObjective3 * 100);
        }
    }
    int UpdateDic(ref int value, ref bool objective, int pointsForMission)
    {
        return 0;
    }
    void Update()
    {
    }
}
