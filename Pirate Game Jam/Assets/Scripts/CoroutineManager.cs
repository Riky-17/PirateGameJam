using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance { get; private set;}
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
