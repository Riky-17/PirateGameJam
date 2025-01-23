using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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

   
    //objectives accomplished
    private bool accompObjective1 = false;
    private bool accompObjective2 = false;
    private bool accompObjective3 = false;
    

    public static ObjectivesManager Instance { get; private set; }

    byte objectivenum = 0;
    Dictionary<byte, byte> objectives = new Dictionary<byte, byte>();
    List<byte> objectiveID = new List<byte>();
    private int initialEnemiesNum;
    private int initialCiviliansNum;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //setting the initial amount of enemies
        if (GameManager.Instance.Enemies != null)
        {
            initialEnemiesNum = GameManager.Instance.Enemies.Count;
        }
        if(GameManager.Instance.Civilians != null)
        {
            initialCiviliansNum = GameManager.Instance.Civilians.Count;
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

    void UpdatingDictionaryObjectives()
    {
        if (missionObjective1 != 0)
        {
            objectivenum++;
            objectiveID.Add(objectivenum);
            objectives.Add(objectivenum, missionObjective1);
        }
        if (missionObjective2 != 0)
        {
            objectivenum++;
            objectiveID.Add(objectivenum);
            objectives.Add(objectivenum, missionObjective2);
        }
        if (missionObjective3 != 0)
        {
            objectivenum++;
            objectiveID.Add(objectivenum);
            objectives.Add(objectivenum, missionObjective3);
        }
    }

    public void checkingObjectives()
    {
        if (initialEnemiesNum != GameManager.Instance.Enemies.Count && !accompObjective1)
        {
            if (objectives.TryGetValue(1, out byte value))
            {
                UpdateDic(ref value, ref accompObjective1);
            }
        }
        if (initialCiviliansNum != GameManager.Instance.Civilians.Count && !accompObjective2)
        {
            if (objectives.TryGetValue(2, out byte value))
            {
                UpdateDic(ref value, ref accompObjective2);
            }
        }
        
    }
    byte UpdateDic(ref byte value, ref bool objective)
    {
        Debug.Log("Mehod UpdateDic was reached");
        value--;
        initialCiviliansNum = GameManager.Instance.Civilians.Count;
        initialEnemiesNum = GameManager.Instance.Enemies.Count;
        if (value <= 0)
        {
            Debug.Log("A mission was accomplished");
            objective = true;
        }
        return value;
    }
    void Update()
    {
    }
}
