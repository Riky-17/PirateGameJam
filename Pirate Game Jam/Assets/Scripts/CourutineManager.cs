using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class CourutineManager : MonoBehaviour
{
    public static CourutineManager Instance;
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
    }

    public void StartingCoroutine(IEnumerator cooldown)
    {        
        StartCoroutine(cooldown);
        return;
    }
}
