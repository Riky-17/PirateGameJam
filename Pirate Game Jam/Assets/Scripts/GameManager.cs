using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public List<Enemy> Enemies { get; private set; } = new();

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //Go to the main menu
        if(Input.GetKeyDown(KeyCode.M))
            SceneManager.LoadScene(0);
    }

    public void LoadScene(int sceneID) => SceneManager.LoadScene(sceneID);
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
}
