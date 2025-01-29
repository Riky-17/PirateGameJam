using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<Enemy> Enemies { get; private set; } = new();
    public List<BossParachuteEnemy> BossEnemies { get; private set; } = new();
    public List<Civilian> Civilians { get; private set; } = new();

    public int totalEXP;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void OnEnable() => SceneManager.sceneLoaded += GiveExpToPlayer;
    void OnDisable() => SceneManager.sceneLoaded -= GiveExpToPlayer;

    void Update()
    {
        //Go to the main menu
        if(Input.GetKeyDown(KeyCode.M))
            SceneManager.LoadScene(0);
    }

    public void ClearBossEnemies()
    {
        for (int i = BossEnemies.Count - 1; i >= 0; i--)
        {
            BossParachuteEnemy bossEnemy = BossEnemies[i];
            bossEnemy.Die();
        }
    }

    private void GiveExpToPlayer(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log(totalEXP);
        BalancedSliderController.Instance.IncreasingSliderValueSilent(totalEXP);
    }

    public void LoadScene(int sceneID) => SceneManager.LoadScene(sceneID);
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
}
