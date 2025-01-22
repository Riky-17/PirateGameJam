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

    public Coroutine StartingCoroutine(IEnumerator cooldown)
    {
        return StartCoroutine(cooldown);
    }

    public void StoppingCoroutine(Coroutine coroutine) => StopCoroutine(coroutine);

}
